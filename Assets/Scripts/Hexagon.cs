using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{

    public Vector3 shrinkVector;

    // Start is called before the first frame update
    void Start()
    {
        
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
        if(collision.transform.CompareTag("MidBase"))
            Destroy(gameObject);            
    }   

}
