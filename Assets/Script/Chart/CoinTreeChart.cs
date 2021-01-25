using BackEnd;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CoinTreeChart : MonoBehaviour
{
    [SerializeField] string field;
    public static CoinTreeChart instance;
    private void Awake() { instance = this; }
    CoinTreeChartInfo[] coinTreeChartInfos;

    public int MaxLevel()
    {
        return coinTreeChartInfos.Length;
    }
    public string GetCoinTreeHp(int level)
    {
        return coinTreeChartInfos[level].Hp;
    }

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            coinTreeChartInfos = new CoinTreeChartInfo[jsonData.Count];

            for (int i = 0; i < jsonData.Count; i++)
            {
                coinTreeChartInfos[i] = new CoinTreeChartInfo();
                JsonData rowData = jsonData[i];
                coinTreeChartInfos[i].Hp = rowData["Hp"]["S"].ToString();
            }

            if (loadAction != null) loadAction();
        });
    }
}
public class CoinTreeChartInfo
{
    public string Hp;
}