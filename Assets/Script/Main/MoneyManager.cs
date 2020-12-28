using Function;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

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
    void GoldInit()
    {
        gold.GetChild(0).Find("Text").GetComponent<Text>().text = MyMath.ValueToString(UserInfo.instance.gold);
    }
    public void GoldAdd(string money)
    {
        UserInfo.instance.gold = MyMath.Add(UserInfo.instance.gold, money);
        GoldInit();
        abilityManager.CheckRedIcon();
    }
    public void GoldSub(string money)
    {
        UserInfo.instance.gold = MyMath.Sub(UserInfo.instance.gold, money);
        GoldInit();
        abilityManager.CheckRedIcon();
    }
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
}
