using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject hexagonPrefab;

    public Vector3 spawnPoint;

    public float spawnrate;
    public float nextTimeToSpawn; 


    // Update is called once per frame
    void Update()
    {
        
        if(Time.time >= nextTimeToSpawn)
        {
            Instantiate(hexagonPrefab, spawnPoint, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            nextTimeToSpawn = Time.time + spawnrate;    
        }


    }
}