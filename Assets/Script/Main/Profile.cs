using Function;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    public Text hpText;
    public Image hp_Fore;
    public Text combatPowerText;

    private void Start()
    {
        CombatPower_UI_Setting();
    }

    public void Hp_UI_Setting(string currentHp, string maxHp)
    {
        hpText.text = currentHp + "/" + maxHp;

        float fillAmount = MyMath.Amount(currentHp, maxHp);

        hp_Fore.fillAmount = fillAmount;
    }

    public void CombatPower_UI_Setting()
    {
       

        combatPowerText.text = GetTotalPower();
    }

    public string GetTotalPower()
    {
        CombatPowerChartInfo combatPowerChartInfo = CombatPowerChart.instance.combatPowerChartInfo;
        string Atk = MyMath.Multiple(UserInfo.instance.GetAtk(), combatPowerChartInfo.Atk);
        float AtkPercent = UserInfo.instance.GetAtkPercent() * combatPowerChartInfo.AtkPercent;
        string Hp = MyMath.Multiple(UserInfo.instance.GetHp(), combatPowerChartInfo.Hp);
        float HpPercent = UserInfo.instance.GetHpPercent() * combatPowerChartInfo.HpPercent;
        float CriticalPercent = UserInfo.instance.GetCriticalPercent() * combatPowerChartInfo.CriticalPercent;
        float CriticalDamage = UserInfo.instance.GetCriticalDamagePercent() * combatPowerChartInfo.CriticalDamage;
        float AngerTime = UserInfo.instance.GetAngerTime() * combatPowerChartInfo.AngerTime;
        float AngerDamage = UserInfo.instance.GetAngerDamage() * combatPowerChartInfo.AngerDamage;
        float Gold = UserInfo.instance.GetGoldPercent() * combatPowerChartInfo.Gold;
        float SkillColltime = UserInfo.instance.GetSkillColltime() * combatPowerChartInfo.SkillColltime;
        float MasicStone = UserInfo.instance.GetMasicStonePercent() * combatPowerChartInfo.MasicStone;
        float EnhanceStone = UserInfo.instance.GetEnhanceStonePercent() * combatPowerChartInfo.EnhanceStone;
        float TransStone = UserInfo.instance.GetTransStonePercent() * combatPowerChartInfo.TransStone;
        float BossDamage = UserInfo.instance.GetBossAtkPercent() * combatPowerChartInfo.BossDamage;
        float AtkSpeed = UserInfo.instance.GetAutoAtkSpeed() * combatPowerChartInfo.AtkSpeed;

        string total = (AtkPercent + HpPercent + CriticalPercent + CriticalDamage + AngerTime + AngerDamage + Gold +
            SkillColltime + MasicStone + EnhanceStone + TransStone + BossDamage + AtkSpeed).ToString();
        total = MyMath.Add(total, Atk);
        total = MyMath.Add(total, Hp);

        return total;
    }
}
