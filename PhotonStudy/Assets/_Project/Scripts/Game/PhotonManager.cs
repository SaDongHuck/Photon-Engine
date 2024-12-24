using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

//포톤 콜백 리스너
public class PhotonManager : MonoBehaviourPunCallbacks
{
    public bool isTestmode = false;
    private void Start()
    {
        isTestmode = PhotonNetwork.IsConnected == false;

        if (isTestmode)
        {
            PhotonNetwork.ConnectUsingSettings();
        } else
        {
            GameManager.isGameReady = true;
        }
    }

    public override void OnConnectedToMaster()
    {
        if(isTestmode)
        {
            RoomOptions option = new()
            {
                IsVisible = false,
                MaxPlayers = 8,
            };
            PhotonNetwork.JoinOrCreateRoom("TestRoom", option, TypedLobby.Default);
        }
    }

    public override void OnJoinedRoom()
    {
        
        if(isTestmode)
        {
            GameObject.Find("Canvas/DebugText").GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name;
            GameManager.isGameReady = true;
        }
    }
}
