using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
	public Text roomNameLabel;
	public InputField messageInput;
	public Button sendButton;
	public RectTransform messageContent;
	public GameObject messageEntryPrefab;

	public string myNickname = "무명의 전사";

	private void Awake()
	{ 
		messageInput.onEndEdit.AddListener(x=>SendChatMessage());
		sendButton.onClick.AddListener(SendChatMessage);
	}
	
	//메세지 보낼때 호출
	public void SendChatMessage()
	{

        string message = messageInput.text;
        if (string.IsNullOrEmpty(message)) return;

		if(message.ContainsFword())
		{
			ReceiveChatMessage("", "<color=red>비속어가 포함되어 있습니다.</color>");
		}
		else
		{
            //ChatManager에게 메세지 보내기 호출
            ChatManager.instance.SendChatMessage(message);

            messageInput.text = "";
            messageInput.ActivateInputField();
        }
        
	}

	//메세지 받을때 호출
	public void ReceiveChatMessage(string nickname, string message)
	{
		var entry = Instantiate(messageEntryPrefab, messageContent);
		entry.transform.Find("Nickname").GetComponent<Text>().text = nickname;
		entry.transform.Find("Message").GetComponent<Text>().text = message;
	}


}

