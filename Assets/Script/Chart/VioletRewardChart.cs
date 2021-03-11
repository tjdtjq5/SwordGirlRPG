using BackEnd;
using Function;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VioletRewardChart : MonoBehaviour
{
    [SerializeField] string field;
    public static VioletRewardChart instance;
    private void Awake() { instance = this; }
    public VioletRewardChartInfo[] violetRewardChartInfo;

    public VioletRewardChartInfo GetVioletReward(string totalDamage)
    {
        /*
        if (MyMath.CompareValue(violetRewardChartInfo[0].TotalDamage, totalDamage) >= 1)
        {
            return null;
        }
        */

        for (int i = 0; i < violetRewardChartInfo.Length; i++)
        {
            if (MyMath.CompareValue(violetRewardChartInfo[i].TotalDamage, totalDamage) >= 1)
            {
                return violetRewardChartInfo[i];
            }
        }

        return violetRewardChartInfo[violetRewardChartInfo.Length - 1];
    }

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            violetRewardChartInfo = new VioletRewardChartInfo[jsonData.Count];

            for (int i = 0; i < jsonData.Count; i++)
            {
                violetRewardChartInfo[i] = new VioletRewardChartInfo();
                JsonData rowData = jsonData[i];
                violetRewardChartInfo[i].TotalDamage = rowData["TotalDamage"]["S"].ToString();
                if (i > 0) violetRewardChartInfo[i].BeforeDamage = jsonData[i - 1]["TotalDamage"]["S"].ToString();
                else violetRewardChartInfo[i].BeforeDamage = "0";
                violetRewardChartInfo[i].Level = i + 1;
                violetRewardChartInfo[i].MasicStoneCount = int.Parse(rowData["MasicStoneCount"]["S"].ToString());
                violetRewardChartInfo[i].GoldCount = rowData["GoldCount"]["S"].ToString();
            }

            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class VioletRewardChartInfo
{
    public string TotalDamage;
    public string BeforeDamage;
    public int Level;
    public int MasicStoneCount;
    public string GoldCount;
}