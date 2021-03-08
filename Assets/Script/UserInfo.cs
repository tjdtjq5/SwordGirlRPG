using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;
using Function;
using LitJson;

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
                        LoadRelic(() => {
                            LoadPublicUserInfo(() => {
                                LoadUserNomalMonster(() => {
                                    LoadUserDayByQuest(() => {
                                        LoadUserWeekByQuest(() => {
                                            LoadUserPass(() => {
                                                LoadCoinTree(() => {
                                                    LoadUserViolet(() => {
                                                        callback();
                                                    });
                                                });
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    }

    [ContextMenu("테스트")]
    public void Test()
    {
        Debug.Log("공격력 : " + GetAtk());
        Debug.Log("공격력퍼센트 : " + GetAtkPercent());
        Debug.Log("체력 : " + GetHp());
        Debug.Log("체력퍼센트 : " + GetHpPercent());
        Debug.Log("크리티컬 : " + GetCriticalPercent());
        Debug.Log("크뎀 : " + GetCriticalDamagePercent());
        Debug.Log("분노시간 : " + GetAngerTime());
        Debug.Log("분노데미지 : " + GetAngerDamage());
        Debug.Log("골드 : " + GetGoldPercent());
        Debug.Log("스킬쿨타임 : " + GetSkillColltime());
        Debug.Log("마력석 : " + GetMasicStonePercent());
        Debug.Log("강화석 : " + GetEnhanceStonePercent());
        Debug.Log("분노데미지 : " + GetTransStonePercent());
        Debug.Log("보스공격력 : " + GetBossAtkPercent());
        Debug.Log("공격속도 : " + GetAutoAtkSpeed());
    }
    [ContextMenu("테스트2")]
    public void Test2()
    {
        string damage = MyMath.Multiple("99999", 1.5F);
        Debug.Log(damage);
    }

    /// === 능력치 === ///
    public string GetAtk()
    {
        string total = "";
        // 신체능력 
        string abilityAtk = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[abilityAtkLevel].Atk;
        // 유물 
        string relic = "0";
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.공격력)
            {
                string enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease).ToString();
                relic += MyMath.Add(relic, enhanceAbilityIncrease);
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.공격력)
            {
                string starEnhanceAbilityIncrease =  (relicChartInfo.StarEnhanceAbilityIncrease).ToString();
                relic += MyMath.Add(relic, starEnhanceAbilityIncrease);
            }
        }

        total = MyMath.Add(abilityAtk, relic);
        return total;
    }
    public float GetAtkPercent()
    {
        float total = 1;
        // 신체능력 
        float abilityAtkPercent = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[abilityAtkPercentLevel].AtkPercent;
        // 마력검기 
        float masicMissile = 0;
        UserMasicMissile userMasicMissile = GetEqipMasicMissile();
        if (userMasicMissile != null)
        {
            MasicMissileChartInfo masicMissileInfo = MasicMissileChart.instance.GetMasicMissileInfo(userMasicMissile.Name)[userMasicMissile.Upgrade];
            masicMissile = masicMissileInfo.AbilityNum;
        }
        // 무기 
        UserWeapone userWeapone = GetEqipWeapone();
        float weaponeAtkPercent = 0;
        if (userWeapone != null)
        {
            WeaponeChartInfo weaponeChart = WeaponeChart.instance.GetWeaponeChartInfo(userWeapone.name)[userWeapone.upgrade];
            weaponeAtkPercent = weaponeChart.AtkPercent;
        }
        // 유물 
        float relic = 0;
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.공격력퍼센트)
            {
                float enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease);
                relic += enhanceAbilityIncrease;
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.공격력퍼센트)
            {
                float starEnhanceAbilityIncrease = relicChartInfo.StarEnhanceAbilityIncrease;
                relic += starEnhanceAbilityIncrease;
            }
        }
        total += abilityAtkPercent;
        total += masicMissile;
        total += weaponeAtkPercent;
        total += relic;

        return total;
    }
    public string GetHp()
    {
        string total = "";
        // 신체능력 
        string abilityHp = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[abilityHpLevel].Hp;
        // 유물 
        string relic = "0";
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.체력)
            {
                string enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease).ToString();
                relic += MyMath.Add(relic, enhanceAbilityIncrease);
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.체력)
            {
                string starEnhanceAbilityIncrease = (relicChartInfo.StarEnhanceAbilityIncrease).ToString();
                relic += MyMath.Add(relic, starEnhanceAbilityIncrease);
            }
        }
        total = MyMath.Add(abilityHp, relic);
        return total;
    }
    public float GetHpPercent()
    {
        float total = 1;
        // 신체능력 
        float abilityHpPercent = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[abilityHpPercentLevel].HpPercent;
        // 유물 
        float relic = 0;
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.체력퍼센트)
            {
                float enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease);
                relic += enhanceAbilityIncrease;
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.체력퍼센트)
            {
                float starEnhanceAbilityIncrease = relicChartInfo.StarEnhanceAbilityIncrease;
                relic += starEnhanceAbilityIncrease;
            }
        }
        // 복장
        UserCloth userCloth = GetEqipCloth();
        float cloth = 0;
        if (userCloth != null)
        {
            ClothChartInfo clothChartInfo = ClothChart.instance.GetClothChartInfo(userCloth.name)[userCloth.upgrade];
            cloth += clothChartInfo.HpPercent;
        }

        total += abilityHpPercent;
        total += relic;
        total += cloth;

        return total;
    }
    public float GetCriticalPercent()
    {
        float total = 0;

        // 무기 
        UserWeapone userWeapone = GetEqipWeapone();
        float weaponeCriticalPercent = 0;
        if (userWeapone != null)
        {
            WeaponeChartInfo weaponeChart = WeaponeChart.instance.GetWeaponeChartInfo(userWeapone.name)[userWeapone.upgrade];
            weaponeCriticalPercent = weaponeChart.CriticalPercent;
        }
        // 유물 
        float relic = 0;
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.크리티컬퍼센트)
            {
                float enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease);
                relic += enhanceAbilityIncrease;
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.크리티컬퍼센트)
            {
                float starEnhanceAbilityIncrease = relicChartInfo.StarEnhanceAbilityIncrease;
                relic += starEnhanceAbilityIncrease;
            }
        }

        total += weaponeCriticalPercent;
        total += relic;

        return total;
    }
    public float GetCriticalDamagePercent()
    {
        float total = 1;

        // 신체능력 
        float abilityCriticalDamagePercent = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[abilityCriticalDamageLevel].CriticalDamage;
        // 유물 
        float relic = 0;
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.크리티컬데미지)
            {
                float enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease);
                relic += enhanceAbilityIncrease;
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.크리티컬데미지)
            {
                float starEnhanceAbilityIncrease = relicChartInfo.StarEnhanceAbilityIncrease;
                relic += starEnhanceAbilityIncrease;
            }
        }

        total += abilityCriticalDamagePercent;
        total += relic;

        return total;
    }
    public float GetAngerTime()
    {
        float total = 0;
        // 유물 
        float relic = 0;
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.분노시간)
            {
                float enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease);
                relic += enhanceAbilityIncrease;
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.분노시간)
            {
                float starEnhanceAbilityIncrease = relicChartInfo.StarEnhanceAbilityIncrease;
                relic += starEnhanceAbilityIncrease;
            }
        }

        total += relic;

        return total;

    }
    public float GetAngerDamage()
    {
        float total = 0;
        // 신체능력 
        float abilityAngerDamage = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[abilityAngerDamageLevel].AngerDamage;
        // 유물
        float relic = 0;
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.분노데미지)
            {
                float enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease);
                relic += enhanceAbilityIncrease;
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.분노데미지)
            {
                float starEnhanceAbilityIncrease = relicChartInfo.StarEnhanceAbilityIncrease;
                relic += starEnhanceAbilityIncrease;
            }
        }

        total += abilityAngerDamage;
        total += relic;

        return total;
    }
    public float GetGoldPercent()
    {
        float total = 0;
        // 유물
        float relic = 0;
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.골드추가획득량)
            {
                float enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease);
                relic += enhanceAbilityIncrease;
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.골드추가획득량)
            {
                float starEnhanceAbilityIncrease = relicChartInfo.StarEnhanceAbilityIncrease;
                relic += starEnhanceAbilityIncrease;
            }
        }

        total += relic;


        return total;
    }
    public float GetSkillColltime()
    {
        float total = 0;
        // 유물
        float relic = 0;
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.스킬쿨타임)
            {
                float enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease);
                relic += enhanceAbilityIncrease;
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.스킬쿨타임)
            {
                float starEnhanceAbilityIncrease = relicChartInfo.StarEnhanceAbilityIncrease;
                relic += starEnhanceAbilityIncrease;
            }
        }

        total += relic;

        return total;
    }
    public float GetMasicStonePercent()
    {
        float total = 0;
        // 유물
        float relic = 0;
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.마력수정추가획득량)
            {
                float enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease);
                relic += enhanceAbilityIncrease;
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.마력수정추가획득량)
            {
                float starEnhanceAbilityIncrease = relicChartInfo.StarEnhanceAbilityIncrease;
                relic += starEnhanceAbilityIncrease;
            }
        }

        total += relic;

        return total;

    }
    public float GetEnhanceStonePercent()
    {
        float total = 0;
        // 유물
        float relic = 0;
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.마력수정추가획득량)
            {
                float enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease);
                relic += enhanceAbilityIncrease;
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.마력수정추가획득량)
            {
                float starEnhanceAbilityIncrease = relicChartInfo.StarEnhanceAbilityIncrease;
                relic += starEnhanceAbilityIncrease;
            }
        }

        total += relic;

        return total;
    }
    public float GetTransStonePercent()
    {
        float total = 0;
        // 유물
        float relic = 0;
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.초월석추가획득량)
            {
                float enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease);
                relic += enhanceAbilityIncrease;
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.초월석추가획득량)
            {
                float starEnhanceAbilityIncrease = relicChartInfo.StarEnhanceAbilityIncrease;
                relic += starEnhanceAbilityIncrease;
            }
        }

        total += relic;

        return total;
    }
    public float GetBossAtkPercent()
    {
        float total = 0;
        // 유물
        float relic = 0;
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.보스공격력)
            {
                float enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease);
                relic += enhanceAbilityIncrease;
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.보스공격력)
            {
                float starEnhanceAbilityIncrease = relicChartInfo.StarEnhanceAbilityIncrease;
                relic += starEnhanceAbilityIncrease;
            }
        }
        // 신체능력 
        float abilityBossAtkPercent = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[abilityFaustDamageLevel].FaustDamage;

        total += abilityBossAtkPercent;
        total += relic;

        return total;
    }
    public float GetAutoAtkSpeed()
    {
        float total = 0;

        // 무기 
        UserWeapone userWeapone = GetEqipWeapone();
        float weaponeAtkSpeed = 0;
        if (userWeapone != null)
        {
            WeaponeChartInfo weaponeChart = WeaponeChart.instance.GetWeaponeChartInfo(userWeapone.name)[userWeapone.upgrade];
            weaponeAtkSpeed = weaponeChart.AtkSpeed;
        }
        // 유물
        float relic = 0;
        for (int i = 0; i < userRelics.Count; i++)
        {
            RelicChartInfo relicChartInfo = RelicChart.instance.GetRelicChartInfo(userRelics[i].name)[(int)userRelics[i].gradeType - 1];
            if (relicChartInfo.EnhanceAbilityType == AbilityType.자동공격속도)
            {
                float enhanceAbilityIncrease = ((userRelics[i].upgrade + 1) * relicChartInfo.EnhanceAbilityIncrease);
                relic += enhanceAbilityIncrease;
            }
            if (relicChartInfo.StarEnhanceAbilityType == AbilityType.자동공격속도)
            {
                float starEnhanceAbilityIncrease = relicChartInfo.StarEnhanceAbilityIncrease;
                relic += starEnhanceAbilityIncrease;
            }
        }

        total += weaponeAtkSpeed;
        total += relic;

        // 최대수치
        if (total > 90) total = 90;

        return total;
    }

    // === 유저 공용 정보 === // 
    public UserPublicInfo userPublicInfo = null;
    public void SavePublicUserInfo(System.Action callback)
    {
        if (userPublicInfo == null)
        {
            callback();
            return;
        }

        Param param = new Param();

        // 복장
        List<string> clothData = new List<string>();
        for (int i = 0; i < userCloths.Count; i++)
        {
            clothData.Add(userCloths[i].name + "/" + userCloths[i].upgrade + "/" + userCloths[i].num + "/" + userCloths[i].isEqip);
        }
        param.Add("Cloth", clothData);

        BackendGameInfo.instance.PublicTableUpdate("PublicUserInfo", param, () => { callback(); });
    }
    public void LoadPublicUserInfo(System.Action callback)
    {
        string nickname = "";
        userCloths.Clear();

        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetMyPublicContents, "PublicUserInfo", (backendCallback) => {

            switch (backendCallback.GetStatusCode())
            {
                case "200": // 성공 
                    break;
                case "404": // 존재하지 않는 tableName인 경우 
                    Debug.Log("존재하지 않는 tableName인 경우 ");
                    break;
                case "400": // private table 아닌 tableName을 입력한 경우 또는 limit이 100이상인 경우
                    Debug.Log("public table 아닌 tableName 을 입력한 경우 또는 limit이 100이상인 경우");
                    break;
                case "412": // 비활성화 된 tableName인 경우 
                    Debug.Log("비활성화 된 tableName인 경우 ");
                    break;
                default:
                    break;
            }
            if (backendCallback.GetReturnValuetoJSON()[0].Count == 0) // 테이블에 해당 유저의 정보가 아무것도 없는 경우
            {
                string clothName = ClothChart.instance.clothChartInfos[0].Name;
                PushCloth(clothName);
                EqipCloth(clothName);

                userPublicInfo = new UserPublicInfo(nickname, userCloths);
                SavePublicUserInfo(() => { callback(); });
            }
            else
            {
                for (int i = 0; i < backendCallback.GetReturnValuetoJSON()[0].Count; i++)
                {
                    JsonData jsonData = backendCallback.GetReturnValuetoJSON()[0][i];

                    // 닉네임
                    if (jsonData.Keys.Contains("nickname"))
                    {
                        nickname = jsonData["nickname"][0].ToString();
                    }

                    // 복장
                    if (jsonData.Keys.Contains("Cloth"))
                    {
                        JsonData keyData = jsonData["Cloth"][0];
                        for (int j = 0; j < keyData.Count; j++)
                        {
                            string[] data = keyData[j][0].ToString().Split('/');
                            userCloths.Add(new UserCloth(data[0], int.Parse(data[1]), int.Parse(data[2]), bool.Parse(data[3])));
                        }
                    }
                }

                userPublicInfo = new UserPublicInfo(nickname, userCloths);
                callback();
            }
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
        BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPrivateContents, "Money", (backendCallback) => {

            string stateCode = backendCallback.GetStatusCode();
            switch (stateCode)
            {
                case "200": // 성공 
                    break;
                case "404": // 존재하지 않는 tableName인 경우 
                    Debug.Log("존재하지 않는 tableName인 경우 ");
                    break;
                case "400": // private table 아닌 tableName을 입력한 경우 또는 limit이 100이상인 경우
                    Debug.Log("private table 아닌 tableName을 입력한 경우 또는 limit이 100이상인 경우");
                    break;
                case "412": // 비활성화 된 tableName인 경우 
                    Debug.Log("비활성화 된 tableName인 경우 ");
                    break;
                default:
                    break;
            }
            if (backendCallback.GetReturnValuetoJSON()[0].Count == 0) // 테이블에 해당 유저의 정보가 아무것도 없는 경우
            {
                gold = "1000000";
                crystal = 100000;
            }
            else
            {
                JsonData jsonData = backendCallback.GetReturnValuetoJSON()[0][0];
                if (jsonData.Keys.Contains("gold"))
                {
                    gold = jsonData["gold"][0].ToString();
                }
                else
                {
                    gold = "0";
                }
                if (jsonData.Keys.Contains("crystal"))
                {
                    crystal = int.Parse(jsonData["crystal"][0].ToString());
                }
                else
                {
                    crystal = 0;
                }
                if (jsonData.Keys.Contains("masicStone"))
                {
                    masicStone = int.Parse(jsonData["masicStone"][0].ToString());
                }
                else
                {
                    masicStone = 0;
                }
                if (jsonData.Keys.Contains("enhanceStone"))
                {
                    enhanceStone = int.Parse(jsonData["enhanceStone"][0].ToString());
                }
                else
                {
                    enhanceStone = 0;
                }
                if (jsonData.Keys.Contains("transStone"))
                {
                    transStone = int.Parse(jsonData["transStone"][0].ToString());
                }
                else
                {
                    transStone = 0;
                }
                if (jsonData.Keys.Contains("punishTiket"))
                {
                    punishTiket = int.Parse(jsonData["punishTiket"][0].ToString());
                }
                else
                {
                    punishTiket = 0;
                }
            }

            callback();
        });
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
    public UserMasicMissile GetEqipMasicMissile()
    {
        for (int i = 0; i < userMasicMissiles.Count; i++)
        {
            if (userMasicMissiles[i].isEqip == true)
            {
                return userMasicMissiles[i];
            }
        }
        return null;
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
    public UserWeapone GetEqipWeapone()
    {
        for (int i = 0; i < userWeapones.Count; i++)
        {
            if (userWeapones[i].isEqip == true)
            {
                return userWeapones[i];
            }
        }
        return null;
    }
    public void SaveWeapone(System.Action callback)
    {
        Param param = new Param();
        List<string> data = new List<string>();
        for (int i = 0; i < userWeapones.Count; i++)
        {
            data.Add(userWeapones[i].name + "/" + userWeapones[i].upgrade + "/" + userWeapones[i].num + "/" + userWeapones[i].isEqip);
        }
        param.Add("Weapone", data);
        BackendGameInfo.instance.PrivateTableUpdate("Eqip", param, () => {  callback(); });
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
        }, () => {
            string defaultWeapon = WeaponeChart.instance.weaponeChartInfos[0].Name;
            PushWeapone(defaultWeapon);
            EqipWeapone(defaultWeapon);
            callback(); 
        });
    }
    // 3. 유물
    public List<UserRelic> userRelics = new List<UserRelic>();
    public UserRelic GetUserRelicInfo(string name)
    {
        for (int i = 0; i < userRelics.Count; i++)
        {
            if (userRelics[i].name == name)
            {
                return userRelics[i];
            }
        }
        return null;
    } // 특정 유물 정보
    public void PushRelic(string name)
    {
        for (int i = 0; i < userRelics.Count; i++)
        {
            if (userRelics[i].name == name)
            {
                userRelics[i].num++;
                return;
            }
        }
        userRelics.Add(new UserRelic(name, GradeType.별1개, 0, 1));
    } // 유물 획득
    public void UpgradeRelic(string name)
    {
        for (int i = 0; i < userRelics.Count; i++)
        {
            if (userRelics[i].name == name)
            {
                userRelics[i].upgrade++;
            }
        }
    } // 유물 업그레이드
    public void StarUpgradeRelic(string name, int needNum)
    {
        for (int i = 0; i < userRelics.Count; i++)
        {
            if (userRelics[i].name == name)
            {
                userRelics[i].gradeType = (GradeType)((int)userRelics[i].gradeType + 1);
                userRelics[i].num -= needNum;
            }
        }
    } // 유물 성급 업그레이드
    public void SaveRelic(System.Action callback)
    {
        Param param = new Param();
        List<string> data = new List<string>();
        for (int i = 0; i < userRelics.Count; i++)
        {
            data.Add(userRelics[i].name + "/" + userRelics[i].gradeType + "/" + userRelics[i].upgrade + "/" + userRelics[i].num);
        }
        param.Add("Relic", data);
        BackendGameInfo.instance.PrivateTableUpdate("Eqip", param, () => { callback(); });
    }
    public void LoadRelic(System.Action callback)
    {
        userRelics.Clear();
        BackendGameInfo.instance.GetPrivateContents("Eqip", "Relic", () => {
            for (int i = 0; i < BackendGameInfo.instance.serverDataList.Count; i++)
            {
                string[] data = BackendGameInfo.instance.serverDataList[i].Split('/');
                userRelics.Add(new UserRelic(data[0], (GradeType)System.Enum.Parse(typeof(GradeType), data[1]), int.Parse(data[2]), int.Parse(data[3])));
            }
            callback();
        }, () => { PushRelic(RelicChart.instance.relicChartInfos[0].Name); callback(); });
    }
    // 4. 복장
    public List<UserCloth> userCloths = new List<UserCloth>();
    public UserCloth GetUserClothInfo(string name)
    {
        for (int i = 0; i < userCloths.Count; i++)
        {
            if (userCloths[i].name == name)
            {
                return userCloths[i];
            }
        }
        return null;
    } // 특정 복장 정보
    public void PushCloth(string name)
    {
        for (int i = 0; i < userCloths.Count; i++)
        {
            if (userCloths[i].name == name)
            {
                userCloths[i].num++;
                return;
            }
        }
        userCloths.Add(new UserCloth(name, 0, 1, false));
    } // 복장 획득
    public void UpgradeCloth(string name, int needNum)
    {
        for (int i = 0; i < userCloths.Count; i++)
        {
            if (userCloths[i].name == name)
            {
                userCloths[i].upgrade++;
                userCloths[i].num -= needNum;
            }
        }
    } // 복장 업그레이드
    public void EqipCloth(string name)
    {
        for (int i = 0; i < userCloths.Count; i++)
        {
            if (userCloths[i].name == name)
            {
                userCloths[i].isEqip = true;
            }
            else
            {
                userCloths[i].isEqip = false;
            }
        }
    } // 복장 장착
    public UserCloth GetEqipCloth()
    {
        for (int i = 0; i < userCloths.Count; i++)
        {
            if (userCloths[i].isEqip == true)
            {
                return userCloths[i];
            }
        }
        return null;
    }
    public void SaveCloth(System.Action callback)
    {
        Param param = new Param();
        List<string> data = new List<string>();
        for (int i = 0; i < userCloths.Count; i++)
        {
            data.Add(userCloths[i].name + "/" + userCloths[i].upgrade + "/" + userCloths[i].num + "/" + userCloths[i].isEqip);
        }
        param.Add("Cloth", data);
        BackendGameInfo.instance.PublicTableUpdate("PublicUserInfo", param, () => { callback(); });
    }
  

    /// === 토벌 === ///
    // 1. 노멀 몬스터 
    public List<UserNomalMonster> userNomalMonsters = new List<UserNomalMonster>();
    public void ClearNomalMonster(string name)
    {
        BackendReturnObject servertime = Backend.Utils.GetServerTime();
        string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
        DateTime currentTime = DateTime.Parse(time);

        for (int i = 0; i < userNomalMonsters.Count; i++)
        {
            if (userNomalMonsters[i].name == name)
            {
                userNomalMonsters[i].count++;
             
                userNomalMonsters[i].clearDateTime = currentTime;
                return;
            }
        }
        userNomalMonsters.Add(new UserNomalMonster(name, 1, currentTime));
    } // 몬스터를 클리어시 정보 넣기 
    public UserNomalMonster GetUserNomalMonsterInfo(string name)
    {
        for (int i = 0; i < userNomalMonsters.Count; i++)
        {
            if (userNomalMonsters[i].name == name)
            {
                return userNomalMonsters[i];
            }
        }
        return null;
    } // 유저 몬스터 정보 얻기 
    public void SaveUserNomalMonster(System.Action callback)
    {
        Param param = new Param();
        List<string> data = new List<string>();
        for (int i = 0; i < userNomalMonsters.Count; i++)
        {
            data.Add(userNomalMonsters[i].name + "/" + userNomalMonsters[i].count + "/" + userNomalMonsters[i].clearDateTime);
        }
        param.Add("NomalMonster", data);
        BackendGameInfo.instance.PrivateTableUpdate("Monster", param, () => { callback(); });
    }
    public void LoadUserNomalMonster(System.Action callback)
    {
        userNomalMonsters.Clear();
        BackendGameInfo.instance.GetPrivateContents("Monster", "NomalMonster", () => {
            for (int i = 0; i < BackendGameInfo.instance.serverDataList.Count; i++)
            {
                string[] data = BackendGameInfo.instance.serverDataList[i].Split('/');
                userNomalMonsters.Add(new UserNomalMonster(data[0], int.Parse(data[1]), DateTime.Parse(data[2])));
            }
            callback();
        }, () => { callback(); });
    }

    /// === 퀘스트 === ///
    // 1. 일일 퀘스트 
    public List<UserDayByQuest> userDayByQuests = new List<UserDayByQuest>();
    public int GetUserDayByQuestCount(int ID) // 해당 퀘스트의 클리어 횟수를 가져옴 
    {
        for (int i = 0; i < userDayByQuests.Count; i++)
        {
            if (userDayByQuests[i].ID == ID)
            {
                DateTime currentTime = TimeManager.instance.currentTime;

                if (userDayByQuests[i].time.Year == currentTime.Year && userDayByQuests[i].time.Month == currentTime.Month && userDayByQuests[i].time.Day == currentTime.Day)
                {
                    return userDayByQuests[i].count;
                }
                else
                {
                    userDayByQuests[i].count = 0;
                    return 0;
                }
            }
        }
        return 0;
    }
    public bool GetUserDayByQuestComplete(int ID) // 해당 퀘스트의 완료여부 가져옴
    {
        for (int i = 0; i < userDayByQuests.Count; i++)
        {
            if (userDayByQuests[i].ID == ID)
            {
                DateTime currentTime = TimeManager.instance.currentTime;

                if (userDayByQuests[i].time.Year == currentTime.Year && userDayByQuests[i].time.Month == currentTime.Month && userDayByQuests[i].time.Day == currentTime.Day)
                {
                    return userDayByQuests[i].complete;
                }
                else
                {
                    userDayByQuests[i].time = currentTime;
                    userDayByQuests[i].complete = false;
                    return false;
                }
            }
        }
        return false;
    }  
    public void PushUserDayByQuestCount(int ID) // 해당 퀘스트에 클리어횟수 추가 
    {
        DateTime currentTime = TimeManager.instance.currentTime;

        for (int i = 0; i < userDayByQuests.Count; i++)
        {
            if (userDayByQuests[i].ID == ID)
            {
                if (userDayByQuests[i].time.Year == currentTime.Year && userDayByQuests[i].time.Month == currentTime.Month && userDayByQuests[i].time.Day == currentTime.Day)
                {
                    userDayByQuests[i].count++;
                    return;
                }
                else
                {
                    userDayByQuests[i].time = currentTime;
                    userDayByQuests[i].count = 1;
                    return;
                }
            }
        }

        userDayByQuests.Add(new UserDayByQuest(ID, currentTime, 1, false));
    }
    public void PushUserDayByQuestComplete(int ID) // 해당 퀘스트 완료시키기 
    {
        for (int i = 0; i < userDayByQuests.Count; i++)
        {
            if (userDayByQuests[i].ID == ID)
            {
                DateTime currentTime = TimeManager.instance.currentTime;

                userDayByQuests[i].time = currentTime;

                userDayByQuests[i].complete = true;

                DayByQuestChartInfo dayByQuestChartInfo = DayByQuestChart.instance.dayByQuestChartInfos[ID];
                PushUserWeekByQuestPoint(dayByQuestChartInfo.Point); // 주간퀘 포인트 적립
                PushUserPassPoint(dayByQuestChartInfo.Point); // 패스 포인트 적립
            }
        }
    }
    public void SaveUserDayByQuest(System.Action callback)
    {
        Param param = new Param();
        List<string> data = new List<string>();
        for (int i = 0; i < userDayByQuests.Count; i++)
        {
            data.Add(userDayByQuests[i].ID + "/" + userDayByQuests[i].time + "/" + userDayByQuests[i].count + "/" + userDayByQuests[i].complete);
        }
        param.Add("Day", data);
        BackendGameInfo.instance.PrivateTableUpdate("Quest", param, () => { callback(); });
    }
    public void LoadUserDayByQuest(System.Action callback)
    {
        userDayByQuests.Clear();
        BackendGameInfo.instance.GetPrivateContents("Quest", "Day", () => {
            for (int i = 0; i < BackendGameInfo.instance.serverDataList.Count; i++)
            {
                string[] data = BackendGameInfo.instance.serverDataList[i].Split('/');
                userDayByQuests.Add(new UserDayByQuest(int.Parse(data[0]), DateTime.Parse(data[1]), int.Parse(data[2]), bool.Parse(data[3])));
            }
            callback();
        }, () => { callback(); });
    }
    // 2. 주간 퀘스트
    public UserWeekByQuest userWeekByQuest = new UserWeekByQuest();
    public void PushUserWeekByQuestPoint(int point) // 주간퀘스트 포인트 적립
    {
        DateTime currentTime = TimeManager.instance.currentTime;
        TimeSpan subTimeTimeSpan = currentTime.Subtract(userWeekByQuest.time);
        DateTime subTime = new DateTime(subTimeTimeSpan.Ticks);

        bool flag = false;
        switch (currentTime.DayOfWeek)
        {
            case DayOfWeek.Monday:
                if (subTime.Day < 1) flag = true;
                break;
            case DayOfWeek.Tuesday:
                if (subTime.Day < 2) flag = true;
                break;
            case DayOfWeek.Wednesday:
                if (subTime.Day < 3) flag = true;
                break;
            case DayOfWeek.Thursday:
                if (subTime.Day < 4) flag = true;
                break;
            case DayOfWeek.Friday:
                if (subTime.Day < 5) flag = true;
                break;
            case DayOfWeek.Saturday:
                if (subTime.Day < 6) flag = true;
                break;
            case DayOfWeek.Sunday:
                if (subTime.Day < 7) flag = true;
                break;
        }

        if (flag)
        {
            userWeekByQuest.point += point;
        }
        else
        {
            userWeekByQuest.time = currentTime;
            userWeekByQuest.point = point;
        }
    }
    public int GetUserWeekByQuestPoint() // 주간퀘스트 포인트 가져옴 
    {
        DateTime currentTime = TimeManager.instance.currentTime;
        TimeSpan subTimeTimeSpan = currentTime.Subtract(userWeekByQuest.time);
        DateTime subTime = new DateTime(subTimeTimeSpan.Ticks);

        bool flag = false;
        switch (currentTime.DayOfWeek)
        {
            case DayOfWeek.Monday:
                if (subTime.Day < 1) flag = true;
                break;
            case DayOfWeek.Tuesday:
                if (subTime.Day < 2) flag = true;
                break;
            case DayOfWeek.Wednesday:
                if (subTime.Day < 3) flag = true;
                break;
            case DayOfWeek.Thursday:
                if (subTime.Day < 4) flag = true;
                break;
            case DayOfWeek.Friday:
                if (subTime.Day < 5) flag = true;
                break;
            case DayOfWeek.Saturday:
                if (subTime.Day < 6) flag = true;
                break;
            case DayOfWeek.Sunday:
                if (subTime.Day < 7) flag = true;
                break;
        }

        if (flag)
        {
            return userWeekByQuest.point;
        }
        else
        {
            return 0;
        }
    }
    public void PushUserWeekByQuestComplete(int ID) // 해당 주간 퀘스트 완료시키기
    {
        if (userWeekByQuest.complete.ContainsKey(ID))
        {
            userWeekByQuest.complete[ID] = true;
        }
        else
        {
            userWeekByQuest.complete.Add(ID, true);
        }
    }
    public bool GetUserWeekByQuestComplete(int ID) // 해당 주간 퀘스트 완료정보 가져오기
    {
        DateTime currentTime = TimeManager.instance.currentTime;
        TimeSpan subTimeTimeSpan = currentTime.Subtract(userWeekByQuest.time);
        DateTime subTime = new DateTime(subTimeTimeSpan.Ticks);

        bool flag = false;
        switch (currentTime.DayOfWeek)
        {
            case DayOfWeek.Monday:
                if (subTime.Day < 1) flag = true;
                break;
            case DayOfWeek.Tuesday:
                if (subTime.Day < 2) flag = true;
                break;
            case DayOfWeek.Wednesday:
                if (subTime.Day < 3) flag = true;
                break;
            case DayOfWeek.Thursday:
                if (subTime.Day < 4) flag = true;
                break;
            case DayOfWeek.Friday:
                if (subTime.Day < 5) flag = true;
                break;
            case DayOfWeek.Saturday:
                if (subTime.Day < 6) flag = true;
                break;
            case DayOfWeek.Sunday:
                if (subTime.Day < 7) flag = true;
                break;
        }

        if (flag)
        {
          
        }
        else
        {
            userWeekByQuest.complete.Clear();
        }

        if (userWeekByQuest.complete.ContainsKey(ID))
        {
            return userWeekByQuest.complete[ID];
        }
        else
        {
            userWeekByQuest.complete.Add(ID, false);
            return false;
        }
    }
    public void SaveUserWeekByQuest(System.Action callback)
    {
        Param param = new Param();
        string point = userWeekByQuest.point.ToString();
        string complete = "";
        string time = userWeekByQuest.time.ToString();

        foreach (KeyValuePair<int, bool> dict in userWeekByQuest.complete)
        {
            complete += dict.Key + "," + dict.Value + "=";
        }
        if(userWeekByQuest.complete.Count > 0) complete = complete.Substring(0, complete.Length - 1);

        string data = point + "/" + complete + "/" + time;

        param.Add("Week", data);
        BackendGameInfo.instance.PrivateTableUpdate("Quest", param, () => { callback(); });
    }
    public void LoadUserWeekByQuest(System.Action callback)
    {
        BackendGameInfo.instance.GetPrivateContents("Quest", "Week", () => {
            string[] data = BackendGameInfo.instance.serverDataList[0].Split('/');
            int point = int.Parse(data[0]);
            DateTime time = DateTime.Parse(data[2]);
            Dictionary<int, bool> complete = new Dictionary<int, bool>();
            string[] data1List = data[1].Split('=');
            if (data1List.Length > 1)
            {
                for (int i = 0; i < data1List.Length; i++)
                {
                    int key = int.Parse(data1List[i].Split(',')[0]);
                    bool value = bool.Parse(data1List[i].Split(',')[1]);
                    complete.Add(key, value);
                }
            }
            userWeekByQuest = new UserWeekByQuest(point, complete, time);
            callback();
        }, () => { callback(); });
    }
    // 3. 패스
    public UserPass userPass = new UserPass();
    public void PushUserPassPoint(int point)
    {
        DateTime currentTime = TimeManager.instance.currentTime;
        DateTime passTime = userPass.time;
        if (currentTime.Year == passTime.Year && currentTime.Month == passTime.Month)
        {
            userPass.point += point;
        }
        else
        {
            userPass.time = currentTime;
            userPass.point = point;
        }
    } // 패스 포인트 적립
    public int GetUserPassPoint()
    {
        DateTime currentTime = TimeManager.instance.currentTime;
        DateTime passTime = userPass.time;
        if (currentTime.Year == passTime.Year && currentTime.Month == passTime.Month)
        {
            return userPass.point;
        }
        else
        {
            return 0;
        }
    } // 패스 포인트 가져오기
    public void PushUserPassNomalComplete(int ID) // 해당 ID의 패스 노말 완료 
    {
        if (userPass.nomalComplete.ContainsKey(ID))
        {
            userPass.nomalComplete[ID] = true;
        }
        else
        {
            userPass.nomalComplete.Add(ID, true);
        }
    }
    public void PushUserPassPassComplete(int ID) // 해당 ID의 패스 패스 완료
    {
        if (userPass.passComplete.ContainsKey(ID))
        {
            userPass.passComplete[ID] = true;
        }
        else
        {
            userPass.passComplete.Add(ID, true);
        }
    }
    public bool GetUserPassNomalComplete(int ID) // 해당 패스 노멀완료정보 가져오기
    {
        DateTime currentTime = TimeManager.instance.currentTime;
        DateTime passTime = userPass.time;
        if (currentTime.Year == passTime.Year && currentTime.Month == passTime.Month)
        {
            
        }
        else
        {
            userPass.nomalComplete.Clear();
        }

        if (userPass.nomalComplete.ContainsKey(ID))
        {
            return userPass.nomalComplete[ID];
        }
        else
        {
            userPass.nomalComplete.Add(ID, false);
            return false;
        }
    }
    public bool GetUserPassPassComplete(int ID) // 해당 패스 패스완료정보 가져오기
    {
        DateTime currentTime = TimeManager.instance.currentTime;
        DateTime passTime = userPass.time;
        if (currentTime.Year == passTime.Year && currentTime.Month == passTime.Month)
        {

        }
        else
        {
            userPass.passComplete.Clear();
        }

        if (userPass.passComplete.ContainsKey(ID))
        {
            return userPass.passComplete[ID];
        }
        else
        {
            userPass.passComplete.Add(ID, false);
            return false;
        }
    }
    public void SaveUserPass(System.Action callback)
    {
        Param param = new Param();
        string point = userPass.point.ToString();
        string nomalComplete = "";
        string passComplete = "";
        string time = userPass.time.ToString();

        foreach (KeyValuePair<int, bool> dict in userPass.nomalComplete)
        {
            nomalComplete += dict.Key + "," + dict.Value + "=";
        }
        if (userPass.nomalComplete.Count > 0) nomalComplete = nomalComplete.Substring(0, nomalComplete.Length - 1);
        foreach (KeyValuePair<int, bool> dict in userPass.passComplete)
        {
            passComplete += dict.Key + "," + dict.Value + "=";
        }
        if (userPass.passComplete.Count > 0) passComplete = passComplete.Substring(0, passComplete.Length - 1);

        string data = point + "/" + nomalComplete + "/" + passComplete + "/" + time;

        param.Add("Pass", data);
        BackendGameInfo.instance.PrivateTableUpdate("Quest", param, () => { callback(); });
    }
    public void LoadUserPass(System.Action callback)
    {
        BackendGameInfo.instance.GetPrivateContents("Quest", "Pass", () => {
            string[] data = BackendGameInfo.instance.serverDataList[0].Split('/');
            int point = int.Parse(data[0]);
            DateTime time = DateTime.Parse(data[3]);

            Dictionary<int, bool> nomalComplete = new Dictionary<int, bool>();
            string[] data1List = data[1].Split('=');
            if (data1List.Length > 1)
            {
                for (int i = 0; i < data1List.Length; i++)
                {
                    int key = int.Parse(data1List[i].Split(',')[0]);
                    bool value = bool.Parse(data1List[i].Split(',')[1]);
                    nomalComplete.Add(key, value);
                }
            }

            Dictionary<int, bool> passComplete = new Dictionary<int, bool>();
            string[] data2List = data[2].Split('=');
            if (data2List.Length > 1)
            {
                for (int i = 0; i < data2List.Length; i++)
                {
                    int key = int.Parse(data2List[i].Split(',')[0]);
                    bool value = bool.Parse(data2List[i].Split(',')[1]);
                    passComplete.Add(key, value);
                }
            }

            userPass = new UserPass(point, nomalComplete, passComplete, time);
            callback();
        }, () => { callback(); });
    }

    /// 코인트리
    public int coinTreeLevel;
    public void CoinTreeLevelUp()
    {
        int maxLevel = CoinTreeChart.instance.MaxLevel();
        coinTreeLevel++;
        if (coinTreeLevel >= maxLevel)
        {
            coinTreeLevel = maxLevel;
        }
    }
    public void SaveCoinTree(System.Action callback)
    {
        Param param = new Param();
        param.Add("Level", coinTreeLevel);
        BackendGameInfo.instance.PrivateTableUpdate("CoinTree", param, () => { callback(); });
    }
    public void LoadCoinTree(System.Action callback)
    {
        BackendGameInfo.instance.GetPrivateContents("CoinTree", "Level", () => {
            coinTreeLevel = int.Parse(BackendGameInfo.instance.serverDataList[0]);
            callback();
        }, () => {
            coinTreeLevel = 1;
            callback(); });
    }

    // 바이올렛 남은 카운트 
    public UserViolet userViolet = null;
    public int GetVioletRemainCount()
    {
        DateTime currentTime = TimeManager.instance.currentTime;
        int maxCount = 3;
        if(userViolet == null) // 아무런 정보가 없을경우 풀충 
        {
            userViolet = new UserViolet(currentTime, maxCount);
        }

        // 년 월 일 이 같다면 남은 카운트 리턴
        if (userViolet.dateTime.Year == currentTime.Year && userViolet.dateTime.Month == currentTime.Month && userViolet.dateTime.Day == currentTime.Day)
        {
            return userViolet.violetRemainCount;
        }
        else // 같지 않다면 풀충하고 풀 카운트 리턴 
        {
            userViolet = new UserViolet(currentTime, maxCount);
            return maxCount;
        }
    }    // 바이올렛 카운트 가져오기
    public void PullVioletRemainCount() // 바이올렛 카운트 소비하기 
    {
        DateTime currentTime = TimeManager.instance.currentTime;
        userViolet.violetRemainCount--;
        userViolet.dateTime = currentTime;
    }
    public void PushVioletRemainCount() // 바이올렛 카운트 충전하기 
    {
        DateTime currentTime = TimeManager.instance.currentTime;
        userViolet.violetRemainCount++;
        userViolet.dateTime = currentTime;
    }
    public void SaveUserViolet(System.Action callback)
    {
        if (userViolet == null)
        {
            callback();
            return;
        }

        Param param = new Param();
        string data = userViolet.dateTime + "=" + userViolet.violetRemainCount;
        param.Add("RemainCount", data);
        BackendGameInfo.instance.PrivateTableUpdate("Violet", param, () => { callback(); });
    }
    public void LoadUserViolet(System.Action callback)
    {
        BackendGameInfo.instance.GetPrivateContents("Violet", "RemainCount", () => {
            string[] dataList = BackendGameInfo.instance.serverDataList[0].Split('=');
            DateTime dateTime = DateTime.Parse(dataList[0]);
            int remainCount = int.Parse(dataList[1]);
            userViolet = new UserViolet(dateTime, remainCount);
            callback();
        }, () => {
            callback();
        });
    }
}
public class UserPublicInfo
{
    public string nickname;
    public List<UserCloth> userCloths = new List<UserCloth>(); 

    public UserPublicInfo(string nickname, List<UserCloth> userCloths)
    {
        this.nickname = nickname;
        this.userCloths = userCloths;
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
public class UserRelic
{
    public string name;
    public GradeType gradeType;
    public int upgrade;
    public int num;

    public UserRelic(string name, GradeType gradeType, int upgrade, int num)
    {
        this.name = name;
        this.gradeType = gradeType;
        this.upgrade = upgrade;
        this.num = num;
    }
}
public class UserCloth
{
    public string name;
    public int upgrade;
    public int num;
    public bool isEqip;

    public UserCloth(string name, int upgrade, int num, bool isEqip)
    {
        this.name = name;
        this.upgrade = upgrade;
        this.num = num;
        this.isEqip = isEqip;
    }
}
public class UserNomalMonster
{
    public string name;
    public int count;
    public DateTime clearDateTime;

    public UserNomalMonster(string name, int count, DateTime dateTime)
    {
        this.name = name;
        this.count = count;
        this.clearDateTime = dateTime;
    }
}
public class UserDayByQuest
{
    public int ID;
    public DateTime time;
    public int count;
    public bool complete;

    public UserDayByQuest(int ID, DateTime time, int count, bool complete)
    {
        this.ID = ID;
        this.time = time;
        this.count = count;
        this.complete = complete;
    }
}
public class UserWeekByQuest
{
    public int point;
    public Dictionary<int, bool> complete = new Dictionary<int, bool>();
    public DateTime time = new DateTime();

    public UserWeekByQuest()
    {
        point = 0;
        complete.Clear();
    }

    public UserWeekByQuest(int point, Dictionary<int,bool> complete, DateTime time)
    {
        this.point = point;
        this.complete = complete;
        this.time = time;
    }
}
public class UserPass
{
    public int point;
    public Dictionary<int, bool> nomalComplete = new Dictionary<int, bool>();
    public Dictionary<int, bool> passComplete = new Dictionary<int, bool>();
    public DateTime time = new DateTime();

    public UserPass()
    {
        point = 0;
        nomalComplete.Clear();
        passComplete.Clear();
    }

    public UserPass(int point, Dictionary<int, bool> nomalComplete, Dictionary<int, bool> passComplete, DateTime time)
    {
        this.point = point;
        this.nomalComplete = nomalComplete;
        this.passComplete = passComplete;
        this.time = time;
    }
}
public class UserViolet
{
    public DateTime dateTime;
    public int violetRemainCount; // 일일 입장횟수 남은 카운트 

    public UserViolet(DateTime dateTime, int violetRemainCount)
    {
        this.dateTime = dateTime;
        this.violetRemainCount = violetRemainCount;
    }
}