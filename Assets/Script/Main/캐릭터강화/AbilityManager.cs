using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Function;
using DG.Tweening;

public class AbilityManager : MonoBehaviour
{
    [Header("Card")]
    public Transform atkCard;
    IEnumerator atkCardColorCoroutine;
    public Transform atkPercentCard;
    IEnumerator atkPercentCardColorCoroutine;
    public Transform hpCard;
    IEnumerator hpCardColorCoroutine;
    public Transform hpPercentCard;
    IEnumerator hpPercentCardColorCoroutine;
    public Transform criticalDamageCard;
    IEnumerator criticalDamageCardColorCoroutine;
    public Transform angerDamageCard;
    IEnumerator angerDamageCardColorCoroutine;
    public Transform faustDamageCard;
    IEnumerator faustDamageCardColorCoroutine;
    public Sprite btnColorSprite;
    public Sprite btnNullSprite;
    public Image frameImg;
    IEnumerator frameColorCoroutine;

    [Header("UI")]
    public Scrollbar scrollbar;
    public Transform switchCon;
    public Transform minSwitchCon;
    public Transform maxSwitchCon;
    public GameObject redIcon;

    [Header("플레이어")]
    public PlayerController playerController;

    public void Show()
    {
        Init();
    }
    public void Init()
    {
        AtkCardInit();
        AtkPercentCardInit();
        HpCardInit();
        HpPercentCardInit();
        CriticalDamageCardInit();
        AngerDamageCardInit();
        FaustDamageCardInit();
        CheckRedIcon();
    }
    void AtkCardInit()
    {
        int userLevel = UserInfo.instance.abilityAtkLevel;
        int len = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo.Length;
        if (userLevel >= len-2)
        {
            atkCard.Find("레벨").GetComponent<Text>().text = "공격력 Lv. Max";
            string currentMax = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].Atk;
            atkCard.Find("현재").GetComponent<Text>().text = MyMath.ValueToString(currentMax);
            atkCard.Find("다음").GetComponent<Text>().text = "???";
            atkCard.Find("강화버튼").Find("Text").GetComponent<Text>().text = "Max";
            atkCard.Find("강화버튼").GetComponent<Image>().sprite = btnNullSprite;
            atkCard.Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
            return;
        }

        string current = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].Atk;
        string next = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel+1].Atk;
        string needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].AtkM;
        string userGold = UserInfo.instance.gold;
        atkCard.Find("레벨").GetComponent<Text>().text = "공격력 Lv." + userLevel;
        atkCard.Find("현재").GetComponent<Text>().text = MyMath.ValueToString(current);
        atkCard.Find("다음").GetComponent<Text>().text = MyMath.ValueToString(next);
        atkCard.Find("강화버튼").Find("Text").GetComponent<Text>().text = MyMath.ValueToString(needGold);

        if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
        {
            atkCard.Find("강화버튼").GetComponent<Image>().sprite = btnColorSprite;
        }
        else
        {
            atkCard.Find("강화버튼").GetComponent<Image>().sprite = btnNullSprite;
        }

        atkCard.Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
        atkCard.Find("강화버튼").GetComponent<Button>().onClick.AddListener(() => {
            needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].AtkM;
            userGold = UserInfo.instance.gold;
            if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
            {
                MoneyManager.instance.GoldSub(needGold);
                UserInfo.instance.abilityAtkLevel++;
                if (atkCardColorCoroutine != null) StopCoroutine(atkCardColorCoroutine);
                atkCardColorCoroutine = ImageColorChangeCoroutine(atkCard.GetComponent<Image>(), Color.gray, Color.white, 0.08f);
                StartCoroutine(atkCardColorCoroutine);
                if (frameColorCoroutine != null) StopCoroutine(frameColorCoroutine);
                frameColorCoroutine = ImageColorChangeCoroutine(frameImg, new Color(1,1,1,0), Color.white, 0.16f);
                StartCoroutine(frameColorCoroutine);
                Init();
                UserInfo.instance.SaveAbility(()=> {
                    UserInfo.instance.SaveMoney(() => { });
                });
            }
        });
    }
    void AtkPercentCardInit()
    {
        int userLevel = UserInfo.instance.abilityAtkPercentLevel;
        int len = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo.Length;
        if (userLevel >= len - 2)
        {
            atkPercentCard.Find("레벨").GetComponent<Text>().text = "공격력% Lv. Max";
            float currentMax = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].AtkPercent;
            atkPercentCard.Find("현재").GetComponent<Text>().text = currentMax + "%";
            atkPercentCard.Find("다음").GetComponent<Text>().text = "???";
            atkPercentCard.Find("강화버튼").Find("Text").GetComponent<Text>().text = "Max";
            atkPercentCard.Find("강화버튼").GetComponent<Image>().sprite = btnNullSprite;
            atkPercentCard.Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
            return;
        }

        float current = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].AtkPercent;
        float next = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel + 1].AtkPercent;
        string needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].AtkPercentM;
        string userGold = UserInfo.instance.gold;
        atkPercentCard.Find("레벨").GetComponent<Text>().text = "공격력% Lv." + userLevel;
        atkPercentCard.Find("현재").GetComponent<Text>().text = current + "%";
        atkPercentCard.Find("다음").GetComponent<Text>().text = next + "%";
        atkPercentCard.Find("강화버튼").Find("Text").GetComponent<Text>().text = MyMath.ValueToString(needGold);

        if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
        {
            atkPercentCard.Find("강화버튼").GetComponent<Image>().sprite = btnColorSprite;
        }
        else
        {
            atkPercentCard.Find("강화버튼").GetComponent<Image>().sprite = btnNullSprite;
        }
        atkPercentCard.Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
        atkPercentCard.Find("강화버튼").GetComponent<Button>().onClick.AddListener(() => {
            needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel + 1].AtkPercentM;
            userGold = UserInfo.instance.gold;
            if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
            {
                MoneyManager.instance.GoldSub(needGold);
                UserInfo.instance.abilityAtkPercentLevel++;
                if (atkPercentCardColorCoroutine != null) StopCoroutine(atkPercentCardColorCoroutine);
                atkPercentCardColorCoroutine = ImageColorChangeCoroutine(atkPercentCard.GetComponent<Image>(), Color.gray, Color.white, 0.08f);
                StartCoroutine(atkPercentCardColorCoroutine);
                if (frameColorCoroutine != null) StopCoroutine(frameColorCoroutine);
                frameColorCoroutine = ImageColorChangeCoroutine(frameImg, new Color(1, 1, 1, 0), Color.white, 0.16f);
                StartCoroutine(frameColorCoroutine);
                Init();
                UserInfo.instance.SaveAbility(() => {
                    UserInfo.instance.SaveMoney(() => { });
                });
            }
        });
    }
    void HpCardInit()
    {
        int userLevel = UserInfo.instance.abilityHpLevel;
        int len = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo.Length;
        if (userLevel >= len - 2)
        {
            hpCard.Find("레벨").GetComponent<Text>().text = "체력 Lv. Max";
            string currentMax = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].Hp;
            hpCard.Find("현재").GetComponent<Text>().text = MyMath.ValueToString(currentMax);
            hpCard.Find("다음").GetComponent<Text>().text = "???";
            hpCard.Find("강화버튼").Find("Text").GetComponent<Text>().text = "Max";
            hpCard.Find("강화버튼").GetComponent<Image>().sprite = btnNullSprite;
            hpCard.Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
            return;
        }

        string current = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].Hp;
        string next = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel + 1].Hp;
        string needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].HpM;
        string userGold = UserInfo.instance.gold;
        hpCard.Find("레벨").GetComponent<Text>().text = "체력 Lv." + userLevel;
        hpCard.Find("현재").GetComponent<Text>().text = MyMath.ValueToString(current);
        hpCard.Find("다음").GetComponent<Text>().text = MyMath.ValueToString(next);
        hpCard.Find("강화버튼").Find("Text").GetComponent<Text>().text = MyMath.ValueToString(needGold);

        if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
        {
            hpCard.Find("강화버튼").GetComponent<Image>().sprite = btnColorSprite;
        }
        else
        {
            hpCard.Find("강화버튼").GetComponent<Image>().sprite = btnNullSprite;
        }
        hpCard.Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
        hpCard.Find("강화버튼").GetComponent<Button>().onClick.AddListener(() => {
            needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel + 1].HpM;
            userGold = UserInfo.instance.gold;
            if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
            {
                MoneyManager.instance.GoldSub(needGold);
                UserInfo.instance.abilityHpLevel++;
                if (hpCardColorCoroutine != null) StopCoroutine(hpCardColorCoroutine);
                hpCardColorCoroutine = ImageColorChangeCoroutine(hpCard.GetComponent<Image>(), Color.gray, Color.white, 0.08f);
                StartCoroutine(hpCardColorCoroutine);
                if (frameColorCoroutine != null) StopCoroutine(frameColorCoroutine);
                frameColorCoroutine = ImageColorChangeCoroutine(frameImg, new Color(1, 1, 1, 0), Color.white, 0.16f);
                StartCoroutine(frameColorCoroutine);
                Init();
                UserInfo.instance.SaveAbility(() => {
                    UserInfo.instance.SaveMoney(() => { });
                });

                playerController.Hp_Initialized();
                playerController.Hp_UI_Setting();
            }
        });
    }
    void HpPercentCardInit()
    {
        int userLevel = UserInfo.instance.abilityHpPercentLevel;
        int len = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo.Length;
        if (userLevel >= len - 2)
        {
            hpPercentCard.Find("레벨").GetComponent<Text>().text = "체력% Lv. Max";
            float currentMax = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].HpPercent;
            hpPercentCard.Find("현재").GetComponent<Text>().text = currentMax + "%";
            hpPercentCard.Find("다음").GetComponent<Text>().text = "???";
            hpPercentCard.Find("강화버튼").Find("Text").GetComponent<Text>().text = "Max";
            hpPercentCard.Find("강화버튼").GetComponent<Image>().sprite = btnNullSprite;
            hpPercentCard.Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
            return;
        }

        float current = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].HpPercent;
        float next = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel + 1].HpPercent;
        string needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].HpPercentM;
        string userGold = UserInfo.instance.gold;
        hpPercentCard.Find("레벨").GetComponent<Text>().text = "체력% Lv." + userLevel;
        hpPercentCard.Find("현재").GetComponent<Text>().text = current + "%";
        hpPercentCard.Find("다음").GetComponent<Text>().text = next + "%";
        hpPercentCard.Find("강화버튼").Find("Text").GetComponent<Text>().text = MyMath.ValueToString(needGold);

        if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
        {
            hpPercentCard.Find("강화버튼").GetComponent<Image>().sprite = btnColorSprite;
        }
        else
        {
            hpPercentCard.Find("강화버튼").GetComponent<Image>().sprite = btnNullSprite;
        }
        hpPercentCard.Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
        hpPercentCard.Find("강화버튼").GetComponent<Button>().onClick.AddListener(() => {
            needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel + 1].HpPercentM;
            userGold = UserInfo.instance.gold;
            if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
            {
                MoneyManager.instance.GoldSub(needGold);
                UserInfo.instance.abilityHpPercentLevel++;
                if (hpPercentCardColorCoroutine != null) StopCoroutine(hpPercentCardColorCoroutine);
                hpPercentCardColorCoroutine = ImageColorChangeCoroutine(hpPercentCard.GetComponent<Image>(), Color.gray, Color.white, 0.08f);
                StartCoroutine(hpPercentCardColorCoroutine);
                if (frameColorCoroutine != null) StopCoroutine(frameColorCoroutine);
                frameColorCoroutine = ImageColorChangeCoroutine(frameImg, new Color(1, 1, 1, 0), Color.white, 0.16f);
                StartCoroutine(frameColorCoroutine);
                Init();
                UserInfo.instance.SaveAbility(() => {
                    UserInfo.instance.SaveMoney(() => { });
                });

                playerController.Hp_Initialized();
                playerController.Hp_UI_Setting();
            }
        });
    }
    void CriticalDamageCardInit()
    {
        int userLevel = UserInfo.instance.abilityCriticalDamageLevel;
        int len = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo.Length;
        if (userLevel >= len - 2)
        {
            criticalDamageCard.Find("레벨").GetComponent<Text>().text = "크리티컬데미지 Lv. Max";
            float currentMax = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].CriticalDamage;
            criticalDamageCard.Find("현재").GetComponent<Text>().text = currentMax + "%";
            criticalDamageCard.Find("다음").GetComponent<Text>().text = "???";
            criticalDamageCard.Find("강화버튼").Find("Text").GetComponent<Text>().text = "Max";
            criticalDamageCard.Find("강화버튼").GetComponent<Image>().sprite = btnNullSprite;
            criticalDamageCard.Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
            return;
        }

        float current = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].CriticalDamage;
        float next = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel + 1].CriticalDamage;
        string needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].CriticalDamageM;
        string userGold = UserInfo.instance.gold;
        criticalDamageCard.Find("레벨").GetComponent<Text>().text = "크리티컬데미지 Lv." + userLevel;
        criticalDamageCard.Find("현재").GetComponent<Text>().text = current + "%";
        criticalDamageCard.Find("다음").GetComponent<Text>().text = next + "%";
        criticalDamageCard.Find("강화버튼").Find("Text").GetComponent<Text>().text = MyMath.ValueToString(needGold);

        if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
        {
            criticalDamageCard.Find("강화버튼").GetComponent<Image>().sprite = btnColorSprite;
        }
        else
        {
            criticalDamageCard.Find("강화버튼").GetComponent<Image>().sprite = btnNullSprite;
        }
        criticalDamageCard.Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
        criticalDamageCard.Find("강화버튼").GetComponent<Button>().onClick.AddListener(() => {
            needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel + 1].CriticalDamageM;
            userGold = UserInfo.instance.gold;
            if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
            {
                MoneyManager.instance.GoldSub(needGold);
                UserInfo.instance.abilityCriticalDamageLevel++;
                if (criticalDamageCardColorCoroutine != null) StopCoroutine(criticalDamageCardColorCoroutine);
                criticalDamageCardColorCoroutine = ImageColorChangeCoroutine(criticalDamageCard.GetComponent<Image>(), Color.gray, Color.white, 0.08f);
                StartCoroutine(criticalDamageCardColorCoroutine);
                if (frameColorCoroutine != null) StopCoroutine(frameColorCoroutine);
                frameColorCoroutine = ImageColorChangeCoroutine(frameImg, new Color(1, 1, 1, 0), Color.white, 0.16f);
                StartCoroutine(frameColorCoroutine);
                Init();
                UserInfo.instance.SaveAbility(() => {
                    UserInfo.instance.SaveMoney(() => { });
                });
            }
        });
    }
    void AngerDamageCardInit()
    {
        int userLevel = UserInfo.instance.abilityAngerDamageLevel;
        int len = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo.Length;
        if (userLevel >= len - 2)
        {
            angerDamageCard.Find("레벨").GetComponent<Text>().text = "분노데미지 Lv. Max";
            float currentMax = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].AngerDamage;
            angerDamageCard.Find("현재").GetComponent<Text>().text = currentMax + "%";
            angerDamageCard.Find("다음").GetComponent<Text>().text = "???";
            angerDamageCard.Find("강화버튼").Find("Text").GetComponent<Text>().text = "Max";
            angerDamageCard.Find("강화버튼").GetComponent<Image>().sprite = btnNullSprite;
            angerDamageCard.Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
            return;
        }

        float current = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].AngerDamage;
        float next = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel + 1].AngerDamage;
        string needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].AngerDamageM;
        string userGold = UserInfo.instance.gold;
        angerDamageCard.Find("레벨").GetComponent<Text>().text = "분노데미지 Lv." + userLevel;
        angerDamageCard.Find("현재").GetComponent<Text>().text = current + "%";
        angerDamageCard.Find("다음").GetComponent<Text>().text = next + "%";
        angerDamageCard.Find("강화버튼").Find("Text").GetComponent<Text>().text = MyMath.ValueToString(needGold);

        if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
        {
            angerDamageCard.Find("강화버튼").GetComponent<Image>().sprite = btnColorSprite;
        }
        else
        {
            angerDamageCard.Find("강화버튼").GetComponent<Image>().sprite = btnNullSprite;
        }
        angerDamageCard.Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
        angerDamageCard.Find("강화버튼").GetComponent<Button>().onClick.AddListener(() => {
            needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel + 1].AngerDamageM;
            userGold = UserInfo.instance.gold;
            if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
            {
                MoneyManager.instance.GoldSub(needGold);
                UserInfo.instance.abilityAngerDamageLevel++;
                if (angerDamageCardColorCoroutine != null) StopCoroutine(angerDamageCardColorCoroutine);
                angerDamageCardColorCoroutine = ImageColorChangeCoroutine(angerDamageCard.GetComponent<Image>(), Color.gray, Color.white, 0.08f);
                StartCoroutine(angerDamageCardColorCoroutine);
                if (frameColorCoroutine != null) StopCoroutine(frameColorCoroutine);
                frameColorCoroutine = ImageColorChangeCoroutine(frameImg, new Color(1, 1, 1, 0), Color.white, 0.16f);
                StartCoroutine(frameColorCoroutine);
                Init();
                UserInfo.instance.SaveAbility(() => {
                    UserInfo.instance.SaveMoney(() => { });
                });
            }
        });
    }
    void FaustDamageCardInit()
    {
        int userLevel = UserInfo.instance.abilityFaustDamageLevel;
        int len = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo.Length;
        if (userLevel >= len - 2)
        {
            faustDamageCard.Find("레벨").GetComponent<Text>().text = "파우스트데미지 Lv. Max";
            float currentMax = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].FaustDamage;
            faustDamageCard.Find("현재").GetComponent<Text>().text = currentMax + "%";
            faustDamageCard.Find("다음").GetComponent<Text>().text = "???";
            faustDamageCard.Find("강화버튼").Find("Text").GetComponent<Text>().text = "Max";
            faustDamageCard.Find("강화버튼").GetComponent<Image>().sprite = btnNullSprite;
            faustDamageCard.Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
            return;

        }
        float current = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].FaustDamage;
        float next = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel + 1].FaustDamage;
        string needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].FaustDamageM;
        string userGold = UserInfo.instance.gold;
        faustDamageCard.Find("레벨").GetComponent<Text>().text = "파우스트데미지 Lv." + userLevel;
        faustDamageCard.Find("현재").GetComponent<Text>().text = current + "%";
        faustDamageCard.Find("다음").GetComponent<Text>().text = next + "%";
        faustDamageCard.Find("강화버튼").Find("Text").GetComponent<Text>().text = MyMath.ValueToString(needGold);

        if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
        {
            faustDamageCard.Find("강화버튼").GetComponent<Image>().sprite = btnColorSprite;
        }
        else
        {
            faustDamageCard.Find("강화버튼").GetComponent<Image>().sprite = btnNullSprite;
        }
        faustDamageCard.Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
        faustDamageCard.Find("강화버튼").GetComponent<Button>().onClick.AddListener(() => {
            needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel + 1].FaustDamageM;
            userGold = UserInfo.instance.gold;
            if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
            {
                MoneyManager.instance.GoldSub(needGold);
                UserInfo.instance.abilityFaustDamageLevel++;
                if (faustDamageCardColorCoroutine != null) StopCoroutine(faustDamageCardColorCoroutine);
                faustDamageCardColorCoroutine = ImageColorChangeCoroutine(faustDamageCard.GetComponent<Image>(), Color.gray, Color.white, 0.08f);
                StartCoroutine(faustDamageCardColorCoroutine);
                if (frameColorCoroutine != null) StopCoroutine(frameColorCoroutine);
                frameColorCoroutine = ImageColorChangeCoroutine(frameImg, new Color(1, 1, 1, 0), Color.white, 0.16f);
                StartCoroutine(frameColorCoroutine);
                Init();
                UserInfo.instance.SaveAbility(() => {
                    UserInfo.instance.SaveMoney(() => { });
                });
            }
        });
    }

    IEnumerator ImageColorChangeCoroutine(Image image, Color originColor, Color changeColor, float speed)
    {
        bool r = false, g = false, b = false, a = false;
        float rColor = originColor.r;
        float gColor = originColor.g;
        float bColor = originColor.b;
        float aColor = originColor.a;
        WaitForSeconds waitTime = new WaitForSeconds(0.02f);
  
        while (!r || !g || !b || !a)
        {
            if (!r)
            {
                if (rColor < changeColor.r)
                {
                    rColor += speed;
                    if (changeColor.r - rColor <= speed) r = true;
                }
                else
                {
                    rColor -= speed;
                    if (rColor - changeColor.r <= speed) r = true;
                }
            }

            if (!g)
            {
                if (gColor < changeColor.g)
                {
                    gColor += speed;
                    if (changeColor.g - gColor <= speed) g = true;
                }
                else
                {
                    gColor -= speed;
                    if (gColor - changeColor.g <= speed) g = true;
                }
            }

            if (!b)
            {
                if (bColor < changeColor.b)
                {
                    bColor += speed;
                    if (changeColor.b - bColor <= speed) b = true;
                }
                else
                {
                    bColor -= speed;
                    if (bColor - changeColor.b <= speed) b = true;
                }
            }

            if (!a)
            {
                if (aColor < changeColor.a)
                {
                    aColor += speed;
                    if (changeColor.a - aColor <= speed) a = true;
                }
                else
                {
                    aColor -= speed;
                    if (aColor - changeColor.a <= speed) a = true;
                }
            }

            image.color = new Color(rColor, gColor, bColor, aColor);
            yield return waitTime;
        }
        while (r || g || b || a)
        {
            if (r)
            {
                if (rColor < originColor.r)
                {
                    rColor += speed;
                    if (originColor.r - rColor <= speed) r = false;
                }
                else
                {
                    rColor -= speed;
                    if (rColor - originColor.r <= speed) r = false;
                }
            }

            if (g)
            {
                if (gColor < originColor.g)
                {
                    gColor += speed;
                    if (originColor.g - gColor <= speed) g = false;
                }
                else
                {
                    gColor -= speed;
                    if (gColor - originColor.g <= speed) g = false;
                }
            }

            if (b)
            {
                if (bColor < originColor.b)
                {
                    bColor += speed;
                    if (originColor.b - bColor <= speed) b = false;
                }
                else
                {
                    bColor -= speed;
                    if (bColor - originColor.b <= speed) b = false;
                }
            }

            if (a)
            {
                if (aColor < originColor.a)
                {
                    aColor += speed;
                    if (originColor.a - aColor <= speed) a = false;
                }
                else
                {
                    aColor -= speed;
                    if (aColor - originColor.a <= speed) a = false;
                }
            }

            image.color = new Color(rColor, gColor, bColor, aColor);
            yield return waitTime;
        }
        image.color = originColor;
    }

    private void Update()
    {
        if (scrollbar.value > 0 && scrollbar.value < 1)
        {
            float value = 1 - scrollbar.value;
            float positionY = minSwitchCon.position.y + (value * (maxSwitchCon.position.y - minSwitchCon.position.y));
            switchCon.DOMoveY(positionY, 0);
        }
    }

    public bool CheckRedIcon()
    {
        string userGold = UserInfo.instance.gold;
        // Atk
        int userLevel = UserInfo.instance.abilityAtkLevel;
        string needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].AtkM;
        if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
        {
            redIcon.SetActive(true);
            return true;
        }
        // AtkPercent
        userLevel = UserInfo.instance.abilityAtkPercentLevel;
        needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].AtkPercentM;
        if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
        {
            redIcon.SetActive(true);
            return true;
        }
        // Hp
        userLevel = UserInfo.instance.abilityHpLevel;
        needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].HpM;
        if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
        {
            redIcon.SetActive(true);
            return true;
        }
        // HpPercent
        userLevel = UserInfo.instance.abilityHpPercentLevel;
        needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].HpPercentM;
        if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
        {
            redIcon.SetActive(true);
            return true;
        }
        // CriticalDamage
        userLevel = UserInfo.instance.abilityCriticalDamageLevel;
        needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].CriticalDamageM;
        if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
        {
            redIcon.SetActive(true);
            return true;
        }
        // AngerDamag
        userLevel = UserInfo.instance.abilityAngerDamageLevel;
        needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].AngerDamageM;
        if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
        {
            redIcon.SetActive(true);
            return true;
        }
        // FaustDamage
        userLevel = UserInfo.instance.abilityFaustDamageLevel;
        needGold = CharacterEnhanceAbilityChart.instance.characterEnhanceAbilityChartInfo[userLevel].FaustDamageM;
        if (MyMath.CompareValue(userGold, needGold) != -1) // 돈이 모자라지 않음 
        {
            redIcon.SetActive(true);
            return true;
        }
        redIcon.SetActive(false);
        return false;
    }
}
