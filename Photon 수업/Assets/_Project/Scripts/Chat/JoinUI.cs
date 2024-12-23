using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinUI : MonoBehaviour
{
	public InputField nicknameInput;
	public InputField roomnameInput;
	public Button nicknameChangeButton;

	public Button connectButton;

	public Button joinRoomButton;
	public Text logText;
    private void Awake()
    {
        nicknameInput.onValueChanged.AddListener(NicknameInputEdit);
		nicknameChangeButton.onClick.AddListener(nicknameChangeButtonclick);
        connectButton.onClick.AddListener(ConnectButtonclick);
        joinRoomButton.onClick.AddListener(JoinRoomButtonClick);
    }

    //닉네임 입력란에 입력이 될 때마다 문자열 검증
    private void NicknameInputEdit(string input)
    {
        nicknameInput.SetTextWithoutNotify(input);
        logText.text = "";
    }

    // 유요한 닉네임인지 검증 할거야
    //미ㅣㅡㅡㅜ
    private void nicknameChangeButtonclick()
    {
        string nickname = nicknameInput.text;

        if (nickname.NicKnameValidate())
        {
            ChatManager.instance.SetNickname(nickname);
        }
        else
        {
            logText.text = "닉네임이 규칙에서 벗어납니다.";
        }

    }

    private void ConnectButtonclick()
    {
        ChatManager.instance.ConnectUsingSettings();
        connectButton.interactable = false;
    }
    private void JoinRoomButtonClick()
    {
        ChatManager.instance.ChatStart(roomnameInput.text);
        roomnameInput.interactable = false;
        joinRoomButton.interactable= false;
    }

    public void OnJoinedServer()
    {
        connectButton.GetComponentInChildren<Text>().text = "채팅 서버 접속 됨";
    }
    

    
}

