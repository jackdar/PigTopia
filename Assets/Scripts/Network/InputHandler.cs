using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    Vector2 mouseDirection = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        //Convert the mouse from screen space to world space
        Vector3 mouseWorldPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mouseWorldPoint.z = 0;

        //Calculate the direction of the mouse based on the players object.
        mouseDirection = mouseWorldPoint - transform.position;

        //Normalize if to ensure that we get a range from -1 to 1
        mouseDirection.Normalize();
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        networkInputData.movementInput = mouseDirection;

        return networkInputData;
    }
}
