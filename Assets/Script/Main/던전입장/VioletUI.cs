using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VioletUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject mainObj;
    public Text countText;
    public GameObject rule;
    public Scrollbar scrollbar_rule;
    int maxCount = 3;

    [Header("스크립트")]
    public DungeonManager dungeonManager;
    public MonsterUIManager monsterUIManager;

    public void Open()
    {
        mainObj.SetActive(true);
        UI_Setting();
    }
    public void Close()
    {
        mainObj.SetActive(false);
        RuleClose();
    }

    public void UI_Setting()
    {
        countText.text = UserInfo.instance.GetVioletRemainCount() + "/" + maxCount;
    }

    public void VioletPlay()
    {
        UserInfo.instance.PullVioletRemainCount();
        Debug.Log(UserInfo.instance.GetVioletRemainCount());
        UserInfo.instance.SaveUserViolet(() => {
            dungeonManager.VioletPlay();
            Close();
            monsterUIManager.Close();
        });
    }

    public void RuleOpen()
    {
        rule.gameObject.SetActive(true);
        scrollbar_rule.value = 0;
    }
    public void RuleClose()
    {
        rule.gameObject.SetActive(false);
    }
}
