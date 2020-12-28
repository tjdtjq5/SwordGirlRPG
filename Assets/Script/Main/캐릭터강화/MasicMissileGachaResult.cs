using Function;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MasicMissileGachaResult : MonoBehaviour
{
    public GameObject blackPannel;
    public GameObject gachaPannel;

    public Transform card01Pannel;
    public Transform card06Pannel;

    public Button oneBtn;
    public Button sixBtn;
    public Button okBtn;

    public SpriteAtlas gradeSpriteAtlas;

    public MasicMissileManager masicMissileManager;

    public void Open01()
    {
        blackPannel.SetActive(true);
        gachaPannel.SetActive(true);
        card01Pannel.gameObject.SetActive(true);
        card06Pannel.gameObject.SetActive(false);
        BtnSetting();

        MasicMissileChartInfo masicMissileChartInfo = MasicMissileChart.instance.GetMasicMissileInfo(GetRandomMasicMissileName())[0];
        card01Pannel.GetChild(0).Find("마력검기").Find("등급이미지").GetComponent<Image>().sprite = gradeSpriteAtlas.GetSprite(masicMissileChartInfo.GradeType.ToString());
        card01Pannel.GetChild(0).Find("마력검기").Find("등급이미지").GetComponent<Image>().SetNativeSize();
        card01Pannel.GetChild(0).Find("마력검기").Find("Image").GetComponent<Image>().sprite = masicMissileChartInfo.Image;
        card01Pannel.GetChild(0).Find("이름").GetComponent<Text>().text = masicMissileChartInfo.Name;
        UserInfo.instance.PushMasicMissile(masicMissileChartInfo.Name);
        UserInfo.instance.SaveMasicMissile(()=> { });
        masicMissileManager.Init();
    }
    public void Open06()
    {
        blackPannel.SetActive(true);
        gachaPannel.SetActive(true);
        card01Pannel.gameObject.SetActive(false);
        card06Pannel.gameObject.SetActive(true);
        BtnSetting();

        for (int i = 0; i < 6; i++)
        {
            MasicMissileChartInfo masicMissileChartInfo = MasicMissileChart.instance.GetMasicMissileInfo(GetRandomMasicMissileName())[0];
            card06Pannel.GetChild(i).Find("마력검기").Find("등급이미지").GetComponent<Image>().sprite = gradeSpriteAtlas.GetSprite(masicMissileChartInfo.GradeType.ToString());
            card06Pannel.GetChild(i).Find("마력검기").Find("등급이미지").GetComponent<Image>().SetNativeSize();
            card06Pannel.GetChild(i).Find("마력검기").Find("Image").GetComponent<Image>().sprite = masicMissileChartInfo.Image;
            card06Pannel.GetChild(i).Find("이름").GetComponent<Text>().text = masicMissileChartInfo.Name;
            UserInfo.instance.PushMasicMissile(masicMissileChartInfo.Name);
        }
        UserInfo.instance.SaveMasicMissile(() => { });
        masicMissileManager.Init();
    }
    void BtnSetting()
    {
        int moneyNum = GachaChart.instance.gachaChartInfos.MasicMissile_Crystal_Num;

        oneBtn.transform.Find("MoneyNum").GetComponent<Text>().text = moneyNum.ToString();
        oneBtn.onClick.RemoveAllListeners();
        oneBtn.onClick.AddListener(() => {
            if (UserInfo.instance.crystal >= moneyNum) MoneyManager.instance.CrystalSub(moneyNum);
            UserInfo.instance.SaveMoney(() => { });
            Open01();
        });
        sixBtn.transform.Find("MoneyNum").GetComponent<Text>().text = moneyNum.ToString();
        sixBtn.onClick.RemoveAllListeners();
        sixBtn.onClick.AddListener(() => {
            if (UserInfo.instance.crystal >= moneyNum * 6) MoneyManager.instance.CrystalSub(moneyNum * 6);
            UserInfo.instance.SaveMoney(() => { });
            Open06();
        });
        okBtn.onClick.RemoveAllListeners();
        okBtn.onClick.AddListener(() => {
            blackPannel.SetActive(false);
            gachaPannel.SetActive(false);
        });
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
            List<string> missileList = MasicMissileChart.instance.MasicMissileType(gradeType);
            int rand = Random.Range(0, missileList.Count);
            return missileList[rand];
        }
        if (r < p02)
        {
            gradeType = GradeType.별2개;
            List<string> missileList = MasicMissileChart.instance.MasicMissileType(gradeType);
            int rand = Random.Range(0, missileList.Count);
            return missileList[rand];
        }
        if (r < p03)
        {
            gradeType = GradeType.별3개;
            List<string> missileList = MasicMissileChart.instance.MasicMissileType(gradeType);
            int rand = Random.Range(0, missileList.Count);
            return missileList[rand];
        }
        if (r < p04)
        {
            gradeType = GradeType.별4개;
            List<string> missileList = MasicMissileChart.instance.MasicMissileType(gradeType);
            int rand = Random.Range(0, missileList.Count);
            return missileList[rand];
        }
        if (r < p05)
        {
            gradeType = GradeType.별5개;
            List<string> missileList = MasicMissileChart.instance.MasicMissileType(gradeType);
            int rand = Random.Range(0, missileList.Count);
            return missileList[rand];
        }
        if (r < p06)
        {
            gradeType = GradeType.별6개;
            List<string> missileList = MasicMissileChart.instance.MasicMissileType(gradeType);
            int rand = Random.Range(0, missileList.Count);
            return missileList[rand];
        }
        return "";
    }

}
