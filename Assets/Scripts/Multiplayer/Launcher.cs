using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{

   public static Launcher Instance;
   public MenuManager menuManager;
   public GameObject roomManager;

   public Player[] players;

   [Header("Profile")]
   public TMP_Text nickName;

   [Header("Room Listing")]
   public TMP_InputField roomNameInputField;
   public Transform roomListContent;
   public GameObject roomListItemPrefab;

   [Header("Player In Room Listing")]
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameObject;

    [Header("Error")]
    public TMP_Text errorText;

   private void Awake() 
   {
        Instance = this;
   } 

    //Connect To Master Server
   private void Start() 
   {
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); 
        }
        else if(!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

   }

    //Join Lobby
   public override void OnConnectedToMaster()
   {
        if(PhotonNetwork.InLobby)
        {
            return;
        }

        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
   }

   public override void OnJoinedLobby()
   {
        menuManager.OpenMenu("HomeMenu");
        PhotonNetwork.NickName = "Sugon" + Random.Range(1,69);
        nickName.text = PhotonNetwork.NickName;
   }

   //Get Player name
    public void SetPlayerDetails(string playerName)
    {
        PhotonNetwork.NickName = playerName;
        //playerProfileName.text = playerName;
    }

    //Create Room
    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        menuManager.OpenMenu("LoadingMenu");
    }
    
    //Join Room
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        menuManager.OpenMenu("LoadingMenu");
    }

#region Room Listing

    //INS Room Menu
    public override void OnJoinedRoom()
    {
        //INS RoomManager
        if(!GameObject.Find("RoomManager") && PhotonNetwork.IsMasterClient)
        {
            //PhotonNetwork.InstantiateRoomObject(roomManager.name, Vector3.zero, Quaternion.identity);
            //Instantiate(roomManager, Vector3.zero, Quaternion.identity);
        }

        //PhotonNetwork.InstantiateRoomObject(roomManager.name, Vector3.zero, Quaternion.identity);
        Instantiate(roomManager, Vector3.zero, Quaternion.identity);

        menuManager.OpenMenu("RoomMenu");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        startGameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

     //when master client leaves Asign start game option to new master client
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    //New player joins and instantiates his player prefab
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    //start game option
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(3);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    // room creation fails
    public override void OnCreateRoomFailed(short returnCode, string message)
	{
		errorText.text = "Room Creation Failed: " + message;
		//Debug.LogError("Room Creation Failed: " + message);
		menuManager.OpenMenu("ErrorMenu");
	}

    //Leaves Room
    public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
		menuManager.OpenMenu("LoadingMenu");
	}

    //After leaving room
    public override void OnLeftRoom()
	{
        //joinedRoom = false;
        Destroy(GameObject.FindGameObjectWithTag("RoomManager"));
		menuManager.OpenMenu("HomeMenu");
	}

    

#endregion

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for(int i = 0; i < roomList.Count; i++)
        {
            if(roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    //Disconnect from Master Server
	public void DisconnectFromMasterAndLogOut()
	{
        //LoginPagePlayfab.Instance.Logout();        
	    PhotonNetwork.Disconnect();
        SceneManager.LoadScene("HomeMenu");
	}
}
