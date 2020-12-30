using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class WeaponeManager : MonoBehaviour
{
    public GameObject card;

    public Transform content;
    public Transform acquisition;
    public Transform unAcqusition;
    public Transform line;

    [Header("빽 이미지")]
    public Sprite aBtnBackSprite;
    public Sprite bBtnBackSprite;
    public Sprite aIconBackSprite;
    public Sprite bIconBackSprite;

    [Header("아틀라스")]
    public SpriteAtlas gradeSpriteAtlas;

    [Header("가챠 버튼 텍스트")]
    public Text one_M_Num;
    public Text eight_M_Num;

    [Header("RedIcon")]
    public GameObject redIconObj;

    void Start()
    {
        Instantiated();
        GachaBtnUISetting();
    }

    void Instantiated() // 카드생성 
    {
        for (int i = 0; i < acquisition.childCount; i++)
        {
            Destroy(acquisition.GetChild(i).gameObject);
        }
        for (int i = 0; i < unAcqusition.childCount; i++)
        {
            Destroy(unAcqusition.GetChild(i).gameObject);
        }

        int len = WeaponeChart.instance.WeaponeType().Count;
        for (int i = 0; i < len; i++)
        {
            Instantiate(card, Vector2.zero, Quaternion.identity, acquisition);
            Instantiate(card, Vector2.zero, Quaternion.identity, unAcqusition);
        }
    }
    void Init() // 카드 정보 초기화 (백지로 만들기)
    {
        for (int i = 0; i < acquisition.childCount; i++)
        {
            acquisition.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < unAcqusition.childCount; i++)
        {
            unAcqusition.GetChild(i).gameObject.SetActive(false);
        }

        int len = WeaponeChart.instance.WeaponeType().Count; // 전체카드 수 
        int userLen = UserInfo.instance.userWeapones.Count; // 유저 카드 수 
        int noneLen = len - userLen; // none카드 수 

        acquisition.GetComponent<RectTransform>().sizeDelta = new Vector2(acquisition.GetComponent<RectTransform>().sizeDelta.x, 175 * userLen);
        unAcqusition.GetComponent<RectTransform>().sizeDelta = new Vector2(unAcqusition.GetComponent<RectTransform>().sizeDelta.x, 175 * noneLen);
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, 175 * len + 80);
    }
    void UISetting() // 카드에 정보 넣기 
    {
        List<string> weaponeTypeList = WeaponeChart.instance.WeaponeType();
        int ac = 0;
        int un = 0;
        for (int i = 0; i < weaponeTypeList.Count; i++)
        {
            string name = weaponeTypeList[i];
            if (UserInfo.instance.GetUserWeaponeInfo(weaponeTypeList[i]) != null) // 해당 미사일 정보를 유저가 가지고 있다면 
            {
                acquisition.GetChild(ac).gameObject.SetActive(true);

                UserWeapone userWeapone = UserInfo.instance.GetUserWeaponeInfo(weaponeTypeList[i]);
                WeaponeChartInfo weaponeChart = WeaponeChart.instance.GetWeaponeChartInfo(userWeapone.name)[userWeapone.upgrade];

                acquisition.GetChild(ac).Find("장착버튼").GetComponent<Image>().sprite = aBtnBackSprite;
                acquisition.GetChild(ac).Find("장착버튼").GetComponent<Button>().onClick.RemoveAllListeners();
                acquisition.GetChild(ac).Find("장착버튼").GetComponent<Button>().onClick.AddListener(() => {
                    UserInfo.instance.EqipWeapone(userWeapone.name);
                    UISetting();
                    UserInfo.instance.SaveWeapone(() => {  });
                });

                acquisition.GetChild(ac).Find("Icon").Find("back").GetComponent<Image>().sprite = aIconBackSprite;
                acquisition.GetChild(ac).Find("Icon").Find("icon").GetComponent<Image>().sprite = weaponeChart.Image;
                acquisition.GetChild(ac).Find("Icon").Find("icon").GetComponent<Image>().color = Color.white;
                acquisition.GetChild(ac).Find("Icon").Find("별등급").GetComponent<Image>().sprite = gradeSpriteAtlas.GetSprite(weaponeChart.GradeType.ToString());

                acquisition.GetChild(ac).Find("이름").GetComponent<Text>().text = userWeapone.name + " Lv. " + userWeapone.upgrade;
                acquisition.GetChild(ac).Find("이름").GetComponent<Text>().color = Color.white;

                acquisition.GetChild(ac).Find("능력치").GetComponent<Text>().text = "공격력% + " + weaponeChart.AtkPercent + "%\n" + "공격속도 + " + weaponeChart.AtkSpeed + "%\n" + "크리티컬 확률 + " + weaponeChart.CriticalPercent + "%";
                acquisition.GetChild(ac).Find("능력치").GetComponent<Text>().color = Color.white;

                acquisition.GetChild(ac).Find("강화").GetComponent<Button>().onClick.RemoveAllListeners();
                if (WeaponeChart.instance.GetWeaponeChartInfo(userWeapone.name).Count > userWeapone.upgrade + 1) // 현재 maxUpgrade상태가 아니라면
                {
                    acquisition.GetChild(ac).Find("강화").GetComponent<Button>().onClick.AddListener(() => {
                        if (userWeapone.num >= weaponeChart.ConbinationNum)
                        {
                            UserInfo.instance.UpgradeWeapone(userWeapone.name, weaponeChart.ConbinationNum);
                            UISetting();
                            UserInfo.instance.SaveWeapone(() => { });
                        }
                    });
                    acquisition.GetChild(ac).Find("강화").Find("카드수").GetComponent<Text>().text = userWeapone.num + "/" + weaponeChart.ConbinationNum;
                    acquisition.GetChild(ac).Find("강화").Find("RedIcon").gameObject.SetActive(userWeapone.num >= weaponeChart.ConbinationNum);
                }
                else
                {
                    acquisition.GetChild(ac).Find("강화").Find("RedIcon").gameObject.SetActive(false);
                    acquisition.GetChild(ac).Find("강화").Find("카드수").GetComponent<Text>().text = userWeapone.num + "/MAX";
                }
              
                acquisition.GetChild(ac).Find("강화").Find("fore").GetComponent<Image>().fillAmount = userWeapone.num / (float)weaponeChart.ConbinationNum;

                acquisition.GetChild(ac).Find("Toggle").GetComponent<Toggle>().isOn = userWeapone.isEqip;

                ac++;
            }
            else
            {
                unAcqusition.GetChild(un).gameObject.SetActive(true);

                WeaponeChartInfo weaponeChart = WeaponeChart.instance.GetWeaponeChartInfo(name)[0];

                unAcqusition.GetChild(un).Find("장착버튼").GetComponent<Image>().sprite = bBtnBackSprite;
                unAcqusition.GetChild(un).Find("장착버튼").GetComponent<Button>().onClick.RemoveAllListeners();

                unAcqusition.GetChild(un).Find("Icon").Find("back").GetComponent<Image>().sprite = bIconBackSprite;
                unAcqusition.GetChild(un).Find("Icon").Find("icon").GetComponent<Image>().sprite = weaponeChart.Image;
                unAcqusition.GetChild(un).Find("Icon").Find("icon").GetComponent<Image>().color = Color.gray;
                unAcqusition.GetChild(un).Find("Icon").Find("별등급").GetComponent<Image>().sprite = gradeSpriteAtlas.GetSprite("빈"+weaponeChart.GradeType.ToString());

                unAcqusition.GetChild(un).Find("이름").GetComponent<Text>().text = name;
                unAcqusition.GetChild(un).Find("이름").GetComponent<Text>().color = Color.gray;

                unAcqusition.GetChild(un).Find("능력치").GetComponent<Text>().text = "공격력% + " + weaponeChart.AtkPercent + "%\n" + "공격속도 + " + weaponeChart.AtkSpeed + "%\n" + "크리티컬 확률 + " + weaponeChart.CriticalPercent + "%";
                unAcqusition.GetChild(un).Find("능력치").GetComponent<Text>().color = Color.gray;

                unAcqusition.GetChild(un).Find("강화").GetComponent<Button>().onClick.RemoveAllListeners();
                unAcqusition.GetChild(un).Find("강화").Find("fore").GetComponent<Image>().fillAmount = 0;
                unAcqusition.GetChild(un).Find("강화").Find("카드수").GetComponent<Text>().text = 0 + "/" + weaponeChart.ConbinationNum;

                unAcqusition.GetChild(un).Find("Toggle").GetComponent<Toggle>().isOn = false;

                un++;
            }
        }

        RedIconCheck();
    }

    public void Open()
    {
        Init();
        UISetting();
    }

    void GachaBtnUISetting()
    {
        int money = GachaChart.instance.gachaChartInfos.Weapone_Crystal_Num;
        one_M_Num.text = money.ToString();
        eight_M_Num.text = (money * 8).ToString();
    }

    public bool RedIconCheck()
    {
        List<UserWeapone> userWeapones = UserInfo.instance.userWeapones;
        for (int i = 0; i < userWeapones.Count; i++)
        {
            int num = userWeapones[i].num;
            int combinationNum = WeaponeChart.instance.GetWeaponeChartInfo(userWeapones[i].name)[userWeapones[i].upgrade].ConbinationNum;
            if (num >= combinationNum)
            {
                if (WeaponeChart.instance.GetWeaponeChartInfo(userWeapones[i].name).Count > userWeapones[i].upgrade + 1)
                {
                    redIconObj.SetActive(true);
                    return true;
                }
            }
        }
        redIconObj.SetActive(false);
        return false;
    }
}
