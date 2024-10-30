using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{

    Vector3 moveInput = Vector3.zero;
    Vector3 viewInput = Vector3.zero;

    bool jumpPressed;

    public float sensX;
    public float sensY;
    private float xRotation;
    private float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // View Input
        viewInput.x = Input.GetAxis("Mouse X") * sensX;
        viewInput.y = Input.GetAxis("Mouse Y") * sensY; 
        // Move Input store keyboard horizontal and vertical movement
        moveInput.x = Input.GetAxis("Horizontal"); // store horizontal movement 1 or -1
        moveInput.z = Input.GetAxis("Vertical"); // store vertical movement 1 or -1

        // Jump Pressed
        if(Input.GetButtonDown("Jump"))
            jumpPressed = true;
    }

    public NetworkInputData GetNetworkInputData()
    {
        NetworkInputData networkInputData = new();

        // Set Mouse X Input
        networkInputData.mouseXRotation = viewInput.x;
        
        // Set Move Input
        networkInputData.moveDirection = moveInput;

        // Set Jump Input
        networkInputData.isJumpPressed = jumpPressed;
        jumpPressed = false;

        return networkInputData;

    }

    public Vector3 GetViewInput()
    {
        return viewInput;

    }
}
