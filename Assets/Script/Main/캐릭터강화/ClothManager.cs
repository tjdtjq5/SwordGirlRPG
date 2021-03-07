using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ClothManager : MonoBehaviour
{
    public GameObject card;
    public Transform content;
    public SpriteAtlas gradeSpriteAtlas;

    [Header("빽 이미지")]
    public Sprite aBgBackSprite;
    public Sprite bBgBackSprite;

    [Header("플레이어")]
    public PlayerController playerController;

    private void Start()
    {
        Initialized();
    }

    public void Open()
    {
        UISetting();
    }
    void Initialized() // 카드 생성 
    {
        int len = ClothChart.instance.ClothType().Count;

        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        for (int i = 0; i < len; i++)
        {
            Instantiate(card, Vector3.zero, Quaternion.identity, content);
        }

        content.GetComponent<RectTransform>().sizeDelta = new Vector2((398 * len), content.GetComponent<RectTransform>().sizeDelta.y);
    }
    public void UISetting()
    {
        int len = ClothChart.instance.ClothType().Count;
        int userLen = UserInfo.instance.userCloths.Count;

        // 유저가 가지고 있는 카드 정보 
        for (int i = 0; i < userLen; i++)
        {
            UserCloth userCloth = UserInfo.instance.userCloths[i];
            string name = userCloth.name;
            int upgrade = userCloth.upgrade;
            int num = userCloth.num;
            bool isEqip = userCloth.isEqip;
            ClothChartInfo clothChartInfo = ClothChart.instance.GetClothChartInfo(name)[upgrade];

            Transform card = content.GetChild(i);

            card.GetComponent<Image>().sprite = aBgBackSprite;

            card.Find("이름레벨").GetComponent<Text>().text = name + " Lv. " + upgrade;

            // 장착
            card.Find("Middle").GetComponent<Button>().onClick.RemoveAllListeners();
            card.Find("Middle").GetComponent<Button>().onClick.AddListener(() => {
                if (!isEqip)
                {
                    UserInfo.instance.EqipCloth(name);
                    UserInfo.instance.SaveCloth(() => { });
                    UISetting();
                    playerController.SkinChange();

                    playerController.Hp_Initialized();
                    playerController.Hp_UI_Setting();
                }
            });
            card.Find("Middle").Find("Icon").GetComponent<Image>().sprite = clothChartInfo.Image;
            card.Find("Middle").Find("Icon").GetComponent<Image>().color = Color.white;
            card.Find("Middle").Find("별등급").GetComponent<Image>().sprite = gradeSpriteAtlas.GetSprite(clothChartInfo.Grade.ToString());
            card.Find("Middle").Find("별등급").GetComponent<Image>().SetNativeSize();
            card.Find("Middle").Find("능력").GetComponent<Text>().text = "체력 " + clothChartInfo.HpPercent + "%";
            card.Find("Middle").Find("Toggle").GetComponent<Toggle>().isOn = isEqip;
            card.Find("Middle").Find("Lock").gameObject.SetActive(false);

            card.Find("강화").Find("fore").GetComponent<Image>().fillAmount = num / (float)clothChartInfo.CombinationNum;
            card.Find("강화").Find("개수").GetComponent<Text>().text = num + "/" + clothChartInfo.CombinationNum;
            card.Find("강화").Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
            card.Find("강화").Find("강화버튼").GetComponent<Button>().onClick.AddListener(() => {
                if (num >= clothChartInfo.CombinationNum && upgrade < ClothChart.instance.GetClothChartInfo(name).Count - 1)
                {

                    UserInfo.instance.UpgradeCloth(name, clothChartInfo.CombinationNum);
                    UserInfo.instance.SaveCloth(() => { });
                    UISetting();

                    playerController.Hp_Initialized();
                    playerController.Hp_UI_Setting();
                }
            });
        }

        // 가지고 있지 않는 카드 정보 
        int count = userLen;
        for (int i = 0; i < len; i++)
        {
            string name = ClothChart.instance.ClothType()[i];
            UserCloth userCloth = UserInfo.instance.GetUserClothInfo(name);
            if (userCloth == null)
            {
                ClothChartInfo clothChartInfo = ClothChart.instance.GetClothChartInfo(name)[0];
                Transform card = content.GetChild(count);

                card.GetComponent<Image>().sprite = bBgBackSprite;

                card.Find("이름레벨").GetComponent<Text>().text = name;

                card.Find("Middle").GetComponent<Button>().onClick.RemoveAllListeners();
                card.Find("Middle").Find("Icon").GetComponent<Image>().sprite = clothChartInfo.Image;
                card.Find("Middle").Find("Icon").GetComponent<Image>().color = Color.black;
                card.Find("Middle").Find("별등급").GetComponent<Image>().sprite = gradeSpriteAtlas.GetSprite("빈"+clothChartInfo.Grade.ToString());
                card.Find("Middle").Find("별등급").GetComponent<Image>().SetNativeSize();
                card.Find("Middle").Find("능력").GetComponent<Text>().text = "체력 " + clothChartInfo.HpPercent + "%";
                card.Find("Middle").Find("Toggle").GetComponent<Toggle>().isOn = false;
                card.Find("Middle").Find("Lock").gameObject.SetActive(true);

                card.Find("강화").Find("fore").GetComponent<Image>().fillAmount = 0;
                card.Find("강화").Find("개수").GetComponent<Text>().text = "0/0";
                card.Find("강화").Find("강화버튼").GetComponent<Button>().onClick.RemoveAllListeners();
                count++;
            }
        }
    }
 
}
