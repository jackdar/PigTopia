using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TMP_Text score;
    
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
        Vector2 movement = inputVector * moveSpeed;

        rb.velocity = new Vector2(movement.x, movement.y);
        isWalking = movement != Vector2.zero;
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

    public void updateScore()
    {
        score.text = (score.text + 1).ToString();
    }
}
