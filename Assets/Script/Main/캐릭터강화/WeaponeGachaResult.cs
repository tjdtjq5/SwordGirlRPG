using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class WeaponeGachaResult : MonoBehaviour
{
    public GameObject blackPannel;
    public GameObject gachaPannel;

    [Header("버튼들")]
    public Text one_M_Num;
    public Text eight_M_Num;
    [Header("카드패널")]
    public GameObject oneCardPannel;
    public GameObject eightCardPannel;
    [Header("아틀라스")]
    public SpriteAtlas gradeSpriteAtlas;
    [Header("스크립트")]
    public WeaponeManager weaponeManager;

    private void Start()
    {
        BtnUISetting();
    }
    public void Open01()
    {
        int money = GachaChart.instance.gachaChartInfos.Weapone_Crystal_Num;
        if (UserInfo.instance.crystal < money) // 돈이 모자랄 경우
        {
            return;
        }
        MoneyManager.instance.CrystalSub(money);

        // 무기 얻기 
        for (int i = 0; i < oneCardPannel.transform.childCount; i++)
        {
            string itemName = GetRandomMasicMissileName();
            WeaponeChartInfo weaponeChartInfo = WeaponeChart.instance.GetWeaponeChartInfo(itemName)[0];
            oneCardPannel.transform.GetChild(i).Find("Icon").GetComponent<Image>().sprite = weaponeChartInfo.Image;
            oneCardPannel.transform.GetChild(i).Find("GradeType").GetComponent<Image>().sprite = gradeSpriteAtlas.GetSprite(weaponeChartInfo.GradeType.ToString());
            oneCardPannel.transform.GetChild(i).Find("GradeType").GetComponent<Image>().SetNativeSize();
            UserInfo.instance.PushWeapone(itemName);
        }

        // 저장 
        UserInfo.instance.SaveMoney(() => {
            UserInfo.instance.SaveWeapone(() => { });
        });


        blackPannel.SetActive(true);
        gachaPannel.SetActive(true);

        oneCardPannel.SetActive(true);
        eightCardPannel.SetActive(false);

        weaponeManager.Open();
    }
    public void Open08()
    {
        int money = GachaChart.instance.gachaChartInfos.Weapone_Crystal_Num * 8;
        if (UserInfo.instance.crystal < money) // 돈이 모자랄 경우
        {
            return;
        }
        MoneyManager.instance.CrystalSub(money);

        // 무기 얻기 
        for (int i = 0; i < eightCardPannel.transform.childCount; i++)
        {
            string itemName = GetRandomMasicMissileName();
            WeaponeChartInfo weaponeChartInfo = WeaponeChart.instance.GetWeaponeChartInfo(itemName)[0];
            eightCardPannel.transform.GetChild(i).Find("Icon").GetComponent<Image>().sprite = weaponeChartInfo.Image;
            eightCardPannel.transform.GetChild(i).Find("GradeType").GetComponent<Image>().sprite = gradeSpriteAtlas.GetSprite(weaponeChartInfo.GradeType.ToString());
            eightCardPannel.transform.GetChild(i).Find("GradeType").GetComponent<Image>().SetNativeSize();
            UserInfo.instance.PushWeapone(itemName);
        }

        // 저장 
        UserInfo.instance.SaveMoney(() => {
            UserInfo.instance.SaveWeapone(() => { });
        });

        blackPannel.SetActive(true);
        gachaPannel.SetActive(true);

        oneCardPannel.SetActive(false);
        eightCardPannel.SetActive(true);

        weaponeManager.Open();
    }

    void BtnUISetting()
    {
        int money = GachaChart.instance.gachaChartInfos.Weapone_Crystal_Num;
        one_M_Num.text = money.ToString();
        eight_M_Num.text = (money * 8).ToString();
    }

    public void Close()
    {
        blackPannel.SetActive(false);
        gachaPannel.SetActive(false);
    }

    string GetRandomMasicMissileName()
    {
        float r = Random.Range(0, 100);
        GradeType gradeType = GradeType.별1개;
        float p01 = GachaChart.instance.gachaChartInfos.GradeType01_Percent;
        float p02 = p01 + GachaChart.instance.gachaChartInfos.GradeType02_Percent;
        float p03 = p02 + GachaChart.instance.gachaChartInfos.GradeType03_Percent;
        float p04 = p03 + GachaChart.instance.gachaChartInfos.GradeType04_Percent;
        float p05 = p04 + GachaChart.instance.gachaChartInfos.GradeType05_Percent;
        float p06 = p05 + GachaChart.instance.gachaChartInfos.GradeType06_Percent;
        if (r < p01)
        {
            gradeType = GradeType.별1개;
            List<string> missileList = WeaponeChart.instance.WeaponeType(gradeType);
            int rand = Random.Range(0, missileList.Count);
            return missileList[rand];
        }
        if (r < p02)
        {
            gradeType = GradeType.별2개;
            List<string> missileList = WeaponeChart.instance.WeaponeType(gradeType);
            int rand = Random.Range(0, missileList.Count);
            return missileList[rand];
        }
        if (r < p03)
        {
            gradeType = GradeType.별3개;
            List<string> missileList = WeaponeChart.instance.WeaponeType(gradeType);
            int rand = Random.Range(0, missileList.Count);
            return missileList[rand];
        }
        if (r < p04)
        {
            gradeType = GradeType.별4개;
            List<string> missileList = WeaponeChart.instance.WeaponeType(gradeType);
            int rand = Random.Range(0, missileList.Count);
            return missileList[rand];
        }
        if (r < p05)
        {
            gradeType = GradeType.별5개;
            List<string> missileList = WeaponeChart.instance.WeaponeType(gradeType);
            int rand = Random.Range(0, missileList.Count);
            return missileList[rand];
        }
        if (r < p06)
        {
            gradeType = GradeType.별6개;
            List<string> missileList = WeaponeChart.instance.WeaponeType(gradeType);
            int rand = Random.Range(0, missileList.Count);
            return missileList[rand];
        }
        return "";
    }
}
