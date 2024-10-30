using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Fusion;

public class UIManager : MonoBehaviour
{
    public MenuManager menuManager;

    public TMP_InputField nickNameInputField;
    public TMP_InputField roomNameInputField;
    public TMP_Text nickNameText;

    public static String NickName;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("NickName"))
        {
            NickName = PlayerPrefs.GetString("NickName");
            nickNameText.text = NickName;
            menuManager.OpenMenu("HomeMenu");
        }
        else
            menuManager.OpenMenu("NickNameMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNickName()
    {
        NickName = nickNameInputField.text;
        nickNameText.text = NickName;

        PlayerPrefs.SetString("NickName", NickName);
        PlayerPrefs.Save();
        
        menuManager.OpenMenu("MainMenu");
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
