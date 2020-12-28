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
            LoadAbility(() => {
                LoadMasicMissile(() => {
                    LoadWeapone(() => {
                        callback();
                    });
                });
            });
        });
    }

    // === 유저 재화 === // 
    // 1.재화 
    [HideInInspector] public string gold; // 골드 
    [HideInInspector] public int crystal; // 크리스탈
    [HideInInspector] public int masicStone; // 마력수정
    [HideInInspector] public int enhanceStone; // 강화석
    [HideInInspector] public int transStone; // 초월석
    // 2.토벌권
    [HideInInspector] public int punishTiket;
    public void SaveMoney(System.Action callback)
    {
        Param param = new Param();
        param.Add("gold", gold);
        param.Add("crystal", crystal);
        param.Add("masicStone", masicStone);
        param.Add("enhanceStone", enhanceStone);
        param.Add("transStone", transStone);
        param.Add("punishTiket", punishTiket);
        BackendGameInfo.instance.PrivateTableUpdate("Money", param, (() => { callback(); }));
    }
    public void LoadMoney(System.Action callback)
    {
        BackendGameInfo.instance.GetPrivateContents("Money", "gold", () => {
            gold = BackendGameInfo.instance.serverDataList[0];
            BackendGameInfo.instance.GetPrivateContents("Money", "crystal", () => {
                crystal = int.Parse(BackendGameInfo.instance.serverDataList[0]);
                BackendGameInfo.instance.GetPrivateContents("Money", "masicStone", () => {
                    masicStone = int.Parse(BackendGameInfo.instance.serverDataList[0]);
                    BackendGameInfo.instance.GetPrivateContents("Money", "enhanceStone", () => {
                        enhanceStone = int.Parse(BackendGameInfo.instance.serverDataList[0]);
                        BackendGameInfo.instance.GetPrivateContents("Money", "transStone", () => {
                            transStone = int.Parse(BackendGameInfo.instance.serverDataList[0]);
                            BackendGameInfo.instance.GetPrivateContents("Money", "punishTiket", () => {
                                punishTiket = int.Parse(BackendGameInfo.instance.serverDataList[0]);
                                callback();
                            });
                        });
                    });
                });
            },()=> {  });
        },()=> { gold = "1000000"; crystal = 1000000; callback(); });
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

    // 3. 마력검기
    public List<UserMasicMissile> userMasicMissiles = new List<UserMasicMissile>();

    public UserMasicMissile GetUserMasicMissileInfo(string missileName)
    {
        for (int i = 0; i < userMasicMissiles.Count; i++)
        {
            if (userMasicMissiles[i].Name == missileName)
            {
                return userMasicMissiles[i];
            }
        }
        return null;
    } // 유저의 특정 미사일 정보 
    public void PushMasicMissile(string missileName, int num = 1)
    {
        if (GetUserMasicMissileInfo(missileName) != null) // 유저가 가지고 있다면 
        {
            GetUserMasicMissileInfo(missileName).Num += num;
        }
        else
        {
            userMasicMissiles.Add(new UserMasicMissile(missileName, 0));
        }
    } // 미사일 획득 
    public void UpgradeMasicMissile(string missileName, int needNum)
    {
        if (GetUserMasicMissileInfo(missileName) != null)
        {
            GetUserMasicMissileInfo(missileName).Upgrade++;
            GetUserMasicMissileInfo(missileName).Num -= needNum;
        }
        else
        {
            Debug.Log("해당 미사일을 유저가 가지고 있지 않습니다.");
        }
    } // 미사일 업그레이드
    public void EqipMasicMissile(string missileName)
    {
        for (int i = 0; i < userMasicMissiles.Count; i++)
        {
            userMasicMissiles[i].isEqip = false;
        }
        GetUserMasicMissileInfo(missileName).isEqip = true;
    }
    public void SaveMasicMissile(System.Action callback)
    {
        Param param = new Param();
        List<string> data = new List<string>();
        for (int i = 0; i < userMasicMissiles.Count; i++)
        {
            data.Add(userMasicMissiles[i].Name + "/" + userMasicMissiles[i].Upgrade + "/" + userMasicMissiles[i].Num + "/" + userMasicMissiles[i].isEqip);
        }
        param.Add("MasicMissile", data);
        BackendGameInfo.instance.PrivateTableUpdate("CharacterEnhance", param, () => { callback(); });
    }
    public void LoadMasicMissile(System.Action callback)
    {
        userMasicMissiles.Clear();
        BackendGameInfo.instance.GetPrivateContents("CharacterEnhance", "MasicMissile", () => {
            for (int i = 0; i < BackendGameInfo.instance.serverDataList.Count; i++)
            {
                string[] data = BackendGameInfo.instance.serverDataList[i].Split('/');
                userMasicMissiles.Add(new UserMasicMissile(data[0], int.Parse(data[1]), int.Parse(data[2]), bool.Parse(data[3])));
            }
            callback();
        }, () => { userMasicMissiles.Add(new UserMasicMissile(MasicMissileChart.instance.masicMissileChartInfos[0].Name, 0,100,false)); callback(); });
    }

    // === 장비 === //
    // 1. 무기 
    public List<UserWeapone> userWeapones = new List<UserWeapone>();
    public UserWeapone GetUserWeaponeInfo(string name)
    {
        for (int i = 0; i < userWeapones.Count; i++)
        {
            if (userWeapones[i].name == name)
            {
                return userWeapones[i];
            }
        }
        return null;
    } // 특정 무기 정보 
    public void PushWeapone(string name)
    {
        for (int i = 0; i < userWeapones.Count; i++)
        {
            if (userWeapones[i].name == name)
            {
                userWeapones[i].num++;
                return;
            }
        }
        userWeapones.Add(new UserWeapone(name, 0, 1, false));
    } // 무기 획득
    public void UpgradeWeapone(string name, int needNum)
    {
        for (int i = 0; i < userWeapones.Count; i++)
        {
            if (userWeapones[i].name == name)
            {
                userWeapones[i].upgrade++;
                userWeapones[i].num -= needNum;
            }
        }
    } // 무기 업그레이드
    public void EqipWeapone(string name)
    {
        for (int i = 0; i < userWeapones.Count; i++)
        {
            if (userWeapones[i].name == name)
            {
                userWeapones[i].isEqip = true;
            }
            else
            {
                userWeapones[i].isEqip = false;
            }
        }
    } // 무기 장착
    public void SaveWeapone(System.Action callback)
    {
        Param param = new Param();
        List<string> data = new List<string>();
        Debug.Log(userWeapones.Count);
        for (int i = 0; i < userWeapones.Count; i++)
        {
            data.Add(userWeapones[i].name + "/" + userWeapones[i].upgrade + "/" + userWeapones[i].num + "/" + userWeapones[i].isEqip);
        }
        param.Add("Weapone", data);
        BackendGameInfo.instance.PrivateTableUpdate("Eqip", param, () => { Debug.Log("bb"); callback(); });
    }
    public void LoadWeapone(System.Action callback)
    {
        userWeapones.Clear();
        BackendGameInfo.instance.GetPrivateContents("Eqip", "Weapone", () => {
            for (int i = 0; i < BackendGameInfo.instance.serverDataList.Count; i++)
            {
                string[] data = BackendGameInfo.instance.serverDataList[i].Split('/');
                userWeapones.Add(new UserWeapone(data[0], int.Parse(data[1]), int.Parse(data[2]), bool.Parse(data[3])));
            }
            callback();
        }, () => { PushWeapone(WeaponeChart.instance.weaponeChartInfos[0].Name);  callback(); });
    }
}
public class UserMasicMissile
{
    public string Name;
    public int Upgrade;
    public int Num;
    public bool isEqip;

    public UserMasicMissile(string Name, int Num)
    {
        this.Name = Name;
        this.Num = Num;
        this.Upgrade = 0;
        isEqip = false;
    }
    public UserMasicMissile(string Name, int Upgrade, int Num, bool isEqip)
    {
        this.Name = Name;
        this.Upgrade = Upgrade;
        this.Num = Num;
        this.isEqip = isEqip;
    }
}
public class UserWeapone
{
    public string name;
    public int upgrade;
    public int num;
    public bool isEqip;

    public UserWeapone(string name, int upgrade, int num, bool isEqip)
    {
        this.name = name;
        this.upgrade = upgrade;
        this.num = num;
        this.isEqip = isEqip;
    }
}