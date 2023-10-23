using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

public class InputHandler : MonoBehaviour
{
    Vector2 moveDir = Vector2.zero;

    private Vector2 mousePos = Vector2.zero;

    [SerializeField] private GameObject shotPoint;
    private bool isFireButtonPressed = false;
    
    private PlayerInputActions playerInputActions;

    private Canvas canvas;

    private bool isKeyPressed = false;
    private float debounce = 0.2f;
    private float lastKeyPressTime;
    private float aimAngle = 0f;

    private MovementHandler movementHandler;
    // private Vector3 mouseAimPos;

    // public float offset;
    //
    // public Transform shotPoint;
    // public GameObject projectile;
    //
    // public float timeBetweenShots;
    // private float nextShotTime;
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
            // mouseAimPos = Input.mousePosition;
            
        }

        // Key debounce
        if (isKeyPressed && Time.time - lastKeyPressTime > debounce)
        {
            isKeyPressed = false;
        }
        
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }

    public void FixedUpdate()
    {
        Vector3 gunPos3D = shotPoint.transform.position;
        Vector2 gunPos2D = new Vector2(gunPos3D.x, gunPos3D.y);
        
        Vector2 aimDirection = mousePos - gunPos2D;
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        shotPoint.transform.eulerAngles = new Vector3(0f,0f,aimAngle);
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        if (!canvas.GetComponent<InGameUIHandler>().GetPauseMenuState())
        {
            networkInputData.movementInput = playerInputActions.Player.Move.ReadValue<Vector2>();
        }

        networkInputData.isFireButtonPressed = isFireButtonPressed;
        networkInputData.aimDirection = new Vector3(0f, 0f, aimAngle);
        
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
