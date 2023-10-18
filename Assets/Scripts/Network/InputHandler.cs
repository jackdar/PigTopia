using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    Vector2 moveDir = Vector2.zero; //initialize vector for player movement input

    private PlayerInputActions playerInputActions; //input actions defined by player

    void Awake()
    {
        playerInputActions = new PlayerInputActions(); //initialize the player's input actions
        playerInputActions.Player.Enable(); //enable the player's input actions
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.W)) moveDir.y = +1f;
        //if (Input.GetKey(KeyCode.S)) moveDir.y = -1f;
        //if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        //if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;
        //
        //moveDir.Normalize();
    }

    //get player's network input data
    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        //read and store movement input from the input system
        networkInputData.movementInput = playerInputActions.Player.Move.ReadValue<Vector2>();

        return networkInputData;
    }
}
