using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Difficulty
{
	Easy = 0,
	Normal,
	Hard,
}

public class RoomPanel : MonoBehaviour
{
	public Text roomTitleText;

	public Difficulty roomDifficulty;

	public Dropdown difficultyDropdown;

	public Text difficultyText;

	public RectTransform playerList;
	public GameObject playerTextPrefab;

	public Dictionary<int, PlayerEntry> playerListDic = new Dictionary<int, PlayerEntry>();


	public Button startButton;
	public Button cancelButton;

	private Dictionary<int, bool> playersReady;

	private void Awake()
	{
		startButton.onClick.AddListener(startButtonClick);
		cancelButton.onClick.AddListener(CancelButtonClick);
		difficultyDropdown.ClearOptions();
		foreach(object difficulty in Enum.GetValues(typeof(Difficulty)))
		{
			Dropdown.OptionData option = new Dropdown.OptionData(difficulty.ToString());
			difficultyDropdown.options.Add(option);
		}
		difficultyDropdown.onValueChanged.AddListener(DiffcultyValueChange);
	}

    private void OnEnable()
    {
		foreach (Transform child in playerList)
		{ //플레이어 리스트에 다른 객체가 있으면 모두 삭제
			Destroy(child.gameObject);
		}
		if (false == PhotonNetwork.InRoom) return;

		roomTitleText.text = PhotonNetwork.CurrentRoom.Name;

		foreach(Player player in PhotonNetwork.CurrentRoom.Players.Values)
		{ //플레이어 정보 객체 생성
			JoinPlayer(player);
		}
        //방장인지 여부를 확인하여 활성화/비활성화
        difficultyDropdown.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
		PhotonNetwork.AutomaticallySyncScene = true;

    }

    

    public void JoinPlayer(Player newPlayer)
	{
		PlayerEntry playerEntry = Instantiate(playerTextPrefab, playerList, false).
			GetComponent<PlayerEntry>();

		playerEntry.playerNameText.text = newPlayer.NickName;
		playerEntry.player = newPlayer;

		if(PhotonNetwork.LocalPlayer.ActorNumber != newPlayer.ActorNumber)
		{
			playerEntry.readyToggle.gameObject.SetActive(false);
		}
	}

	public void LeavePlayer(Player gonePlayer)
	{
		foreach(Transform child in playerList)
		{
			Player player = child.GetComponent<PlayerEntry>().player;
			if(player.ActorNumber == gonePlayer.ActorNumber)
			{
				Destroy(child.gameObject);
			}
		}
	}

    private void CancelButtonClick()
    {
		PhotonNetwork.LeaveRoom();
    }

    private void startButtonClick()
    {
		PhotonNetwork.LoadLevel("GameScene");
    }

    private void DiffcultyValueChange(int arg0)
    {

    }
    
}

