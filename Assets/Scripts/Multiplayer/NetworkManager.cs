using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Fusion.Sockets;
using UnityEngine;
using Fusion;
using System;
using System.Threading.Tasks;
/*
TODO: 
    - Add Player Disconnecting 
    - Add Player Reconnecting
    - Host migration
    - Server mode
    - Pickups and stuff as booster or collectables
    - UI
    - Players able to push each other
    - Sessions to track and add respective button. Fusion lobby system
    - Add Join room button with proper setting as rooms get created. Fusion lobby feature
    - Add Player Listing in lobby menu with their nicknames
*/

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{

    public static NetworkManager Instance;

    public NetworkRunner _runner;
    public NetworkPrefabRef _prefabRef;
    public Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new();

    public PlayerInputManager playerInputManager;

    // Start is called before the first frame update
    private void Start() 
    {

        // Create the Fusion runner and let it know that we will provide user input
        _runner = gameObject.GetComponent<NetworkRunner>();
        _runner.ProvideInput = true;


        if(Instance == null)
            Instance = this;
        
    }

    public async Task JoinLobby(string lobbyName)
    {
        // Join Lobby
        await _runner.JoinSessionLobby(SessionLobby.Custom, lobbyName);
    }


    public async Task InitializeRunner(GameMode gameMode, string sessionName, string lobbyName = "")
    {   

        // Create The NetworkSceneInfo from current scene
        SceneRef sceneRef = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex); 
        
        // Start the game session with the scene or Join Session 
        await _runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            SessionName = sessionName,
            CustomLobbyName = lobbyName, // "MainLobby"
            Scene = sceneRef,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });

        // _runner.LoadScene(0);

    }
    public async void CreateSession(GameMode mode, string sessionName, string lobbyName = "")
    {
        //NetworkManager.instance.StartGame(GameMode.Host, roomNameInputField.text);
        // await NetworkManager.instance.InitializeRunner(GameMode.Host, sessionName:"TestRoom", sceneName:"MLevel1"); // TODO: replace with real room name
        // gameObject.SetActive(false);
        await InitializeRunner(GameMode.Host, sessionName, lobbyName); // TODO: replace with real room name
    }    

    public async void JoinSession(SessionInfo sessionInfo)
    {

        await InitializeRunner(GameMode.Client, sessionInfo.Name);
    }

    public void StartGame()
    {
        if(_runner.IsServer)
            _runner.LoadScene("MLevel1");
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
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("OnSessionListUpdated");

        if(UIManager.Instance == null)
        {
            Debug.Log("UIManager is null");
            return;
        }
        UISessionsManager uiSessionsManager = UIManager.Instance.UISessionsManager;

        if( sessionList.Count == 0 )
        {
            Debug.Log("No sessions found");
            uiSessionsManager.ClearList();
            
            // Show that no sessions were found in status text
            uiSessionsManager.OnNoSessionsFound(); 
        }
        else
        {
            Debug.Log("Sessions found");
            uiSessionsManager.ClearList();
            uiSessionsManager.ClearStatusText();

            foreach (SessionInfo sessionInfo in sessionList )
            {
                uiSessionsManager.AddToList(sessionInfo);
            }
        }



    }    

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        // Debug.Log("OnInputMissing");
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
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject networkObject, PlayerRef playerRef) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject networkObject, PlayerRef playerRef) { }

}
