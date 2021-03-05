using BackEnd;
using DG.Tweening;
using Function;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VioletResult : MonoBehaviour
{
    public GameObject resultObj;
    int maxCount = 3;

    [Header("UI")]
    public Text totalDamageText;
    public Text bestDamageText;
    public Text playCountText;
    public Text masicStoneText;
    public Text goldText;

    [Header("스크립트")]
    public DungeonManager dungeonManager;
    public PlayerController playerController;
    public Ranking ranking;

    [ContextMenu("TEST")]
    void TEST()
    {
        Open("100000050");
    }

    [System.Obsolete]
    public void Open(string totalDamage)
    {
        BackendGameInfo.instance.GetPrivateContents("Violet", "BestDamage", () => {


            string bestDamage = BackendGameInfo.instance.serverDataList[0];

            if (MyMath.CompareValue(totalDamage, bestDamage) == 1)
            {
                bestDamage = totalDamage;
                Param param = new Param();
                param.Add("BestDamage", bestDamage);
                BackendGameInfo.instance.PrivateTableUpdate("Violet", param, ()=> { ranking.VioletScoreRegister(totalDamage); });
            }
            else
            {
                ranking.VioletScoreRegister(totalDamage);
            }

            resultObj.SetActive(true);

            totalDamageText.text = "";
            totalDamageText.DOText(MyMath.ValueToString(totalDamage), 3, true, ScrambleMode.Numerals);
            bestDamageText.text = "";
            bestDamageText.DOText(MyMath.ValueToString(bestDamage), 3, true, ScrambleMode.Numerals);

            playCountText.text = UserInfo.instance.GetVioletRemainCount() + "/" + maxCount;

            masicStoneText.text = "";
            goldText.text = "";
            VioletRewardChartInfo violetRewardChartInfo = VioletRewardChart.instance.GetVioletReward(totalDamage);
            if (violetRewardChartInfo != null)
            {
                masicStoneText.DOText("X" + violetRewardChartInfo.MasicStoneCount, 3, true, ScrambleMode.Numerals);
                goldText.DOText("X" + violetRewardChartInfo.GoldCount, 3, true, ScrambleMode.Numerals);
            }
            else
            {
                masicStoneText.text = "X" + 0;
                goldText.text = "X" + 0;
            }

        }, () => {

            resultObj.SetActive(true);

            Param param = new Param();
            param.Add("BestDamage", totalDamage);
            BackendGameInfo.instance.PrivateTableUpdate("Violet", param, () => { ranking.VioletScoreRegister(totalDamage); });

            totalDamageText.text = "";
            totalDamageText.DOText(MyMath.ValueToString(totalDamage), 3, true, ScrambleMode.Numerals);
            bestDamageText.text = "";
            bestDamageText.DOText(MyMath.ValueToString(totalDamage), 3, true, ScrambleMode.Numerals);

            playCountText.text = UserInfo.instance.GetVioletRemainCount() + "/" + maxCount;

            masicStoneText.text = "";
            goldText.text = "";
            VioletRewardChartInfo violetRewardChartInfo = VioletRewardChart.instance.GetVioletReward(totalDamage);
            if (violetRewardChartInfo != null)
            {
                masicStoneText.DOText("X" + violetRewardChartInfo.MasicStoneCount, 3, true, ScrambleMode.Numerals);
                goldText.DOText("X" + violetRewardChartInfo.GoldCount, 3, true, ScrambleMode.Numerals);
            }
            else
            {
                masicStoneText.text = "X" + 0;
                goldText.text = "X" + 0;
            }
        });
    }
    // 나가기 
    public void Close()
    {
        resultObj.SetActive(false);

        dungeonManager.SetLobby(()=> {
            playerController.Hp_Initialized();
            playerController.Hp_UI_Setting();
            playerController.Play();
        });
    }
    public void ReStart()
    {
        int remainCount = UserInfo.instance.GetVioletRemainCount();
        if (remainCount < 1) // 남은 도전권이 없을 경우 
        {
            OkAlram.instance.Open("남은 도전권이 없습니다\n내일 다시 도전 해주세요");
            return;
        }

        resultObj.SetActive(false);
        dungeonManager.VioletPlay();
    }
}
