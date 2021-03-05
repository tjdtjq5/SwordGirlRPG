using Function;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    // 규칙 ui
    public Text leftText; // 데미지 
    public Text rightText; // 마법석

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
        int remainCount = UserInfo.instance.GetVioletRemainCount();
        if(remainCount < 1) // 남은 도전권이 없을 경우 
        {
            OkAlram.instance.Open("남은 도전권이 없습니다\n내일 다시 도전 해주세요");
            return;
        }
 
        UserInfo.instance.PullVioletRemainCount();

        UserInfo.instance.SaveUserViolet(() => {
            dungeonManager.VioletPlay();
            Close();
            monsterUIManager.Close();
        });
    }

    public void RuleOpen()
    {
        rule.gameObject.SetActive(true);
        scrollbar_rule.value = 1;

        VioletRewardChartInfo[] violetRewardChartInfos = VioletRewardChart.instance.violetRewardChartInfo;
        string damageText = "";
        string masicStoneText = "";
        for (int i = 0; i < violetRewardChartInfos.Length; i++)
        {
            damageText += "데미지 " + MyMath.ValueToString(violetRewardChartInfos[i].TotalDamage) + "\n";
            masicStoneText += violetRewardChartInfos[i].MasicStoneCount + "\n";
        }
        leftText.text = damageText;
        rightText.text = masicStoneText;
    }
    public void RuleClose()
    {
        rule.gameObject.SetActive(false);
    }
}
