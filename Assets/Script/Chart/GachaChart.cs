using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaChart : MonoBehaviour
{
    [SerializeField] string field;
    public static GachaChart instance;
    private void Awake() { instance = this; }
    public GachaChartInfo gachaChartInfos;

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            gachaChartInfos = new GachaChartInfo();
            JsonData rowData = jsonData[0];

            gachaChartInfos.GradeType01_Percent = float.Parse(rowData["GradeType01_Percent"]["S"].ToString());
            gachaChartInfos.GradeType02_Percent = float.Parse(rowData["GradeType02_Percent"]["S"].ToString());
            gachaChartInfos.GradeType03_Percent = float.Parse(rowData["GradeType03_Percent"]["S"].ToString());
            gachaChartInfos.GradeType04_Percent = float.Parse(rowData["GradeType04_Percent"]["S"].ToString());
            gachaChartInfos.GradeType05_Percent = float.Parse(rowData["GradeType05_Percent"]["S"].ToString());
            gachaChartInfos.GradeType06_Percent = float.Parse(rowData["GradeType06_Percent"]["S"].ToString());
            gachaChartInfos.MasicMissile_Crystal_Num = int.Parse(rowData["MasicMissile_Crystal_Num"]["S"].ToString());
            gachaChartInfos.Weapone_Crystal_Num = int.Parse(rowData["Weapone_Crystal_Num"]["S"].ToString());
            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class GachaChartInfo
{
    public float GradeType01_Percent;
    public float GradeType02_Percent;
    public float GradeType03_Percent;
    public float GradeType04_Percent;
    public float GradeType05_Percent;
    public float GradeType06_Percent;
    public int MasicMissile_Crystal_Num;
    public int Weapone_Crystal_Num;
}