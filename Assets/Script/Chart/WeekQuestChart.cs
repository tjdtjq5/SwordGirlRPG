using BackEnd;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeekQuestChart : MonoBehaviour
{
    [SerializeField] string field;
    public static WeekQuestChart instance;
    private void Awake() { instance = this; }
    public WeekQuestChartInfo[] weekQuestChartInfos;

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            weekQuestChartInfos = new WeekQuestChartInfo[jsonData.Count];

            for (int i = 0; i < jsonData.Count; i++)
            {
                weekQuestChartInfos[i] = new WeekQuestChartInfo();
                JsonData rowData = jsonData[i];
                weekQuestChartInfos[i].Point = int.Parse(rowData["Point"]["S"].ToString());

                string[] rewardData = rowData["RewardList"]["S"].ToString().Split('-');
                weekQuestChartInfos[i].RewardList.Clear();
                for (int j = 0; j < rewardData.Length; j++)
                {
                    weekQuestChartInfos[i].RewardList.Add((MoneyType)int.Parse(rewardData[i]));
                }

                string[] rewardCountData = rowData["RewardCountList"]["S"].ToString().Split('-');
                weekQuestChartInfos[i].RewardCountList.Clear();
                for (int j = 0; j < rewardCountData.Length; j++)
                {
                    weekQuestChartInfos[i].RewardCountList.Add(int.Parse(rewardCountData[i]));
                }
            }

            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class WeekQuestChartInfo
{
    public int Point;
    public List<MoneyType> RewardList = new List<MoneyType>();
    public List<int> RewardCountList = new List<int>();
}