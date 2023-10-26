using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using Photon.Pun;
using ExitGames.Client.Photon;

public class CanvasManagerMulti : MonoBehaviour
{
   [Header("References")]
    public PlayerController PC;

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject tabPanel;
    public GameObject levelCompletePanel;


    [Header("Score")]
    public TMP_Text scoreCount;

    [Header("Winner")]
    public TMP_Text winnerName;

    [Header("Settings")]
    public AudioMixer audioMixer;
    
    // Update is called once per frame
    void Update()
    {
        scoreCount.text = PC.score.ToString();
    }

    public void SetWinner(string winner)
    {
        mainPanel.SetActive(false);
        levelCompletePanel.SetActive(true);
        winnerName.text = winner;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

#region SettingsMenu
    
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume",volume);
    }
    
#endregion
}
