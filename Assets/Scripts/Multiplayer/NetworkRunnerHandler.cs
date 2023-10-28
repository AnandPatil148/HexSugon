using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using Fusion.Sockets;
using System.Linq;
using UnityEngine;
using Fusion;
using System;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using NanoSockets;


public class NetworkRunnerHandler : MonoBehaviour
{

    public NetworkRunner networkRunnerPrefab;

    NetworkRunner networkRunner;



    // Start is called before the first frame update
    void Start()
    {
        //Initializes a network runner
        networkRunner = Instantiate(networkRunnerPrefab,Vector3.zero, Quaternion.identity);
        networkRunner.name = "NetworkRunner";

        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);

        Debug.Log($"Server Network Runner Started");           
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {

        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if(sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        runner.ProvideInput = true;

        return runner.StartGame( new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = "TestRoom",
            Initialized = initialized,
            SceneManager = sceneManager,

        });


    }

}
