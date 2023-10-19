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

    // Other components
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody2D_;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigidbody2D_ = GetComponent<Rigidbody2D>();
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
        if(GetInput(out NetworkInputData networkInputData))
        {
            inputDirection = networkInputData.movementInput;
        }

        // Server moves the network objects
        if (Object.HasStateAuthority)
        {
            Vector2 movementDirection = inputDirection;

            movementDirection.Normalize();

            float movementSpeed = (size / Mathf.Pow(size, 1.1f)) * 2;

            rigidbody2D_.velocity = movementDirection * movementSpeed;
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

    void UpdateSize()
    {
        spriteRenderer.transform.localScale = Vector3.one + Vector3.one * 100 * (size / 65535f);
    }

    public static void OnSizeChanged(Changed<MovementHandler> changed)
    {
        changed.Behaviour.UpdateSize();
    }

}
