using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayByQuestChart : MonoBehaviour
{
    [SerializeField] string field;
    public static DayByQuestChart instance;
    private void Awake() { instance = this; }
    public DayByQuestChartInfo[] dayByQuestChartInfos;

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            dayByQuestChartInfos = new DayByQuestChartInfo[jsonData.Count];

            for (int i = 0; i < jsonData.Count; i++)
            {
                dayByQuestChartInfos[i] = new DayByQuestChartInfo();
                JsonData rowData = jsonData[i];
                dayByQuestChartInfos[i].Title = rowData["Title"]["S"].ToString();
                dayByQuestChartInfos[i].Content = rowData["Content"]["S"].ToString();
                dayByQuestChartInfos[i].Count = int.Parse(rowData["Count"]["S"].ToString());
                dayByQuestChartInfos[i].Point = int.Parse(rowData["Point"]["S"].ToString());
                dayByQuestChartInfos[i].Reward01 = (MoneyType)int.Parse(rowData["Reward01"]["S"].ToString());
                dayByQuestChartInfos[i].Reward01Count = int.Parse(rowData["Reward01Count"]["S"].ToString());
                dayByQuestChartInfos[i].Reward02 = (MoneyType)int.Parse(rowData["Reward02"]["S"].ToString());
                dayByQuestChartInfos[i].Reward02Count = int.Parse(rowData["Reward02Count"]["S"].ToString());
                dayByQuestChartInfos[i].MainReward = (MoneyType)int.Parse(rowData["MainReward"]["S"].ToString());
                dayByQuestChartInfos[i].MainRewardCount = int.Parse(rowData["MainRewardCount"]["S"].ToString());
            }

            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class DayByQuestChartInfo
{
    public string Title;
    public string Content;
    public int Count;
    public int Point;
    public MoneyType Reward01;
    public int Reward01Count;
    public MoneyType Reward02;
    public int Reward02Count;
    public MoneyType MainReward;
    public int MainRewardCount;
}