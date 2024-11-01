using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Fusion;
using System;
using NUnit.Framework;

public class UIPlayerMenuItemManager : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnNickNameChanged))]
    public string NickName { get; set; }

    public TMP_Text playerName;

    void Start() 
    {
        if(HasInputAuthority)
            RPC_SetNickNameP(PlayerPrefs.GetString("NickName"));


    }

    public void SetPlayerName(string name)
    {
        playerName.text = name;
    }


    public void OnNickNameChanged()
    {
    }

[Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SetNickNameP(string nickName, RpcInfo info = default)
    {
        NickName = nickName;
        gameObject.name = NickName + "MenuItem";
        playerName.text = NickName;

    }


}
