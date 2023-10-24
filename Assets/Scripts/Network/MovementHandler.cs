using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : NetworkBehaviour
{
    Vector2 inputDirection = Vector2.zero;

    [Networked(OnChanged = nameof(OnSizeChanged))]
    ushort size { get; set; } // Max 65,535

    private CharacterController _controller;

    public float PlayerSpeed = 2f;

    private bool isFacingLeft = true;
    private bool isFacingRight;
    private bool isWalking;
    private bool runSoundPlaying = false;
    private const string IS_WALKING = "IsWalking";

    // Other components
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody2D_;
    BoxCollider2D boxCollider2D;
    Animator animator;


    void Awake()
    {
        animator = GetComponent<NetworkMecanimAnimator>().GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigidbody2D_ = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponentInChildren<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Move in a random direction
        inputDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));

        Reset();
        UpdateSize();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {
            inputDirection = networkInputData.movementInput;
        }

        // Server moves the network objects
        if (Object.HasStateAuthority)
        {
            Vector2 movementDirection = inputDirection;

            movementDirection.Normalize();

            float movementSpeed = (size / Mathf.Pow(size, 1.05f)) * 3;

            rigidbody2D_.velocity = movementDirection * movementSpeed;

            // Handle flip
            if (inputDirection.x > 0 && isFacingLeft)
            {
                spriteRenderer.flipX = true;
                isFacingLeft = false;
                isFacingRight = true;
            }

            if (inputDirection.x < 0 && isFacingRight)
            {
                spriteRenderer.flipX = false;
                isFacingLeft = true;
                isFacingRight = false;
            }

            // Animations
            animator.SetBool(IS_WALKING, IsWalking());

            CollisionCheck();
        }

        if (Object.HasInputAuthority)
        {
            isWalking = inputDirection != Vector2.zero;

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
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(spriteRenderer.transform.position.x, spriteRenderer.transform.position.y, -10), Time.deltaTime);
        }
    }

    public void Reset()
    {
        size = 1;
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
                // Move food to new location
                hitCollider.transform.position = Utils.GetRandomSpawnPosition();

                OnCollectFood(25);
            }
        }
    }

    void UpdateSize()
    {
        spriteRenderer.transform.localScale = Vector3.one + Vector3.one * 100 * (size / 65535f);
    }

    void OnCollectFood(ushort growSize)
    {
        size += growSize;

        GetComponent<NetworkPlayer>().NetFoodEaten++;

        if (GetComponent<NetworkPlayer>().NetFoodEaten < 100)
        {
            UpdateSize();
            //Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, Camera.main.orthographicSize + 8, Time.deltaTime);
        }
    }

    public static void OnSizeChanged(Changed<MovementHandler> changed)
    {
        changed.Behaviour.UpdateSize();
    }

}
