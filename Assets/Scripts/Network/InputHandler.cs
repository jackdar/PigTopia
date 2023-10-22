using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    Vector2 moveDir = Vector2.zero;
    private bool isFireButtonPressed = false;
    
    private PlayerInputActions playerInputActions;

    private Canvas canvas;

    private bool isKeyPressed = false;
    private float debounce = 0.2f;
    private float lastKeyPressTime;

    private MovementHandler movementHandler;
    
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        movementHandler = GetComponent<MovementHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!movementHandler.Object.HasInputAuthority)
        {
            return;
        }
        // Pause menu button
        if (playerInputActions.Player.Pause.IsPressed() && !isKeyPressed)
        {
            HandlePauseMenu();
            isKeyPressed = true;
            lastKeyPressTime = Time.time;
        }

        if (playerInputActions.Player.Fire.IsPressed())
        {
            HandleFire();
        }

        // Key debounce
        if (isKeyPressed && Time.time - lastKeyPressTime > debounce)
        {
            isKeyPressed = false;
        }
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        if (!canvas.GetComponent<InGameUIHandler>().GetPauseMenuState())
        {
            networkInputData.movementInput = playerInputActions.Player.Move.ReadValue<Vector2>();
        }

        networkInputData.isFireButtonPressed = isFireButtonPressed;
        
        // Resetting the state after reading the variable
        isFireButtonPressed = false;

        return networkInputData;
    }

    public void HandlePauseMenu()
    {
        canvas.GetComponent<InGameUIHandler>().OnPauseGame();
    }

    private void HandleFire()
    {
        isFireButtonPressed = true;
        Debug.Log("Fire button pressed!");
    }
}
