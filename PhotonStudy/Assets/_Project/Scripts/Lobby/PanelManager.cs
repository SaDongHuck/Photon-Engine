using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PanelManager : MonoBehaviourPunCallbacks {
	public static PanelManager Instance;

	public LoginPanel login;
	public MenuPanel menu;
	public LobbyPanel lobby;
	public RoomPanel room;

	
	Dictionary<string, GameObject> panelDic;

	private void Awake() {
		Instance = this;
		panelDic = new Dictionary<string, GameObject>()
		{
			{ "Login", login.gameObject},
			{ "Menu", menu.gameObject},
			{ "Lobby", lobby.gameObject},
			{ "Room", room.gameObject }
		};

		PanelOpen("Login");
	}

	public void PanelOpen(string panelName) {
		foreach (var row in panelDic) {
			row.Value.SetActive(row.Key == panelName);
		}
	}

    public override void OnEnable()
    {
		base.OnEnable();//MonoBehaviourPunCallbacks를 상속한 클래스는 OnEnamble을 재정의 할때
		//부모의 꼭 OnEnable을 호출해야 함
        //print("하이");
    }

    public override void OnConnected()
    { //포톤 서버에 접속 되었을 때 호출
		PanelOpen("Menu");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
		LogManager.Log($"로그아웃 됨 {cause}");
		PanelOpen("Login");
    }

    public override void OnCreatedRoom()
    { //방을 생성하였을때 호출
		PanelOpen("Room");
    }

    public override void OnJoinedRoom()
    { //방에 참여
		PanelOpen("Room");
        Hashtable rommCustomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        if (rommCustomProperties.ContainsKey("Difficulty"))
        {
            room.OnDifficultyChange((Difficulty)rommCustomProperties["Difficulty"]);
        }
    }



    public override void OnLeftRoom()
    { // 방에 떠났을때 호출
		PanelOpen("Menu");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        room.JoinPlayer(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        room.LeavePlayer(otherPlayer);
    }
    public override void OnJoinedLobby()
    {
		PanelOpen("Lobby");
    }

    public override void OnLeftLobby()
    {
		PanelOpen("Menu");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        lobby.UpdateRommList(roomList);
    }

    public override void OnRoomPropertiesUpdate(Hashtable p)
    {
        if(p.ContainsKey("Difficulty"))
        {
            room.OnDifficultyChange((Difficulty)p["Difficulty"]);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("CharacterSelect"))
        {
            room.OnCharcterSelectChange(targetPlayer, changedProps);
        }
    }
}
	

	

