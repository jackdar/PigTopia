using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private SpriteRenderer playerSprite;

    private float moveSpeed = 4.0f;
    private bool isFacingLeft = true;
    private bool isFacingRight;
    private bool isWalking;

    private void Update()
    {
        HandleMovement();
        HandleFlip();
    }

    public bool IsWalking()
    {
        return isWalking;
    }
    
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNoramlized();
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0f);

        float moveDistance = moveSpeed * Time.deltaTime;

        transform.position += moveDir * moveDistance;
        isWalking = moveDir != Vector3.zero;
    }

    private void HandleFlip()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNoramlized();
        if (inputVector.x > 0 && isFacingLeft)
        {
            playerSprite.flipX = true;
            isFacingLeft = false;
            isFacingRight = true;
        }

        if (inputVector.x < 0 && isFacingRight)
        {
            playerSprite.flipX = false;
            isFacingLeft = true;
            isFacingRight = false;
        }
    }
}
