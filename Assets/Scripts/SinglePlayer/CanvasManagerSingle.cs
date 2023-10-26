using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasManagerSingle : MonoBehaviour
{

    [Header("References")]
    public PlayerMovement PM;

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject tabPanel;

    [Header("Score")]
    public TMP_Text scoreCount;

    [Header("Settings")]
    public AudioMixer audioMixer;
    
    // Update is called once per frame
    void Update()
    {
        scoreCount.text = PM.score.ToString();
    }

    public void ExitLevel()
    {
        SceneManager.LoadScene(0);
    }

#region SettingsMenu
    
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume",volume);
    }
    
#endregion
}
