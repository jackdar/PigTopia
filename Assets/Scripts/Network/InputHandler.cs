using Fusion;
<<<<<<< HEAD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;
=======
using UnityEngine;
>>>>>>> multiplayer

public class InputHandler : MonoBehaviour
{
    Vector2 moveDir = Vector2.zero;

<<<<<<< HEAD
    private Vector2 mousePos = Vector2.zero;

    [SerializeField] private GameObject shotPoint;
    private bool isFireButtonPressed = false;
    
=======
>>>>>>> multiplayer
    private PlayerInputActions playerInputActions;

    private GameObject canvas;
    private InGameUIHandler inGameUIHandler;

    private bool isKeyPressed = false;
    private float debounce = 0.2f;
    private float lastKeyPressTime;
<<<<<<< HEAD
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
=======

>>>>>>> multiplayer
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        inGameUIHandler = canvas.GetComponent<InGameUIHandler>();
    }

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
<<<<<<< HEAD
        movementHandler = GetComponent<MovementHandler>();
=======
>>>>>>> multiplayer
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        if (!movementHandler.Object.HasInputAuthority)
        {
            return;
        }
=======
>>>>>>> multiplayer
        // Pause menu button
        if (playerInputActions.Player.Pause.IsPressed() && !isKeyPressed)
        {
            if (gameObject == NetworkPlayer.Local.gameObject)
            {
                HandlePauseMenu();
            }
            isKeyPressed = true;
            lastKeyPressTime = Time.time;
        }

<<<<<<< HEAD
        if (playerInputActions.Player.Fire.IsPressed())
        {
            HandleFire();
            // mouseAimPos = Input.mousePosition;
            
        }

=======
>>>>>>> multiplayer
        // Key debounce
        if (isKeyPressed && Time.time - lastKeyPressTime > debounce)
        {
            isKeyPressed = false;
        }
<<<<<<< HEAD
        
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }

    public void FixedUpdate()
    {
        Vector3 gunPos3D = shotPoint.transform.position;
        Vector2 gunPos2D = new Vector2(gunPos3D.x, gunPos3D.y);
        
        Vector2 aimDirection = mousePos - gunPos2D;
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
=======
>>>>>>> multiplayer
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new();

        if (inGameUIHandler != null)
        {
            if (!inGameUIHandler.GetPauseMenuState())
            {
                networkInputData.movementInput = playerInputActions.Player.Move.ReadValue<Vector2>();
            }
        }
<<<<<<< HEAD

        networkInputData.isFireButtonPressed = isFireButtonPressed;
        networkInputData.aimDirection = new Vector3(0f, 0f, aimAngle);
        
        // Resetting the state after reading the variable
        isFireButtonPressed = false;

=======
>>>>>>> multiplayer
        return networkInputData;
    }

    public void HandlePauseMenu()
    {
        canvas.GetComponent<InGameUIHandler>().OnPauseGame();
    }
<<<<<<< HEAD

    private void HandleFire()
    {
        isFireButtonPressed = true;
        Debug.Log("Fire button pressed!");
    }
=======
>>>>>>> multiplayer
}
