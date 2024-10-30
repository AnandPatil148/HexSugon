using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Fusion.Sockets;
using UnityEngine;
using Fusion;
using System;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{

    public NetworkRunner _runner;
    public NetworkPrefabRef _prefabRef;
    public Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new();

    public PlayerInputManager playerInputManager;

    public MenuManager MenuManager;


    // Start is called before the first frame update
    private void Start() 
    {

        // Create the Fusion runner and let it know that we will provide user input
        _runner = gameObject.GetComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        
    }


    async void StartGame(GameMode mode)
    {   

        // Create The NetworkSceneInfo from current scene
        var sceneInfo = new NetworkSceneInfo();
        var sceneRef = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        if (sceneRef.IsValid)
            sceneInfo.AddSceneRef(sceneRef, LoadSceneMode.Additive);


        await _runner.StartGame(new StartGameArgs
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = sceneInfo,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });

        MenuManager.gameObject.SetActive(false);// close the menu temporarily
    }

    public void StartHost()
    {
        StartGame(GameMode.Host);
    }    

    public void StartClient()
    {
        StartGame(GameMode.Client);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(runner.IsServer)
        {
            // MenuManager.OpenMenu("RoomMenu");

            // Spawn Players Prefab
            Vector3 spawnPosition = Vector3.up;
            NetworkObject networkObject = runner.Spawn(_prefabRef, spawnPosition, Quaternion.identity, player);

            _spawnedCharacters.Add(player, networkObject); // store so we can destroy it later

        }
        Debug.Log("Player joined: " + player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if(_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            // Despawn Player Prefab
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if(playerInputManager == null && PlayerManager.instance != null)
            playerInputManager = PlayerManager.instance.GetComponent<PlayerInputManager>();

        if(playerInputManager != null)
            input.Set(playerInputManager.GetNetworkInputData());
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log("OnInputMissing");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("NetworkRunner Shutdown!");
    }

    public void OnConnectedToServer( NetworkRunner runner )
    {
        Debug.Log("Connected to server!");

    }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log("Disconnected from server!");
    }
    
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }    
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject networkObject, PlayerRef playerRef) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject networkObject, PlayerRef playerRef) { }

}
