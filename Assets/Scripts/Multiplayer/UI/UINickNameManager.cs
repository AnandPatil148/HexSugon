using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UINickNameManager : MonoBehaviour
{

    public Camera localCam;

    // Update is called once per frame
    void Update()
    {
        if(localCam == null)
            localCam = FindFirstObjectByType<Camera>();

        if(localCam == null)
            return;

        transform.LookAt(localCam.transform);
        transform.Rotate(Vector3.up * 180);


    }
}
