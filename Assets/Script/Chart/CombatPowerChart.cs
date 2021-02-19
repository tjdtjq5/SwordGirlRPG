using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPowerChart : MonoBehaviour
{
    [SerializeField] string field;
    public static CombatPowerChart instance;
    private void Awake() { instance = this; }
    public CombatPowerChartInfo combatPowerChartInfo;

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            combatPowerChartInfo = new CombatPowerChartInfo();
            JsonData rowData = jsonData[0];

            combatPowerChartInfo.Atk = int.Parse(rowData["Atk"]["S"].ToString());
            combatPowerChartInfo.AtkPercent = int.Parse(rowData["AtkPercent"]["S"].ToString());
            combatPowerChartInfo.Hp = int.Parse(rowData["Hp"]["S"].ToString());
            combatPowerChartInfo.HpPercent = int.Parse(rowData["HpPercent"]["S"].ToString());
            combatPowerChartInfo.CriticalPercent = int.Parse(rowData["CriticalPercent"]["S"].ToString());
            combatPowerChartInfo.CriticalDamage = int.Parse(rowData["CriticalDamage"]["S"].ToString());
            combatPowerChartInfo.AngerTime = int.Parse(rowData["AngerTime"]["S"].ToString());
            combatPowerChartInfo.AngerDamage = int.Parse(rowData["AngerDamage"]["S"].ToString());
            combatPowerChartInfo.Gold = int.Parse(rowData["Gold"]["S"].ToString());
            combatPowerChartInfo.SkillColltime = int.Parse(rowData["SkillColltime"]["S"].ToString());
            combatPowerChartInfo.MasicStone = int.Parse(rowData["MasicStone"]["S"].ToString());
            combatPowerChartInfo.EnhanceStone = int.Parse(rowData["EnhanceStone"]["S"].ToString());
            combatPowerChartInfo.TransStone = int.Parse(rowData["TransStone"]["S"].ToString());
            combatPowerChartInfo.BossDamage = int.Parse(rowData["BossDamage"]["S"].ToString());
            combatPowerChartInfo.AtkSpeed = int.Parse(rowData["AtkSpeed"]["S"].ToString());
            if (loadAction != null) loadAction();
        });
    }
}
public class CombatPowerChartInfo
{
    public int Atk;
    public int AtkPercent;
    public int Hp;
    public int HpPercent;
    public int CriticalPercent;
    public int CriticalDamage;
    public int AngerTime;
    public int AngerDamage;
    public int Gold;
    public int SkillColltime;
    public int MasicStone;
    public int EnhanceStone;
    public int TransStone;
    public int BossDamage;
    public int AtkSpeed;
}