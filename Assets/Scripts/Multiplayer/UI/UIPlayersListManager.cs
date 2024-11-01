using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayersListManager : MonoBehaviour
{

    public GameObject playerMenuItemPrefab;
    public VerticalLayoutGroup verticalLayoutGroup;
    
    public void ClearList()
    {
        foreach (Transform child in verticalLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }

    }

    // Summary: Add new item to list
    public NetworkObject AddToList( string playerName)
    {   
        // Add new item to list
        NetworkObject playerMenuItem = NetworkManager.Instance._runner.Spawn(playerMenuItemPrefab, Vector3.zero, Quaternion.identity);
        playerMenuItem.transform.SetParent(verticalLayoutGroup.transform); // add to list
        playerMenuItem.GetComponent<UIPlayerMenuItemManager>().SetPlayerName(playerName);

        return playerMenuItem;

    }
}
