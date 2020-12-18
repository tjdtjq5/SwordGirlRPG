using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Function;
using BackEnd;
using LitJson;
using BackEnd.Tcp;

public class ChatManager : MonoBehaviour
{
    public Button chatBtn;
    public RectTransform scrollViewRectTransform; Vector2 openSizeDelta = new Vector2(938, 560); Vector2 closeSizeDelta = new Vector2(938, 50);
    public Transform content;
    public InputField inputField;
    public Button sendBtn;
    List<Text> texts = new List<Text>();

    List<SessionInfo> participants = new List<SessionInfo>();
    bool isOpen = false;

    private void Start()
    {
        Handler();

        for (int i = 0; i < content.childCount; i++)
        {
            texts.Add(content.GetChild(i).GetComponent<Text>());
        }
        chatBtn.onClick.AddListener(() => { ChatOpen(); });
        ChannelJoin();
        inputField.gameObject.SetActive(false);
        sendBtn.gameObject.SetActive(false);
    }
    public void ChatOpen()
    {
        isOpen = true;
        chatBtn.onClick.RemoveAllListeners();
        chatBtn.onClick.AddListener(() => { ChatClose(); });
        scrollViewRectTransform.DOSizeDelta(openSizeDelta, 0.2f).OnComplete(()=> {
            inputField.gameObject.SetActive(true);
            sendBtn.gameObject.SetActive(true);
        });
    }
    public void ChatClose()
    {
        isOpen = false;
        inputField.gameObject.SetActive(false);
        sendBtn.gameObject.SetActive(false);

        chatBtn.onClick.RemoveAllListeners();
        chatBtn.onClick.AddListener(() => { ChatOpen(); });
        scrollViewRectTransform.DOSizeDelta(closeSizeDelta, 0.2f);
    }

    public void OnClickSend()
    {
        if (inputField.text.Length == 0) return;

        ChatToChannel(inputField.text);
        AddMesseage(UserInfo.instance.nickName, inputField.text);

        inputField.text = "";
    }
    [ContextMenu("테스트")]
    void Test()
    {
        AddMesseage("테스트", "1231231231");
    }
    void AddMesseage(string nickname, string text)
    {
        string massage = nickname + " : " + text;

        int index = 0;
        bool isFull = true;
        for (int i = 0; i < texts.Count; i++) // 대화 텍스트창에 비어있는 곳 찾기 
        {
            if (texts[i].text == "")
            {
                index = i;
                isFull = false;
                break;
            }
        }

        if (isFull) // 모든 대화 텍스트창이 가득 찼다면 한칸씩 밀기
        {
            index = texts.Count - 1;
            for (int i = 0; i < texts.Count - 1; i++)
            {
                texts[i].text = texts[i + 1].text;
            }
        }

        texts[index].text = massage;
    }


    // 채널입장
    void ChannelJoin()
    {
        Backend.Chat.GetChannelList((callback) =>
        {
            if (callback.IsSuccess())
            {
                JsonData jsonData = callback.GetReturnValuetoJSON()["rows"];
                string hostName = "";
                int port = 0;
                string uuid = "";
                for (int i = 0; i < jsonData.Count; i++)
                {
                    JsonData channelJsonData = jsonData[i];
                    uuid = channelJsonData["uuid"].ToString();
                    hostName = channelJsonData["serverHostName"].ToString();
                    port = int.Parse(channelJsonData["serverPort"].ToString());
                }

                ChannelJoin(hostName, (ushort)port, uuid);
            }
            else
            {
                Debug.Log("일반채널 리스트 가져오기 실패 (에러코드)" + callback.GetErrorCode());
            }
        });
    }

    //채널에 입장하기
    void ChannelJoin(string hostName, int port, string uuid)
    {
        ErrorInfo errorInfo = new ErrorInfo();
        Backend.Chat.JoinChannel(ChannelType.Public, hostName, (ushort)port, uuid, out errorInfo);
    }

    //채널 퇴장하기
    void ChannelLeave()
    {
        Backend.Chat.LeaveChannel(ChannelType.Public);
    }

    //채팅 메세지 전송
    void ChatToChannel(string message)
    {
        Backend.Chat.ChatToChannel(ChannelType.Public, message);
    }

    // 공지 받기
    void OnNotification()
    {
        Backend.Chat.OnNotification = (NotificationEventArgs args) =>
        {
            string subject = args.Subject;
            string messge = args.Message;
        };
    }
    //운영자 공지 보내기
    void ChatToGlobal(string message)
    {
        Backend.Chat.ChatToGlobal(message);
    }

    //운영자 공지 받기
    void OnGlobalChat()
    {
        Backend.Chat.OnGlobalChat = (GlobalChatEventArgs args) =>
        {
            SessionInfo from = args.From; // 보낸 사람의 정보
            string message = args.Message; // 공지 메세지 정보
        };
    }

    // 유저신고
    void ReportUser(string reportedNickname, string reasons, string details)
    {
        Backend.Chat.ReportUser(reportedNickname, reasons, details, callback =>
        {
            // 이후 처리
            if (callback.GetMessage() == "Success")
            {
                string returnMessage = callback.GetReturnValue();
            }
        });
    }

    /// <summary>
    /// 이벤트 헨들러
    /// </summary>
    /// 

    void Handler()
    {
        OnSessionListInChannel();
        OnJoinChannel();
        OnLeaveChannel();
        OnSessionOfflineChannel();
        OnSessionOnlineChannel();
        OnChat();
        OnException();
    }

    // 채널에 입장 시 최초 한번, 해당 채널에 접속하고 있는 모든 게이머들의 정보 콜백
    void OnSessionListInChannel()
    {
        Backend.Chat.OnSessionListInChannel = (args) =>
        {
            participants.Clear();
            // 게이머 정보를 참여자 리스트에 추가
            foreach (SessionInfo session in args.SessionList)
            {
                participants.Add(session);
            }
            // 참여자 목록 출력
            for (int i = 0; i < participants.Count; i++)
            {
                Debug.Log("닉네임 : " + participants[i].NickName);
            }
        };
    }

    //자기자신 혹은 다른 게이머가 채널에 입장한 경우, 자기자신이 채널에 재접속 한 경우
    void OnJoinChannel()
    {
        Backend.Chat.OnJoinChannel = (args) =>
        {
            participants.Add(args.Session);
        };
    }

    //자기자신 혹은 다른 게이머가 채널에서 퇴장한 경우
    void OnLeaveChannel()
    {
        Backend.Chat.OnLeaveChannel = (args) =>
        {
            participants.Remove(args.Session);
        };
    }

    // 자기자신 혹은 다른게이머가 채팅 채널과 접속이 일시적으로 끊어진 경우
    void OnSessionOfflineChannel()
    {
        Backend.Chat.OnSessionOfflineChannel = (args) =>
        {
            participants.Remove(args.Session);
        };
    }
    // 다른게이머가 채팅 채널에 재접속 한 경우
    void OnSessionOnlineChannel()
    {
        Backend.Chat.OnSessionOnlineChannel = (args) =>
        {
            participants.Add(args.Session);
        };
    }
    //같은 채널의 게이머들이 전송한 메시지가 도착한 경우
    void OnChat()
    {
        Backend.Chat.OnChat = (args) =>
        {
            if (args.From.NickName != UserInfo.instance.nickName)
            {
                AddMesseage(args.From.NickName, args.Message);
            }
        };
    }
    // 채팅 관련 내부 기능에 예외가 발생한 경우
    void OnException()
    {
        Backend.Chat.OnException = (args) =>
        {

        };
    }

    private void Update()
    {
        Backend.Chat.Poll();
    }
}
