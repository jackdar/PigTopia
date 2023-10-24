using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : NetworkBehaviour
{
    Vector2 inputDirection = Vector2.zero;

    [Networked(OnChanged = nameof(OnSizeChanged))]
    ushort NetSize { get; set; } // Max 65,535

    [Networked(OnChanged = nameof(OnCharacterFlip))]
    NetworkBool NetIsFlipped { get; set; }

    //[Networked(OnChanged = nameof(OnCharacterSprint))]
    //NetworkBool NetIsSprinting { get; set; }

    private CharacterController _controller;

    private NetworkPlayer player;

    private float playerSpeed = 3f;
    private float playerSprintSpeed = 4f;

    private bool isFacingLeft = true;
    private bool isFacingRight;
    private bool isWalking = false;
    private bool isSprinting = false;
    public bool runSoundPlaying = false;

    // Other components
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody2D_;
    BoxCollider2D boxCollider2D;
    NetworkMecanimAnimator networkAnimator;
    PlayerInputActions playerInputActions;

    void Awake()
    {
        player = GetComponent<NetworkPlayer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigidbody2D_ = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponentInChildren<BoxCollider2D>();
        networkAnimator = GetComponent<NetworkMecanimAnimator>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Move in a random direction
        inputDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));

        Reset();
        UpdateSize();
    }

    //static void OnCharacterSprint(Changed<MovementHandler> changed)
    //{
    //    changed.Behaviour.OnCharacterSprint();
    //}

    //void OnCharacterSprint()
    //{

    //}

    static void OnCharacterFlip(Changed<MovementHandler> changed)
    {
        changed.Behaviour.OnCharacterFlip();
    }

    void OnCharacterFlip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void SprintPressed()
    {
        if (player.NetStamina > 5f)
        {
            isSprinting = true;
        }
    }

    private void SprintReleased()
    {
        isSprinting = false;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {
            inputDirection = networkInputData.movementInput;
            
            if (isSprinting && player.NetStamina > 0)
                player.NetStamina -= 50f * Time.deltaTime;
            if (player.NetStamina == 0)
                SprintReleased();
            if (!isSprinting && (player.NetStamina <= player.NetMaxStamina))
                player.NetStamina += 10f * Time.deltaTime;

            playerInputActions.Player.SprintStart.performed += x => SprintPressed();
            playerInputActions.Player.SprintFinish.performed += x => SprintReleased();

            isWalking = inputDirection != Vector2.zero;

            // Animations
            if (isWalking)
                networkAnimator.Animator.SetTrigger("Walk");
            else
                networkAnimator.Animator.SetTrigger("Idle");
        }

        // Server moves the network objects
        if (Object.HasStateAuthority)
        {
            Vector2 movementDirection = inputDirection;

            movementDirection.Normalize();

            float movementSpeed = (NetSize / Mathf.Pow(NetSize, 1.05f)) * (isSprinting ? playerSprintSpeed : playerSpeed);

            rigidbody2D_.velocity = movementDirection * movementSpeed;

            // Handle flip
            if (inputDirection.x > 0 && isFacingLeft)
            {
                NetIsFlipped = true;
                isFacingLeft = false;
                isFacingRight = true;
            }

            if (inputDirection.x < 0 && isFacingRight)
            {
                NetIsFlipped = false;
                isFacingLeft = true;
                isFacingRight = false;
            }

            CollisionCheck();
        }

        if (Object.HasInputAuthority)
        {
            // SFX
            if (isWalking && !runSoundPlaying)
            {
                GetComponent<NetworkPlayer>().RPC_PlayRunSound(GetComponent<NetworkPlayer>());
                runSoundPlaying = true;
            }

            if (!isWalking && runSoundPlaying)
            {
                GetComponent<NetworkPlayer>().RPC_StopPlayingRunSound(GetComponent<NetworkPlayer>());
                runSoundPlaying = false;
            }
        }
    }

    void LateUpdate()
    {
        if (Object.HasInputAuthority)
        {
            Camera.main.transform.position = Vector3.Lerp(
                Camera.main.transform.position,
                new Vector3(spriteRenderer.transform.position.x,
                spriteRenderer.transform.position.y, -10),
                isSprinting ? Time.deltaTime * 2 : Time.deltaTime);
        }
    }

    public void Reset()
    {
        NetSize = 1;
    }

    void CollisionCheck()
    {
        // Disable own collider so we don't detect player self
        boxCollider2D.enabled = false;

        Collider2D hitCollider = Runner.GetPhysicsScene2D().OverlapCircle(spriteRenderer.transform.position, (spriteRenderer.transform.localScale.x / 2f) * 0.8f);

        // Enable own collider again so others can hit us
        boxCollider2D.enabled = true;

        if (hitCollider != null)
        {
            if (hitCollider.CompareTag("Food"))
            {
                // Pop sound
                hitCollider.gameObject.GetComponent<AudioSource>().PlayOneShot(hitCollider.gameObject.GetComponent<AudioSource>().clip, 1.0f);

                // Move food to new location
                hitCollider.transform.position = Utils.GetRandomSpawnPosition();

                OnCollectFood(25);
            }

            if (hitCollider.CompareTag("HealthPack"))
            {
                if (player.NetHealth < player.NetMaxHealth)
                {
                    // Pop sound
                    hitCollider.gameObject.GetComponent<AudioSource>().PlayOneShot(hitCollider.gameObject.GetComponent<AudioSource>().clip, 1.0f);

                    // Move food to new location
                    hitCollider.transform.position = Utils.GetRandomSpawnPosition();

                    OnCollectHealthPack();
                }
            }
        }
    }

    void UpdateSize()
    {
        spriteRenderer.transform.localScale = Vector3.one + Vector3.one * 100 * (NetSize / 65535f);
    }

    void OnCollectFood(ushort growSize)
    {
        player.NetFoodEaten++;

        if (player.NetFoodEaten < 100)
        {
            NetSize += growSize;
            UpdateSize();
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, Camera.main.orthographicSize + 9, Time.deltaTime);
        }
    }

    void OnCollectHealthPack()
    {
        if (player.NetHealth < (player.NetMaxHealth - 5f))
        {
            player.NetHealth += 5f;
        }
        else
        {
            player.NetHealth = player.NetMaxHealth;
        }
    }
    public static void OnSizeChanged(Changed<MovementHandler> changed)
    {
        changed.Behaviour.UpdateSize();
    }

}
