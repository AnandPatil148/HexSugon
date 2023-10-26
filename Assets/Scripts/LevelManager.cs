using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

public class LevelManager : MonoBehaviour
{

    public void LoadSingle()
    {
        SceneManager.LoadScene(0);
        PhotonNetwork.Disconnect();
    }

    public void LoadMulti()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
