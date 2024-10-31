using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISessionItemManager : MonoBehaviour
{
    public TMP_Text roomNameText;
    public TMP_Text playerCountText;
    public event Action<SessionInfo> OnSessionClicked;

    public bool canPlayersJoin = true;
    SessionInfo sessionInfo;


    public void SetSessionInfo(SessionInfo sessionInfo) 
    {
        this.sessionInfo = sessionInfo;
        roomNameText.text = sessionInfo.Name;
        playerCountText.text = $"Players: {sessionInfo.PlayerCount}/{sessionInfo.MaxPlayers}";
    

        if(sessionInfo.PlayerCount >= sessionInfo.MaxPlayers)
            canPlayersJoin = false;

        GetComponent<Button>().enabled = canPlayersJoin;

    }


    public void JoinSession()
    {
        // Invoke the event the join session with the session info
        OnSessionClicked?.Invoke(sessionInfo);        
    }



}
