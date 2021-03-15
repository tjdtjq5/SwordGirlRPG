using BackEnd;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PassChart : MonoBehaviour
{
    [SerializeField] string field;
    public static PassChart instance;
    private void Awake() { instance = this; }
    public PassChartInfo[] passChartInfos;

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            passChartInfos = new PassChartInfo[jsonData.Count];
            for (int i = 0; i < jsonData.Count; i++)
            {
                passChartInfos[i] = new PassChartInfo();
                JsonData rowData = jsonData[i];
                passChartInfos[i].Point = int.Parse(rowData["Point"]["S"].ToString());
                passChartInfos[i].NomalReward = (MoneyType)int.Parse(rowData["NomalReward"]["S"].ToString());
                passChartInfos[i].NomalRewardCount = int.Parse(rowData["NomalRewardCount"]["S"].ToString());
                passChartInfos[i].PassReward = (MoneyType)int.Parse(rowData["PassReward"]["S"].ToString());
                passChartInfos[i].PassRewardCount = int.Parse(rowData["PassRewardCount"]["S"].ToString());
            }
            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class PassChartInfo
{
    public int Point;
    public MoneyType NomalReward;
    public int NomalRewardCount;
    public MoneyType PassReward;
    public int PassRewardCount;
}