using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class RelicManager : MonoBehaviour
{
    /// <summary>
    ///  모든 타입 
    /// </summary>
    
    public GameObject card;
    public Transform content;
    public Text totalAbilityText;
    public Transform totalRelic;

    [Header("빽 이미지")]
    public Sprite aBgBackSprite;
    public Sprite bBgBackSprite;

    [Header("아틀라스")]
    public SpriteAtlas gradeSpriteAtlas;

    [Header("스크립트")]
    public RelicStarEnhanceResult relicStarEnhanceResult;

    private void Start()
    {
        Instantiated();
    }
    void Instantiated() // 카드생성
    {
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        int len = RelicChart.instance.RelicType().Count;
        for (int i = 0; i < len; i++)
        {
            Instantiate(card, Vector2.zero, Quaternion.identity, content);
        }
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, 207 * content.childCount + 30);
    }

    public void Open()
    {
        UISetting();
    }
    void UISetting() // 카드에 정보 넣기 
    {
        // 전체 능력치 텍스트 
        Dictionary<AbilityType, float> userReilicDic = new Dictionary<AbilityType, float>();
        for (int i = 0; i < UserInfo.instance.userRelics.Count; i++)
        {
            AbilityType enhanceAbilityType = RelicChart.instance.GetRelicChartInfo(UserInfo.instance.userRelics[i].name)[(int)UserInfo.instance.userRelics[i].gradeType - 1].EnhanceAbilityType;
            float enhanceAbilityIncrease = RelicChart.instance.GetRelicChartInfo(UserInfo.instance.userRelics[i].name)[(int)UserInfo.instance.userRelics[i].gradeType - 1].EnhanceAbilityIncrease;
            if (userReilicDic.ContainsKey(enhanceAbilityType))
            {
                userReilicDic[enhanceAbilityType] += (UserInfo.instance.userRelics[i].upgrade + 1) * enhanceAbilityIncrease;
            }
            else
            {
                userReilicDic.Add(enhanceAbilityType, (UserInfo.instance.userRelics[i].upgrade + 1) * enhanceAbilityIncrease);
            }

            AbilityType StarEnhanceAbilityType = RelicChart.instance.GetRelicChartInfo(UserInfo.instance.userRelics[i].name)[(int)UserInfo.instance.userRelics[i].gradeType - 1].StarEnhanceAbilityType;
            float StarEnhanceAbilityIncrease = RelicChart.instance.GetRelicChartInfo(UserInfo.instance.userRelics[i].name)[(int)UserInfo.instance.userRelics[i].gradeType - 1].StarEnhanceAbilityIncrease;
            if (userReilicDic.ContainsKey(StarEnhanceAbilityType))
            {
                userReilicDic[StarEnhanceAbilityType] += StarEnhanceAbilityIncrease;
            }
            else
            {
                userReilicDic.Add(StarEnhanceAbilityType, StarEnhanceAbilityIncrease);
            }
        }
        totalAbilityText.text = "";
        foreach(AbilityType key in System.Enum.GetValues(typeof(AbilityType)))
        {
            if (userReilicDic.ContainsKey(key))
            {
                totalAbilityText.text += key.ToString() + " : " + userReilicDic[key] + "% ";
            }
        }

        // 총 보유 수 
        int totalRelicLen = RelicChart.instance.RelicType().Count;
        int userRelicLen = UserInfo.instance.userRelics.Count;
        totalRelic.Find("Text").GetComponent<Text>().text = "총보유 수량 " + userRelicLen + "/" + totalRelicLen;
        totalRelic.Find("fore").GetComponent<Image>().fillAmount = userRelicLen / (float)totalRelicLen;

        // 카드 
        List<string> relicType = RelicChart.instance.RelicType();
        for (int i = 0; i < relicType.Count; i++)
        {
            Transform card = content.GetChild(i);
            UserRelic userRelic = UserInfo.instance.GetUserRelicInfo(relicType[i]);

            if (userRelic != null)
            {
                GradeType gradeType = RelicChart.instance.GetRelicChartInfo(relicType[i])[(int)userRelic.gradeType - 1].GradeType;
                string clearBossName = RelicChart.instance.GetRelicChartInfo(relicType[i])[(int)userRelic.gradeType - 1].ClearBossName;
                Sprite image = RelicChart.instance.GetRelicChartInfo(relicType[i])[(int)userRelic.gradeType - 1].Image;
                AbilityType enhanceAbilityType = RelicChart.instance.GetRelicChartInfo(relicType[i])[(int)userRelic.gradeType - 1].EnhanceAbilityType;
                float enhanceAbilityIncrease = RelicChart.instance.GetRelicChartInfo(relicType[i])[(int)userRelic.gradeType - 1].EnhanceAbilityIncrease;
                int enhance_M_Default = RelicChart.instance.GetRelicChartInfo(relicType[i])[(int)userRelic.gradeType - 1].Enhance_M_Default;
                int enhance_M_Increase = RelicChart.instance.GetRelicChartInfo(relicType[i])[(int)userRelic.gradeType - 1].Enhance_M_Increase;
                AbilityType StarEnhanceAbilityType = RelicChart.instance.GetRelicChartInfo(relicType[i])[(int)userRelic.gradeType - 1].StarEnhanceAbilityType;
                float StarEnhanceAbilityIncrease = RelicChart.instance.GetRelicChartInfo(relicType[i])[(int)userRelic.gradeType - 1].StarEnhanceAbilityIncrease;
                int CombinationNum = RelicChart.instance.GetRelicChartInfo(relicType[i])[(int)userRelic.gradeType - 1].CombinationNum;

                card.Find("back").GetComponent<Image>().sprite = aBgBackSprite;

                card.Find("IconFrame").Find("Icon").GetComponent<Image>().sprite = image;
                card.Find("IconFrame").Find("Icon").GetComponent<Image>().color = Color.white;
                card.Find("IconFrame").Find("GradeTypeImg").GetComponent<Image>().sprite = gradeSpriteAtlas.GetSprite(gradeType.ToString());
                card.Find("IconFrame").Find("GradeTypeImg").GetComponent<Image>().SetNativeSize();

                card.Find("이름레벨").GetComponent<Text>().text = relicType[i] + " Lv. " + userRelic.upgrade;

                card.Find("능력치").GetComponent<Text>().text = "<color=#5DFF2F>" + enhanceAbilityType.ToString() + " " + (userRelic.upgrade + 1) * enhanceAbilityIncrease + "%</color>\n" +
                                                                "<color=#FF9200>" + StarEnhanceAbilityType.ToString() + " " + StarEnhanceAbilityIncrease + "%</color>";

                int money = enhance_M_Default + enhance_M_Increase * userRelic.upgrade;
                card.Find("강화").Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
                card.Find("강화").Find("Num").GetComponent<Text>().text = money.ToString();
                card.Find("강화").Find("RedIcon").gameObject.SetActive(UserInfo.instance.enhanceStone >= money);
                card.Find("강화").Find("강화버튼").GetComponent<Button>().onClick.AddListener(() => {
                    if (UserInfo.instance.enhanceStone >= money)
                    {
                        MoneyManager.instance.EnhanceStoneSub(money);
                        UserInfo.instance.UpgradeRelic(userRelic.name);
                        UserInfo.instance.SaveMoney(() => { UserInfo.instance.SaveRelic(() => { }); });
                        UISetting();
                    }
                });

                card.Find("성급").Find("성급버튼").GetComponent<Button>().onClick.RemoveAllListeners();
                card.Find("성급").Find("fore").GetComponent<Image>().fillAmount = userRelic.num / (float)CombinationNum;
                card.Find("성급").Find("RedIcon").gameObject.SetActive(userRelic.num / (float)CombinationNum >= 1 && (int)userRelic.gradeType < System.Enum.GetValues(typeof(GradeType)).Length - 1);
                card.Find("성급").Find("소지개수").GetComponent<Text>().text = userRelic.num + "/" + CombinationNum;
                card.Find("성급").Find("성급버튼").GetComponent<Button>().onClick.AddListener(() => {
                    if (userRelic.num / (float)CombinationNum >= 1 && (int)userRelic.gradeType < System.Enum.GetValues(typeof(GradeType)).Length - 1)
                    {
                        relicStarEnhanceResult.Open(userRelic.name);

                        UserInfo.instance.StarUpgradeRelic(userRelic.name, CombinationNum);
                        UserInfo.instance.SaveRelic(() => { });
                        UISetting();
                    }
                });
            }
            else
            {
                GradeType gradeType = RelicChart.instance.GetRelicChartInfo(relicType[i])[0].GradeType;
                string clearBossName = RelicChart.instance.GetRelicChartInfo(relicType[i])[0].ClearBossName;
                Sprite image = RelicChart.instance.GetRelicChartInfo(relicType[i])[0].Image;
                AbilityType enhanceAbilityType = RelicChart.instance.GetRelicChartInfo(relicType[i])[0].EnhanceAbilityType;
                float enhanceAbilityIncrease = RelicChart.instance.GetRelicChartInfo(relicType[i])[0].EnhanceAbilityIncrease;
                int enhance_M_Default = RelicChart.instance.GetRelicChartInfo(relicType[i])[0].Enhance_M_Default;
                int enhance_M_Increase = RelicChart.instance.GetRelicChartInfo(relicType[i])[0].Enhance_M_Increase;
                AbilityType StarEnhanceAbilityType = RelicChart.instance.GetRelicChartInfo(relicType[i])[0].StarEnhanceAbilityType;
                float StarEnhanceAbilityIncrease = RelicChart.instance.GetRelicChartInfo(relicType[i])[0].StarEnhanceAbilityIncrease;
                int CombinationNum = RelicChart.instance.GetRelicChartInfo(relicType[i])[0].CombinationNum;

                card.Find("back").GetComponent<Image>().sprite = bBgBackSprite;

                card.Find("IconFrame").Find("Icon").GetComponent<Image>().sprite = image;
                card.Find("IconFrame").Find("Icon").GetComponent<Image>().color = Color.gray;
                card.Find("IconFrame").Find("GradeTypeImg").GetComponent<Image>().sprite = gradeSpriteAtlas.GetSprite("빈" + gradeType.ToString());
                card.Find("IconFrame").Find("GradeTypeImg").GetComponent<Image>().SetNativeSize();

                card.Find("이름레벨").GetComponent<Text>().text = relicType[i];

                card.Find("능력치").GetComponent<Text>().text = enhanceAbilityType.ToString() + " " + enhanceAbilityIncrease + "%\n" +
                                                                StarEnhanceAbilityType.ToString() + " " + StarEnhanceAbilityIncrease + "%";
               
                card.Find("강화").Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
                card.Find("강화").Find("RedIcon").gameObject.SetActive(false);

                card.Find("성급").Find("성급버튼").GetComponent<Button>().onClick.RemoveAllListeners();
                card.Find("성급").Find("RedIcon").gameObject.SetActive(false);
                card.Find("성급").Find("fore").GetComponent<Image>().fillAmount = 0;
                card.Find("성급").Find("소지개수").GetComponent<Text>().text = "0/0";
            }
        }
    }

}
