using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{
    
    [Header("References")]
    public RoomManager roomManager;
    public Rigidbody rb;
    public PhotonView PV;
    public PlayerCam playerCam;

    [Header("Canvas")]
    public CanvasManagerMulti canvasManager;
    public GameObject mainCanvas;



    [Header("Movement")]
    public Transform orientation;
    public float moveSpeed;
    Vector3 moveDirection;

    
    [Header("Input")]
    public float horizontalInput;
    public float verticalInput;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode tabKey = KeyCode.Tab;

    [Header("GroundCheck")]
    public LayerMask whatIsGround;
    public float playerHeight;
    public float groundDrag;
    public bool grounded;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    public bool readToJump;

    [Header("Score")]
    public int score;



    // Start is called before the first frame update
    void Start()
    {

        if(!PV.IsMine)
        {
            
            //Destroy(rb);
            rb.isKinematic = true;
            playerCam.gameObject.SetActive(false);
            mainCanvas.SetActive(false);
        }

        roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        rb.freezeRotation = true;

    }

    // Update is called once per frame
    void Update()
    {

        if(!PV.IsMine) return;


        //Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        //Handle Drag
        if(grounded)
        {
            rb.drag = groundDrag;
            //readToJump = true;
        } 
        else
        {
            rb.drag = 0;
        }

        // Y value Check
        if(transform.position.y <= -10)
        {
            transform.position = new Vector3(0, 2 , 0);
        } 


        MyInput();
        SpeedControl();
        PV.RPC(nameof(UpdateScore), RpcTarget.MasterClient,PV.ViewID);
    }

    void FixedUpdate() 
    {

        if(!PV.IsMine) return;

        MovePlayer();
    }


    //Gets Input
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Check for Jumping
        if(Input.GetKey(jumpKey) && readToJump && grounded)
        {
            readToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCoolDown);
        }
        if(Input.GetKey(tabKey))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canvasManager.tabPanel.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canvasManager.tabPanel.SetActive(false);

        }
    }

    //Function to move player
    private void MovePlayer()
    {
        //Calculate Movement direction where we are looking at
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //On Ground
        if(grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        //On Air
        else if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);
        }

    }

    //Limits Speed
    private void SpeedControl()
    {
        //Velocity along x and z
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f , rb.velocity.z); 

        if( flatVel.magnitude > moveSpeed)
        {
            Vector3 limitVel = flatVel.normalized * moveSpeed;

            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        } 
    }

    private void Jump()
    {
        //Reset Y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f , rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readToJump = true;
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if(collision.transform.CompareTag("Hexagon"))
        {
            
            if(score <= 0)
                score = 0;
            else
                score--;

            transform.position = new Vector3(0, 2 , 0);
        } 
    }

    private void OnTriggerEnter(Collider collider) 
    {
        if(collider.transform.CompareTag("OpenHexagon"))
        {
            score++;
        }

    }

[PunRPC]
    public void UpdateScore(int viewID)
    {
        PhotonView playerToUpdate = PhotonView.Find(viewID);

        playerToUpdate.GetComponent<PlayerController>().score = score;
        //score++;

    }
    

}

