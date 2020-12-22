using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class UserInfo : MonoBehaviour
{
    public static UserInfo instance;
    private void Awake()
    {
        instance = this;
    }

    [HideInInspector] public string nickName;
    public void Load(System.Action callback)
    {
        LoadMoney(() => {
            LoadAbility(() => { callback(); });
        });
    }

    // === 유저 재화 === // 
    // 1.재화 
    [HideInInspector] public string gold;
    [HideInInspector] public int sapphire;
    [HideInInspector] public int witchStone;
    [HideInInspector] public int ruby;
    [HideInInspector] public int emerald;
    // 2.토벌권
    [HideInInspector] public int punishTiket;
    public void SaveMoney(System.Action callback)
    {
        Param param = new Param();
        param.Add("gold", gold);
        param.Add("sapphire", sapphire);
        param.Add("witchStone", witchStone);
        param.Add("ruby", ruby);
        param.Add("emerald", emerald);
        param.Add("punishTiket", punishTiket);
        BackendGameInfo.instance.PrivateTableUpdate("Money", param, (() => { callback(); }));
    }
    public void LoadMoney(System.Action callback)
    {
        BackendGameInfo.instance.GetPrivateContents("Money", "gold", () => {
            gold = BackendGameInfo.instance.serverDataList[0];
            BackendGameInfo.instance.GetPrivateContents("Money", "sapphire", () => {
                sapphire = int.Parse(BackendGameInfo.instance.serverDataList[0]);
                BackendGameInfo.instance.GetPrivateContents("Money", "witchStone", () => {
                    witchStone = int.Parse(BackendGameInfo.instance.serverDataList[0]);
                    BackendGameInfo.instance.GetPrivateContents("Money", "ruby", () => {
                        ruby = int.Parse(BackendGameInfo.instance.serverDataList[0]);
                        BackendGameInfo.instance.GetPrivateContents("Money", "emerald", () => {
                            emerald = int.Parse(BackendGameInfo.instance.serverDataList[0]);
                            BackendGameInfo.instance.GetPrivateContents("Money", "punishTiket", () => {
                                punishTiket = int.Parse(BackendGameInfo.instance.serverDataList[0]);
                                callback();
                            });
                        });
                    });
                });
            },()=> {  });
        },()=> { gold = "1000000"; ruby = 1000000; callback(); });
    }

    // === 캐릭터 강화 === //
    // 1. 신체능력
    [HideInInspector] public int abilityAtkLevel;
    [HideInInspector] public int abilityAtkPercentLevel;
    [HideInInspector] public int abilityHpLevel;
    [HideInInspector] public int abilityHpPercentLevel;
    [HideInInspector] public int abilityCriticalDamageLevel;
    [HideInInspector] public int abilityAngerDamageLevel;
    [HideInInspector] public int abilityFaustDamageLevel;
    public void SaveAbility(System.Action callback)
    {
        Param param = new Param();
        List<int> AbilityList = new List<int>();
        AbilityList.Add(abilityAtkLevel);
        AbilityList.Add(abilityAtkPercentLevel);
        AbilityList.Add(abilityHpLevel);
        AbilityList.Add(abilityHpPercentLevel);
        AbilityList.Add(abilityCriticalDamageLevel);
        AbilityList.Add(abilityAngerDamageLevel);
        AbilityList.Add(abilityFaustDamageLevel);
        param.Add("Ability", AbilityList);
        BackendGameInfo.instance.PrivateTableUpdate("CharacterEnhance", param,()=> { callback(); });
    }
    public void LoadAbility(System.Action callback)
    {
        BackendGameInfo.instance.GetPrivateContents("CharacterEnhance", "Ability", () => {
            abilityAtkLevel = int.Parse(BackendGameInfo.instance.serverDataList[0]);
            abilityAtkPercentLevel = int.Parse(BackendGameInfo.instance.serverDataList[1]);
            abilityHpLevel = int.Parse(BackendGameInfo.instance.serverDataList[2]);
            abilityHpPercentLevel = int.Parse(BackendGameInfo.instance.serverDataList[3]);
            abilityCriticalDamageLevel = int.Parse(BackendGameInfo.instance.serverDataList[4]);
            abilityAngerDamageLevel = int.Parse(BackendGameInfo.instance.serverDataList[5]);
            abilityFaustDamageLevel = int.Parse(BackendGameInfo.instance.serverDataList[6]);
            callback();
        },()=> { callback(); });
    }
}
