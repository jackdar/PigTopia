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

    public float playerSpeed = 2f;

    // Other components
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody2D_;
    BoxCollider2D box_collider; 

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigidbody2D_ = GetComponent<Rigidbody2D>();
        _controller = GetComponent<CharacterController>();
        box_collider = GetComponentInChildren<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Move in a random direction
        inputDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));

        Reset();
        UpdateSize();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
            inputDirection = networkInputData.movementInput;

        // Server moves the network objects
        if (Object.HasStateAuthority)
        {
            Vector2 movementDirection = inputDirection;

            // Keep the player within the playfield
            if (transform.position.x < Utils.GetPlayfieldSize() / 2f * -1 + spriteRenderer.transform.localScale.x / 2f && movementDirection.x < 0)
            {
                movementDirection.x = 0;
                rigidbody2D_.velocity = new Vector2(0, rigidbody2D_.velocity.y);
            }
            
            if (transform.position.x < Utils.GetPlayfieldSize() / 2f - spriteRenderer.transform.localScale.x / 2f && movementDirection.x < 0)
            {
                movementDirection.x = 0;
                rigidbody2D_.velocity = new Vector2(0, rigidbody2D_.velocity.y);
            }
            
            if (transform.position.y < Utils.GetPlayfieldSize() / 2f * -1 + spriteRenderer.transform.localScale.y / 2f && movementDirection.y < 0)
            {
                movementDirection.y = 0;
                rigidbody2D_.velocity = new Vector2(rigidbody2D_.velocity.x, 0);
            }
            
            if (transform.position.y < Utils.GetPlayfieldSize() / 2f - spriteRenderer.transform.localScale.y / 2f && movementDirection.y < 0)
            {
                movementDirection.y = 0;
                rigidbody2D_.velocity = new Vector2(rigidbody2D_.velocity.x, 0);
            }
            
            movementDirection.Normalize();

            float movementSpeed = (size / Mathf.Pow(size, 1.1f)) * 2;

            // Push the object in a given direction
            rigidbody2D_.AddForce(movementDirection * movementSpeed, ForceMode2D.Impulse);

            //if (rigidbody2D_.velocity.magnitude > movementSpeed)
            //    rigidbody2D_.velocity = rigidbody2D_.velocity.normalized * movementSpeed;

            rigidbody2D_.velocity = movementDirection * playerSpeed;

            CollisionCheck();
        }
    }

    void LateUpdate()
    {
        if (Object.HasInputAuthority)
        {
            float aspectRatio = Camera.main.aspect;
            float orthoSize = (spriteRenderer.transform.localScale.x + 7) / aspectRatio;

            //Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, orthoSize, Time.deltaTime * 0.1f);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(spriteRenderer.transform.position.x, spriteRenderer.transform.position.y, -10), Time.deltaTime);
        }
    }

    public void Reset()
    {
        size = 1;
    }

    void CollisionCheck()
    {
        box_collider.enabled = false; 
        Collider2D hit = Runner.GetPhysicsScene2D().OverlapCircle(spriteRenderer.transform.position, (spriteRenderer.transform.localScale.x / 2f) * 0.8f);
        box_collider.enabled = true; 

        if (hit != null)
        {
            if (hit.CompareTag("Food"))
            {
                hit.transform.position = Utils.GetRandomSpawnPosition();
                OnCollectFood(5);
            }
        }
    }

    void UpdateSize()
    {
        spriteRenderer.transform.localScale = Vector3.one + Vector3.one * 100 * (size / 65535f);
    }

    void OnCollectFood(ushort growSize)
    {
        size  += growSize;
        UpdateSize();
    }

    public static void OnSizeChanged(Changed<MovementHandler> changed)
    {
        changed.Behaviour.UpdateSize();
    }

}
