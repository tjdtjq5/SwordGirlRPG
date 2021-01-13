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

    [ContextMenu("세이브")]
    public void Test1()
    {
        int point = 5;
        Dictionary<int, bool> com = new Dictionary<int, bool>();
        com.Add(0, false);
        com.Add(1, true);
        com.Add(2, false);
        BackendReturnObject servertime = Backend.Utils.GetServerTime();
        string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
        DateTime currentTime = DateTime.Parse(time);

        UserInfo.instance.userWeekByQuest = new UserWeekByQuest(point, com, currentTime);

        UserInfo.instance.SaveUserWeekByQuest(() => { });
    }
    [ContextMenu("로드")]
    public void Test2()
    {
        UserInfo.instance.LoadUserWeekByQuest(() => {
            int point = UserInfo.instance.GetUserWeekByQuestPoint();
            Debug.Log("포인트  :  " + point);
            Debug.Log("com0  :  " + UserInfo.instance.GetUserWeekByQuestComplete(0));
            Debug.Log("com1  :  " + UserInfo.instance.GetUserWeekByQuestComplete(1));
            Debug.Log("com2  :  " + UserInfo.instance.GetUserWeekByQuestComplete(2));
            Debug.Log("Time  :  " + UserInfo.instance.userWeekByQuest.time);
        });
    }

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
                    weekQuestChartInfos[i].RewardList.Add(int.Parse(rewardData[i]));
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
    public List<int> RewardList = new List<int>();
    public List<int> RewardCountList = new List<int>();
}