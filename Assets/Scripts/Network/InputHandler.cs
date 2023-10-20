using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    Vector2 moveDir = Vector2.zero;

    private PlayerInputActions playerInputActions;

    private Canvas canvas;

    private bool isKeyPressed = false;
    private float debounce = 0.2f;
    private float lastKeyPressTime;

    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // Pause menu button
        if (playerInputActions.Player.Pause.IsPressed() && !isKeyPressed)
        {
            HandlePauseMenu();
            isKeyPressed = true;
            lastKeyPressTime = Time.time;
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

        if (canvas != null && !canvas.GetComponent<InGameUIHandler>().GetPauseMenuState())
        {
            networkInputData.movementInput = playerInputActions.Player.Move.ReadValue<Vector2>();
        }
        

        return networkInputData;
    }

    public void HandlePauseMenu()
    {
        canvas.GetComponent<InGameUIHandler>().OnPauseGame();
    }
}
