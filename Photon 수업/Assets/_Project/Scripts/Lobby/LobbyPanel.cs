using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
	public RectTransform roomListRect;
	public GameObject roomButtonPrefab;
	public Button cancelButton;

    private List<RoomInfo> currentRoomList = new List<RoomInfo>();

	private void Awake()
	{
		cancelButton.onClick.AddListener(CancelButtonclick);
	}

    private void OnEnable()
    {
        foreach (RoomInfo roomInfo in currentRoomList)
        {
            AddRoomButton(roomInfo);
        }
    }

    private void OnDisable()
    {
        foreach(Transform child in roomListRect)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateRommList(List<RoomInfo> roomList)
    {
        //현재 RoomList에 있는데, OnRoomListUpdate 파라미터로 넘어온 RoomList에는 없는
        //방 참여 버튼을 삭제 해야 함.
        List<RoomInfo> destrotyCanditate = new List<RoomInfo>(); //파괴 후보
        destrotyCanditate = currentRoomList.FindAll(x =>false == roomList.Contains(x));

        //currentRoomList에는 없는데 roomList에는 있는 방참여 버튼 생성하기
        foreach (RoomInfo roomInfo in roomList)
        {
            if (currentRoomList.Contains(roomInfo)) continue;
            AddRoomButton(roomInfo);
        }

        //destroyCanditate 리스트에 있는 방 참여 버튼 삭제/
        foreach (Transform child in roomListRect)
        {
            if(destrotyCanditate.Exists(x=>x.Name == child.name))
            {
                Destroy(child.gameObject);
            }
        }
        currentRoomList = roomList;
    }

    //방 데이터가 있으면, 방 참여 버튼을 생성할 메서드
    public void AddRoomButton(RoomInfo roomInfo)
    {
       GameObject joinButton = Instantiate(roomButtonPrefab, roomListRect, false);
       joinButton.name = roomInfo.Name;
       joinButton.GetComponent<Button>().onClick.AddListener(() => PhotonNetwork.JoinRoom(roomInfo.Name));
       joinButton.GetComponentInChildren<Text>().text = roomInfo.Name;

    }

    private void CancelButtonclick()
    {
        PhotonNetwork.LeaveLobby();
    }
}

