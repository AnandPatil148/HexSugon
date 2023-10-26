using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    
    ///<summary>
    ///Player Controller Object Prefab
    ///</summary>
    public GameObject PC;
    public RoomManager roomManager;
    public Vector3 spawnPoint;

    private void Awake() 
    {
        roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            transform.position = spawnPoint;

            CreateController();

            //Invoke(nameof(TabListCreate),2f);

        }
    }


    void CreateController()
    { 
        PC = PhotonNetwork.Instantiate(PC.name, spawnPoint, Quaternion.identity);
        //PC.transform.SetParent(transform); 
    }

    
}
