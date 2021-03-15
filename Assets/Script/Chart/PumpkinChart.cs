using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinChart : MonoBehaviour
{
    [SerializeField] string field;
    public static PumpkinChart instance;
    private void Awake() { instance = this; }
    public PumpkinChartInfo[] pumpkinChartInfos;

    public PumpkinChartInfo GetPumpkinInfo(int level)
    {
        if (level < 1 || pumpkinChartInfos.Length < level) return null;
        return pumpkinChartInfos[level - 1];
    }

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            pumpkinChartInfos = new PumpkinChartInfo[jsonData.Count];

            for (int i = 0; i < jsonData.Count; i++)
            {
                pumpkinChartInfos[i] = new PumpkinChartInfo();
                JsonData rowData = jsonData[i];
                pumpkinChartInfos[i].Level = int.Parse(rowData["Level"]["S"].ToString());
                pumpkinChartInfos[i].Hp = rowData["Hp"]["S"].ToString();

                string[] jsonFirstRewardTypeList = rowData["FirstRewardTypeList"]["S"].ToString().Split('-');
                ItemType[] FirstRewardTypeList = new ItemType[jsonFirstRewardTypeList.Length];
                for (int j = 0; j < FirstRewardTypeList.Length; j++)
                {
                    FirstRewardTypeList[j] = (ItemType)int.Parse(jsonFirstRewardTypeList[j]);
                }

                string[] FirstRewardNameList = rowData["FirstRewardNameList"]["S"].ToString().Split('-');

                string[] jsonFirstRewardCountList = rowData["FirstRewardCountList"]["S"].ToString().Split('-');
                int[] FirstRewardCountList = new int[jsonFirstRewardCountList.Length];
                for (int j = 0; j < FirstRewardCountList.Length; j++)
                {
                    FirstRewardCountList[j] = int.Parse(jsonFirstRewardCountList[j]);
                }

                pumpkinChartInfos[i].FirstRewardTypeList = FirstRewardTypeList;
                pumpkinChartInfos[i].FirstRewardNameList = FirstRewardNameList;
                pumpkinChartInfos[i].FirstRewardCountList = FirstRewardCountList;

                string[] jsonRewardTypeList = rowData["RewardTypeList"]["S"].ToString().Split('-');
                ItemType[] RewardTypeList = new ItemType[jsonRewardTypeList.Length];
                for (int j = 0; j < RewardTypeList.Length; j++)
                {
                    RewardTypeList[j] = (ItemType)int.Parse(jsonRewardTypeList[j]);
                }

                string[] RewardNameList = rowData["RewardNameList"]["S"].ToString().Split('-');

                string[] jsonRewardCountList = rowData["RewardCountList"]["S"].ToString().Split('-');
                int[] RewardCountList = new int[jsonRewardCountList.Length];
                for (int j = 0; j < RewardCountList.Length; j++)
                {
                    RewardCountList[j] = int.Parse(jsonRewardCountList[j]);
                }

                pumpkinChartInfos[i].RewardTypeList = RewardTypeList;
                pumpkinChartInfos[i].RewardNameList = RewardNameList;
                pumpkinChartInfos[i].RewardCountList = RewardCountList;
            }

            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class PumpkinChartInfo
{
    public int Level;
    public string Hp;
    public ItemType[] FirstRewardTypeList;
    public string[] FirstRewardNameList;
    public int[] FirstRewardCountList;
    public ItemType[] RewardTypeList;
    public string[] RewardNameList;
    public int[] RewardCountList;
}