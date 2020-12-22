using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEnhanceAbilityChart : MonoBehaviour
{
    [SerializeField] string field;
    public static CharacterEnhanceAbilityChart instance;
    private void Awake() { instance = this; }
    public CharacterEnhanceAbilityChartInfo[] characterEnhanceAbilityChartInfo;

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            characterEnhanceAbilityChartInfo = new CharacterEnhanceAbilityChartInfo[jsonData.Count];

            for (int i = 0; i < jsonData.Count; i++)
            {
                characterEnhanceAbilityChartInfo[i] = new CharacterEnhanceAbilityChartInfo();
                JsonData rowData = jsonData[i];
                characterEnhanceAbilityChartInfo[i].Atk = rowData["Atk"]["S"].ToString();
                characterEnhanceAbilityChartInfo[i].AtkM = rowData["AtkM"]["S"].ToString();
                characterEnhanceAbilityChartInfo[i].AtkPercent = float.Parse(rowData["AtkPercent"]["S"].ToString());
                characterEnhanceAbilityChartInfo[i].AtkPercentM = rowData["AtkPercentM"]["S"].ToString();
                characterEnhanceAbilityChartInfo[i].Hp = rowData["Hp"]["S"].ToString();
                characterEnhanceAbilityChartInfo[i].HpM = rowData["HpM"]["S"].ToString();
                characterEnhanceAbilityChartInfo[i].HpPercent = float.Parse (rowData["HpPercent"]["S"].ToString());
                characterEnhanceAbilityChartInfo[i].HpPercentM = rowData["HpPercentM"]["S"].ToString();
                characterEnhanceAbilityChartInfo[i].CriticalDamage = float.Parse( rowData["CriticalDamage"]["S"].ToString());
                characterEnhanceAbilityChartInfo[i].CriticalDamageM = rowData["CriticalDamageM"]["S"].ToString();
                characterEnhanceAbilityChartInfo[i].AngerDamage = float.Parse (rowData["AngerDamage"]["S"].ToString());
                characterEnhanceAbilityChartInfo[i].AngerDamageM = rowData["AngerDamageM"]["S"].ToString();
                characterEnhanceAbilityChartInfo[i].FaustDamage = float.Parse(rowData["FaustDamage"]["S"].ToString());
                characterEnhanceAbilityChartInfo[i].FaustDamageM = rowData["FaustDamageM"]["S"].ToString();
            }

            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class CharacterEnhanceAbilityChartInfo
{
    public string Atk;
    public string AtkM;
    public float AtkPercent;
    public string AtkPercentM;
    public string Hp;
    public string HpM;
    public float HpPercent;
    public string HpPercentM;
    public float CriticalDamage;
    public string CriticalDamageM;
    public float AngerDamage;
    public string AngerDamageM;
    public float FaustDamage;
    public string FaustDamageM;
}