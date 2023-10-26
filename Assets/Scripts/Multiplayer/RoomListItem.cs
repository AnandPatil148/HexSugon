using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{

    [SerializeField] TMP_Text roomName;

    public RoomInfo roomInfo;

    public void SetUp(RoomInfo _info)
    {
        roomInfo = _info;

        roomName.text = _info.Name;
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(roomInfo);
    }

}
