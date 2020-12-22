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
    public Transform sapphire;

    [Header("스크립트")]
    public AbilityManager abilityManager;

    private void Start()
    {
        GoldInit();
        SapphireInit();
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
    public void SapphireInit()
    {
        sapphire.GetChild(0).Find("Text").GetComponent<Text>().text = UserInfo.instance.sapphire.ToString();
    }
    public void SapphireAdd(int money)
    {
        UserInfo.instance.sapphire = UserInfo.instance.sapphire + money;
        SapphireInit();
    }
    public void SapphireSub(int money)
    {
        UserInfo.instance.sapphire = UserInfo.instance.sapphire - money;
        SapphireInit();
    }
}
