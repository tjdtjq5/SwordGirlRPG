using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WeaponeChart : MonoBehaviour
{
    [SerializeField] string field;
    public static WeaponeChart instance;
    private void Awake() { instance = this; }
    public SpriteAtlas weaponeSpriteAtlas;
    public WeaponeChartInfo[] weaponeChartInfos;

    public List<string> WeaponeType()
    {
        List<string> name = new List<string>();
        for (int i = 0; i < weaponeChartInfos.Length; i++)
        {
            if (!name.Contains(weaponeChartInfos[i].Name))
            {
                name.Add(weaponeChartInfos[i].Name);
            }
        }
        return name;
    } // 모든 초기 무기정보

    public List<string> WeaponeType(GradeType gradeType) // 등급별 초기 무기정보
    {
        List<string> temp = new List<string>();
        for (int i = 0; i < weaponeChartInfos.Length; i++)
        {
            if (weaponeChartInfos[i].GradeType == gradeType && !temp.Contains(weaponeChartInfos[i].Name))
            {
                temp.Add(weaponeChartInfos[i].Name);
            }
        }
        return temp;
    }

    public List<WeaponeChartInfo> GetWeaponeChartInfo(string name)
    {
        List<WeaponeChartInfo> temp = new List<WeaponeChartInfo>();

        for (int i = 0; i < weaponeChartInfos.Length; i++)
        {
            if (weaponeChartInfos[i].Name == name)
            {
                temp.Add(weaponeChartInfos[i]);
            }
        }
        return temp;
    }

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            weaponeChartInfos = new WeaponeChartInfo[jsonData.Count];

            for (int i = 0; i < jsonData.Count; i++)
            {
                weaponeChartInfos[i] = new WeaponeChartInfo();
                JsonData rowData = jsonData[i];
                weaponeChartInfos[i].Name = rowData["Name"]["S"].ToString();
                weaponeChartInfos[i].Image = weaponeSpriteAtlas.GetSprite(rowData["Image"]["S"].ToString());
                weaponeChartInfos[i].GradeType = (GradeType)(int.Parse(rowData["GradeType"]["S"].ToString()));
                weaponeChartInfos[i].AtkPercent = float.Parse(rowData["AtkPercent"]["S"].ToString());
                weaponeChartInfos[i].AtkSpeed = float.Parse(rowData["AtkSpeed"]["S"].ToString());
                weaponeChartInfos[i].CriticalPercent = float.Parse(rowData["CriticalPercent"]["S"].ToString());
                weaponeChartInfos[i].ConbinationNum = int.Parse(rowData["ConbinationNum"]["S"].ToString());
            }

            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class WeaponeChartInfo
{
    public string Name;
    public Sprite Image;
    public GradeType GradeType;
    public float AtkPercent;
    public float AtkSpeed;
    public float CriticalPercent;
    public int ConbinationNum;
}