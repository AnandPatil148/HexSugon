using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerManager : NetworkBehaviour   
{
    public static PlayerManager instance;
    [SerializeField] private NetworkCharacterController _cc;
    public new GameObject camera;
    public float sensX;
    public float sensY;
    public int moveSpeed;
    private float xRotation;
    private float yRotation;

    // Start is called before the first frame update
    void Start()
    {

        if(!Object.HasInputAuthority)
        {
            camera.SetActive(false);
            return;
        }
        
        foreach(Transform trans in transform.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = LayerMask.NameToLayer("LocalPlayerModel");
        }

        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() 
    {
        if(!Object.HasInputAuthority)
            return;


        // rotate camera
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime; // get mouse vertical input and apply it as x axis rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // clamp vertical rotation
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0) ; // rotate camera on x axis

    }

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData data))
        {
            // rotate player
            yRotation = data.mouseXRotation * sensX * Runner.DeltaTime;
            transform.Rotate(0, yRotation, 0); // rotate player on y axis

            // move player
            Vector3 move = transform.right * data.moveDirection.x + transform.forward * data.moveDirection.z; // calculate move direction
            move.Normalize();

            _cc.Move(moveSpeed * Runner.DeltaTime * move); // move player

            // jump player
            if(data.isJumpPressed)
                _cc.Jump();

            CheckFallRespawn();

        }
    }

    public void Respawn()
    {
        transform.position = new Vector3(0, 2 , 0);
    }
    
    void CheckFallRespawn()
    {
        if(transform.position.y <= -10)
            Respawn();
    }

}
