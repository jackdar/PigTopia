using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerButtons
{
    Fire = 0,
}
public struct NetworkInputData : INetworkInput
{
    public Vector2 movementInput;
    public Vector2 aimForwardVector;
    public NetworkBool isFireButtonPressed;
    public NetworkButtons Buttons;
    public Vector3 aimDirection;
}
