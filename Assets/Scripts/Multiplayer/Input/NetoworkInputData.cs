using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public struct NetworkInputData: INetworkInput
{
    public Vector3 moveDirection;

    public float mouseXRotation;

    public NetworkBool isJumpPressed;
}
