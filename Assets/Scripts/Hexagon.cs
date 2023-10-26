using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Hexagon : MonoBehaviourPunCallbacks
{

    public Vector3 shrinkVector;

    // Start is called before the first frame update
    void Start()
    {
        /*
        if(!photonView.IsMine)
        {
            this.enabled = false;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {

        transform.localScale -= shrinkVector * Time.deltaTime;

        if( transform.localScale.x <= 0.05f) //0.62
            Destroy (gameObject );
    }


    public void OnCollisionEnter(Collision collision) 
    {
        if(collision.transform.CompareTag("MidBase") && photonView.ViewID == 0)
            Destroy(gameObject);
        
        else if(collision.transform.CompareTag("MidBase") && PhotonNetwork.IsMasterClient) //&& PhotonNetwork.IsMasterClient
            PhotonNetwork.Destroy(gameObject);        
            
        return;
    }   

}
