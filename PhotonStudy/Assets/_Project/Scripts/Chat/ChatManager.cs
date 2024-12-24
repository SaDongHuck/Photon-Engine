using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Chat.Demo;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChatAuthValues = Photon.Chat.AuthenticationValues;

//포톤 챗을 사용하기 위해서 1. IChatclientListener
public class ChatManager : MonoBehaviour, IChatClientListener
{
   public static ChatManager instance {  get; private set; }

    public JoinUI joinUI;
    public ChatUI chatUI;

    private ChatClient client;

    public ChatState state = 0;

    public string currentChannel;
   private void Awake()
   {
        instance = this;
   }

    //2.ChatClient를 생성
    private void Start()
    {
        client = new ChatClient(this);
        print(@"\n "" ");
    }

    //3.update에서 service를 ghcnf
    private void Update()
    {
        client.Service();
    }

    public void SetNickname(string nickname)
    {
        client.AuthValues = new ChatAuthValues(nickname);
    }

    public void ConnectUsingSettings()
    { //photonServerSettings를 사용하여 접속할 경우
        AppSettings appSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
        ChatAppSettings charSettings = appSettings.GetChatSettings();
        client.ConnectUsingSettings(charSettings);
    }

    
    public void ConnectUsingAppId()
    { //기본적으로 APPId를 통해 접속할 경우
        string chatId = "a092c204-78d2-4e3d-bf71-9ec8bde8f8b3";
        client.Connect(chatId, "1.0", client.AuthValues);
    }

    //특정 채팅방(채팅 채널)에서 채팅 시작
    public void ChatStart(string roomName)
    {
        client.Subscribe(new string[] { roomName });
    }

    //채팅 메세지 전송
    public void SendChatMessage(string message)
    {
        client.PublishMessage(currentChannel, message);
    }

    public void OnChatStateChange(ChatState state)
    {
        if (this.state != state)
        {
            print($"chat state changed : {state}");
            this.state = state;
        }
    }
    public void OnSubscribed(string[] channels, bool[] results)
    {
        currentChannel = channels[0];
        joinUI.gameObject.SetActive(false);
        chatUI.gameObject.SetActive(true);
        chatUI.roomNameLabel.text = channels[0];
        chatUI.ReceiveChatMessage("", $"<coloer=green>{currentChannel} 채팅방에 입장 하였습니다.</color>");
    }

    public void OnConnected()
    {
        joinUI.OnJoinedServer();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName != currentChannel)
        {
            print($"다른 채널의 메시지 수신함 : {channelName}");
            return;
        }

        for (int i = 0; i < senders.Length; i++)
        {
            chatUI.ReceiveChatMessage(senders[i], messages[i].ToString());
        }
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        
    }

    public void OnDisconnected()
    {
       
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        
    }

    public void OnUnsubscribed(string[] channels)
    {
       
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
       
    }

    public void OnUserSubscribed(string channel, string user)
    {
        
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        
    }
}
