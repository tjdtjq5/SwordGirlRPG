using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class RelicChart : MonoBehaviour
{
    [SerializeField] string field;
    public static RelicChart instance;
    private void Awake() { instance = this; }
    public SpriteAtlas relicSpriteAtlas;
    public RelicChartInfo[] relicChartInfos;

    public List<string> RelicType()
    {
        List<string> name = new List<string>();
        for (int i = 0; i < relicChartInfos.Length; i++)
        {
            if (!name.Contains(relicChartInfos[i].Name))
            {
                name.Add(relicChartInfos[i].Name);
            }
        }
        return name;
    } // 모든 초기 무기정보

    public List<RelicChartInfo> GetRelicChartInfo(string name)
    {
        List<RelicChartInfo> temp = new List<RelicChartInfo>();

        for (int i = 0; i < relicChartInfos.Length; i++)
        {
            if (relicChartInfos[i].Name == name)
            {
                temp.Add(relicChartInfos[i]);
            }
        }
        return temp;
    }
    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            relicChartInfos = new RelicChartInfo[jsonData.Count];

            for (int i = 0; i < jsonData.Count; i++)
            {
                relicChartInfos[i] = new RelicChartInfo();
                JsonData rowData = jsonData[i];
                relicChartInfos[i].Name = rowData["Name"]["S"].ToString();
                relicChartInfos[i].Image = relicSpriteAtlas.GetSprite(rowData["Image"]["S"].ToString());
                relicChartInfos[i].GradeType = (GradeType)(int.Parse(rowData["GradeType"]["S"].ToString()));
                relicChartInfos[i].ClearBossName = rowData["ClearBossName"]["S"].ToString();
                relicChartInfos[i].EnhanceAbilityType = (AbilityType)(int.Parse(rowData["EnhanceAbilityType"]["S"].ToString()));
                relicChartInfos[i].EnhanceAbilityIncrease = float.Parse(rowData["EnhanceAbilityIncrease"]["S"].ToString());
                relicChartInfos[i].Enhance_M_Default = int.Parse(rowData["Enhance_M_Default"]["S"].ToString());
                relicChartInfos[i].Enhance_M_Increase = int.Parse(rowData["Enhance_M_Increase"]["S"].ToString());
                relicChartInfos[i].StarEnhanceAbilityType = (AbilityType)(int.Parse(rowData["StarEnhanceAbilityType"]["S"].ToString()));
                relicChartInfos[i].StarEnhanceAbilityIncrease = float.Parse(rowData["StarEnhanceAbilityIncrease"]["S"].ToString());
                relicChartInfos[i].CombinationNum = int.Parse(rowData["CombinationNum"]["S"].ToString());
            }

            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class RelicChartInfo
{
    public string Name;
    public Sprite Image;
    public GradeType GradeType;
    public string ClearBossName;
    public AbilityType EnhanceAbilityType;
    public float EnhanceAbilityIncrease;
    public int Enhance_M_Default;
    public int Enhance_M_Increase;
    public AbilityType StarEnhanceAbilityType;
    public float StarEnhanceAbilityIncrease;
    public int CombinationNum;
}