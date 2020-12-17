using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Function;

public class ChatManager : MonoBehaviour
{
    public Button chatBtn;
    public RectTransform scrollViewRectTransform; Vector2 openSizeDelta = new Vector2(938, 560); Vector2 closeSizeDelta = new Vector2(938, 50);

    [ContextMenu("테스트")]
    void Test()
    {
        Debug.Log(MyMath.Fomula03(8,100,0.1f));
    }
    private void Start()
    {
        chatBtn.onClick.AddListener(() => { ChatOpen(); });
    }
    public void ChatOpen()
    {
        chatBtn.onClick.RemoveAllListeners();
        chatBtn.onClick.AddListener(() => { ChatClose(); });
        scrollViewRectTransform.DOSizeDelta(openSizeDelta, 0.2f);
    }
    public void ChatClose()
    {
        chatBtn.onClick.RemoveAllListeners();
        chatBtn.onClick.AddListener(() => { ChatOpen(); });
        scrollViewRectTransform.DOSizeDelta(closeSizeDelta, 0.2f);
    }
}
