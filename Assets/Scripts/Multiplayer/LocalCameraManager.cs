using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCameraManager : MonoBehaviour
{

    public GameObject camPos;


    // Components
    Camera localCam;


    // Start is called before the first frame update
    void Start()
    {
        // Detach Cam if it is enabled
        if (gameObject.activeInHierarchy)
        {
            transform.parent = null;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(camPos == null)
            return;
            
        if (localCam == null)
            return;
            
        // Move Camera to player Pos
        transform.position = camPos.transform.localPosition;
    }
}
