using Fusion;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    Vector2 moveDir = Vector2.zero;

    private PlayerInputActions playerInputActions;

    private GameObject canvas;
    private InGameUIHandler inGameUIHandler;

    private bool isKeyPressed = false;
    private float debounce = 0.2f;
    private float lastKeyPressTime;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        inGameUIHandler = canvas.GetComponent<InGameUIHandler>();
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
            if (gameObject == NetworkPlayer.Local.gameObject)
            {
                HandlePauseMenu();
            }
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
        NetworkInputData networkInputData = new();

<<<<<<< Updated upstream
        if (!canvas.GetComponent<InGameUIHandler>().GetPauseMenuState())
=======
        if (inGameUIHandler != null)
>>>>>>> Stashed changes
        {
            if (!inGameUIHandler.GetPauseMenuState())
            {
                networkInputData.movementInput = playerInputActions.Player.Move.ReadValue<Vector2>();
            }
        }
        return networkInputData;
    }

    public void HandlePauseMenu()
    {
        canvas.GetComponent<InGameUIHandler>().OnPauseGame();
    }
}
