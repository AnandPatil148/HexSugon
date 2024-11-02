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
    public UIPlayersListManager UIPlayersListManager;

    [Header("TMP Fields")]
    public TMP_InputField nickNameInputField;
    public TMP_InputField roomNameInputField;
    public TMP_Text sessionNameText;
    public TMP_Text nickNameText;

    public string LobbyName = "MainLobby";
    public string SessionName;
    public string NickName;

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
        menuManager.OpenMenu("LoadingMenu");
        
        NickName = nickNameInputField.text;
        nickNameText.text = NickName;

        PlayerPrefs.SetString("NickName", NickName);
        PlayerPrefs.Save();

        await NetworkManager.Instance.JoinLobby(LobbyName); // TODO: replace with real Lobby ID
        
    }

    // If Find Game button is clicked
    public void OnFindGameClick()
    {
        UISessionsManager.OnLoadingSessions();
        menuManager.OpenMenu("FindSessionsMenu");
    }

    public void OnCreateSession()
    {
        // gameObject.SetActive(false);
        NetworkManager.Instance.CreateSession( GameMode.Host, sessionName: roomNameInputField.text, lobbyName: LobbyName);

        menuManager.OpenMenu("LoadingMenu");
    }    

    public void OnJoinSession(SessionInfo sessionInfo)
    {
        NetworkManager.Instance.JoinSession(sessionInfo);
        menuManager.OpenMenu("LoadingMenu");
    }

    public void SetSessionInfo(string sessionName)
    {
        sessionNameText.text = sessionName;
        SessionName = sessionName;
    }

    public void OnStartGame()
    {
        NetworkManager.Instance.StartGame();
    }

    public void LoadSinglePlayerScene()
    {
        SceneManager.LoadScene(0);
    }
}
