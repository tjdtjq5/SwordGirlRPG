using Function;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    public GameObject moneyTextPrepab;

    private void Awake()
    {
        instance = this;
    }

    public Transform gold;
    public Transform crystal;

    [Header("스크립트")]
    public AbilityManager abilityManager;

    private void Start()
    {
        GoldInit();
        CrystalInit();
    }
    // 골드
    void GoldInit()
    {
        gold.GetChild(0).Find("Text").GetComponent<Text>().text = MyMath.ValueToString(UserInfo.instance.gold);
    }
    public void GoldAdd(string money)
    {
        UserInfo.instance.gold = MyMath.Add(UserInfo.instance.gold, money);
        GoldInit();
        abilityManager.CheckRedIcon();

        Vector2 prepabPos = new Vector2(gold.GetChild(0).GetChild(2).position.x, gold.GetChild(0).GetChild(2).position.y - 0.2f);
        GameObject prepab = Instantiate(moneyTextPrepab, prepabPos, Quaternion.identity, this.transform);
        prepab.GetComponent<MoneyTextEffect>().Set("+" + MyMath.ValueToString(money), Color.green);
    }
    public void GoldSub(string money)
    {
        UserInfo.instance.gold = MyMath.Sub(UserInfo.instance.gold, money);
        GoldInit();
        abilityManager.CheckRedIcon();
    }
    // 크리스탈
    public void CrystalInit()
    {
        crystal.GetChild(0).Find("Text").GetComponent<Text>().text = UserInfo.instance.crystal.ToString();
    }
    public void CrystalAdd(int money)
    {
        UserInfo.instance.crystal = UserInfo.instance.crystal + money;
        CrystalInit();
    }
    public void CrystalSub(int money)
    {
        UserInfo.instance.crystal = UserInfo.instance.crystal - money;
        CrystalInit();
    }

    // 강화석 
    public void EnhanceStoneAdd(int money)
    {
        UserInfo.instance.enhanceStone = UserInfo.instance.enhanceStone + money;
    }
    public void EnhanceStoneSub(int money)
    {
        UserInfo.instance.enhanceStone = UserInfo.instance.enhanceStone - money;
    }
    // 마력수정
    public void MasicStoneAdd(int money)
    {
        UserInfo.instance.masicStone = UserInfo.instance.masicStone + money;
    }
    public void MasicStoneSub(int money)
    {
        UserInfo.instance.masicStone = UserInfo.instance.masicStone - money;
    }
    // 초월석
    public void TransStoneAdd(int money)
    {
        UserInfo.instance.transStone = UserInfo.instance.transStone + money;
    }
    public void TransStoneSub(int money)
    {
        UserInfo.instance.transStone = UserInfo.instance.transStone - money;
    }
}
