using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothGachaResult : MonoBehaviour
{
    public GameObject resultPannel;
    public GameObject blackPannel;
    public Transform open01Pannel;
    public Transform open06Pannel;

    public ClothManager clothManager;

    public void Open01()
    {
        int userCrystal = UserInfo.instance.crystal;
        int needCrystal = 100;
        if (userCrystal < needCrystal)
        {
            Debug.Log("크리스탈 부족");
            return;
        }
        MoneyManager.instance.CrystalSub(needCrystal);

        resultPannel.SetActive(true);
        blackPannel.SetActive(true);
        open01Pannel.gameObject.SetActive(true);
        open06Pannel.gameObject.SetActive(false);

        string name = GetRandomMasicClothName();
        UserInfo.instance.PushCloth(name);
        Transform card = open01Pannel.GetChild(0);
        card.Find("이름").GetComponent<Text>().text = name;
        card.Find("Icon").GetComponent<Image>().sprite = ClothChart.instance.GetClothChartInfo(name)[0].Icon;

        UserInfo.instance.SaveMoney(() => { UserInfo.instance.SaveCloth(() => { }); });

        clothManager.UISetting();

    }
    public void Open06()
    {
        int userCrystal = UserInfo.instance.crystal;
        int needCrystal = 100 * 6;
        if (userCrystal < needCrystal)
        {
            Debug.Log("크리스탈 부족");
            return;
        }
        MoneyManager.instance.CrystalSub(needCrystal);

        resultPannel.SetActive(true);
        blackPannel.SetActive(true);
        open01Pannel.gameObject.SetActive(false);
        open06Pannel.gameObject.SetActive(true);

        for (int i = 0; i < 6; i++)
        {
            string name = GetRandomMasicClothName();
            UserInfo.instance.PushCloth(name);
            Transform card = open06Pannel.GetChild(i);
            card.Find("이름").GetComponent<Text>().text = name;
            card.Find("Icon").GetComponent<Image>().sprite = ClothChart.instance.GetClothChartInfo(name)[0].Icon;
        }
        UserInfo.instance.SaveMoney(() => { UserInfo.instance.SaveCloth(() => { }); });

        clothManager.UISetting();
    }

    public void Close()
    {
        resultPannel.SetActive(false);
        blackPannel.SetActive(false);
    }
    string GetRandomMasicClothName()
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
            List<string> nameList = ClothChart.instance.ClothType(gradeType);
            int rand = Random.Range(0, nameList.Count);
            return nameList[rand];
        }
        if (r < p02)
        {
            gradeType = GradeType.별2개;
            List<string> nameList = ClothChart.instance.ClothType(gradeType);
            int rand = Random.Range(0, nameList.Count);
            return nameList[rand];
        }
        if (r < p03)
        {
            gradeType = GradeType.별3개;
            List<string> nameList = ClothChart.instance.ClothType(gradeType);
            int rand = Random.Range(0, nameList.Count);
            return nameList[rand];
        }
        if (r < p04)
        {
            gradeType = GradeType.별4개;
            List<string> nameList = ClothChart.instance.ClothType(gradeType);
            int rand = Random.Range(0, nameList.Count);
            return nameList[rand];
        }
        if (r < p05)
        {
            gradeType = GradeType.별5개;
            List<string> nameList = ClothChart.instance.ClothType(gradeType);
            int rand = Random.Range(0, nameList.Count);
            return nameList[rand];
        }
        if (r < p06)
        {
            gradeType = GradeType.별6개;
            List<string> nameList = ClothChart.instance.ClothType(gradeType);
            int rand = Random.Range(0, nameList.Count);
            return nameList[rand];
        }
        return "";
    }


}
