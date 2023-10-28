using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    
    public static Vector3 GetRandomSpawnPoint()
    {
        return new Vector3( Random.Range(-2.4f, 2.4f), 2, Random.Range(-2.4f, 2.4f)  );
    }


}
