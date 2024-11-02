using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;
using System;
public class UISessionsManager : MonoBehaviour
{

    public TMP_Text statusText;

    public GameObject sessionItemPrefab;
    public VerticalLayoutGroup verticalLayoutGroup;


    public void ClearList()
    {
        foreach (Transform child in verticalLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }

        statusText.gameObject.SetActive(false);
    }

    public void AddToList(SessionInfo sessionInfo)
    {   
        // Add new item to list
        UISessionItemManager sessionItem = Instantiate(sessionItemPrefab, verticalLayoutGroup.transform).GetComponent<UISessionItemManager>();
        sessionItem.SetSessionInfo(sessionInfo);


        // Hook up Event
        sessionItem.OnSessionClicked += sessionItem_OnJoinSessionClicked;

    }

    // Event listners here
    private void sessionItem_OnJoinSessionClicked(SessionInfo info)
    {
        // Invoke the event the join session with the session info
        NetworkManager.Instance.JoinSession(info);

    }

    public void OnNoSessionsFound()
    {
        statusText.text = "No sessions found";
        statusText.gameObject.SetActive(true);
    }

    public void OnLoadingSessions()
    {
        statusText.text = "Loading Sessions...";
        statusText.gameObject.SetActive(true);
    }

    public void ClearStatusText()
    {
        statusText.text = "";
    }

  
}
