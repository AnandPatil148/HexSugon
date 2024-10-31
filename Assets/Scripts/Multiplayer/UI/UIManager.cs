using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Fusion;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    public MenuManager menuManager;

    public UISessionsManager UISessionsManager;

    public TMP_InputField nickNameInputField;
    public TMP_InputField roomNameInputField;
    public TMP_Text nickNameText;

    public static String NickName;
    public static String LobbyName = "MainLobby";

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;


        if(PlayerPrefs.HasKey("NickName"))
        {
            NickName = PlayerPrefs.GetString("NickName");
            nickNameInputField.text = NickName;
            nickNameText.text = NickName;
            menuManager.OpenMenu("NickNameMenu");
        }
        else
            menuManager.OpenMenu("NickNameMenu");

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Join Fusion Lobby
    public async void JoinFusionLobby()
    {
        NickName = nickNameInputField.text;
        nickNameText.text = NickName;

        PlayerPrefs.SetString("NickName", NickName);
        PlayerPrefs.Save();

        await NetworkManager.Instance.JoinLobby(LobbyName); // TODO: replace with real Lobby ID
        
        menuManager.OpenMenu("HomeMenu");
    }

    public void StartHost()
    {
        //NetworkManager.instance.StartGame(GameMode.Host, roomNameInputField.text);
        NetworkManager.instance.StartGame(GameMode.Host, "TestRoom"); // TODO: replace with real room name
    }    

    public void StartClient()
    {
        NetworkManager.instance.StartGame(GameMode.Client, roomNameInputField.text);
    }

    public void LoadSinglePlayerScene()
    {
        SceneManager.LoadScene(0);
    }
}
