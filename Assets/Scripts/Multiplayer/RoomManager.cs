using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using ExitGames.Client.Photon.StructWrapping;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    [Header("Spawner")]
    public GameObject hexagonPrefab;
    public Vector3 spawnPoint;
    public float spawnRate;
    public float nextTimeToSpawn;

    
    [Header("Game Logic")]
    public GameObject playerManager;

    public GameObject localPM;
    
    public bool gameStarted;
    public bool joinedRoom;

    public PlayerController winner;
    public GameObject[] MPlayers;

    private void Awake() 
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;   
    }

    private void Start() 
    {
        joinedRoom = true;    
    }

     public override void OnEnable() 
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        switch (scene.buildIndex)
        {
            case 1:

                //calls The player listing method and sets name properly
                //Launcher.Instance.SetPlayerDetails(LoginPagePlayfab.Instance.Name);
                Launcher.Instance.OnJoinedRoom();
                gameStarted = false;
                winner = null;
                break;

            case 3:
                //calls game loader method which starts game
                GameLoader();
                break;
        }
    }

    private void Update() 
    {
        MPlayers = GameObject.FindGameObjectsWithTag("MPlayer");

        if(gameStarted == true && PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().buildIndex == 3)
        {
            Spawner();
        }
        if(nextTimeToSpawn > 30 && PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().buildIndex == 3)
        {
            CompleteLevel();
        }
    }

    public void GameLoader()
    {

        gameStarted = true;

        localPM = PhotonNetwork.Instantiate(playerManager.name, Vector3.zero, Quaternion.identity);
    }

    public void Spawner()
    {

        if(Time.time >= nextTimeToSpawn)
        {
            PhotonNetwork.InstantiateRoomObject(hexagonPrefab.name, spawnPoint, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            nextTimeToSpawn = Time.time + spawnRate;    
        }
    }

    public void CompleteLevel()
    {

        gameStarted = false;
        winner = MPlayers[0].GetComponent<PlayerController>();

        foreach(GameObject Mplayer in MPlayers)
        {
            PlayerController PC = Mplayer.GetComponent<PlayerController>();
            if ( winner.score < PC.score)
                winner = PC;

            PC.playerCam.enabled = false;
            PC.jumpForce = 0;
            PC.moveSpeed = 0;
            PC.canvasManager.SetWinner(winner.photonView.Owner.NickName);

            
        }
    }

}
