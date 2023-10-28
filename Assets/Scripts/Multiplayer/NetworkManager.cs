using System.Collections.Generic;
using System.Collections;
using Fusion.Sockets;
using UnityEngine;
using Fusion;
using System;
using UnityEngine.Diagnostics;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{

    public NetworkPlayer playerPrefab;


    // Start is called before the first frame update
    void Start()
    {
           
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    {


        //Spawns Player if we are the host
        if(runner.IsServer)
        {

            Debug.Log("OnPlayerJoined We are the server. Spawning Player");
            runner.Spawn(playerPrefab, Utils.GetRandomSpawnPoint(), Quaternion.identity, player);

        }


    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    public void OnConnectedToServer(NetworkRunner runner) { Debug.Log("OnConnectedToServer"); }


    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }


    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { Debug.Log("OnShutDown"); }

    public void OnDisconnectedFromServer(NetworkRunner runner) { Debug.Log("OnDIsConnectedFromServer");}

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { Debug.Log("OnConnectRequest"); }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { Debug.Log("OnConnectFailed");    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) {  }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)  { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) {  }

    public void OnSceneLoadStart(NetworkRunner runner)  {  }
}
