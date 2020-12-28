using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MasicMissileManager : MonoBehaviour
{
    public Transform acquisition;
    public Transform unAcqusition;
    public float hight;
    public RectTransform content;
    public Sprite colorBtnSprite;
    public Sprite noneBtnSprite;

    public GameObject card;

    [Header("UI")]
    public Scrollbar scrollbar;
    public Transform switchCon;
    public Transform minSwitchCon;
    public Transform maxSwitchCon;
    public GameObject redIcon;
    [Header("가챠")]
    public SpriteAtlas moneySpriteAtlas;
    public Transform oneBtn;
    public Transform tenBtn;
    public MasicMissileGachaResult masicMissileGachaResult;

    private void Start()
    {
        Intantiate();
    }

    public void Init()
    {
        int cardLen = MasicMissileChart.instance.MasicMissileType().Count; // 전체 카드 수
        int userLen = UserInfo.instance.userMasicMissiles.Count; // 유저 카드 수 
        int noneCardLen = cardLen - userLen; // 가지고 있지 않는 카드 수 

        if (userLen % 2 == 1) userLen++;
        if (noneCardLen % 2 == 1) noneCardLen++;

        for (int i = 0; i < acquisition.childCount; i++)
        {
            if (i < userLen)
            {
                acquisition.GetChild(i).gameObject.SetActive(true);
                for (int j = 0; j < acquisition.GetChild(i).childCount; j++)
                {
                    acquisition.GetChild(i).GetChild(j).gameObject.SetActive(false);
                }
            }
            else
            {
                acquisition.GetChild(i).gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < unAcqusition.childCount; i++)
        {
            if (i < noneCardLen)
            {
                unAcqusition.GetChild(i).gameObject.SetActive(true);
                for (int j = 0; j < unAcqusition.GetChild(i).childCount; j++)
                {
                    unAcqusition.GetChild(i).GetChild(j).gameObject.SetActive(false);
                }
            }
            else
            {
                unAcqusition.GetChild(i).gameObject.SetActive(false);
            }
        }

        // Transfomr길이 
         float ay = hight * (userLen / 2); 
        acquisition.GetComponent<RectTransform>().sizeDelta = new Vector2(acquisition.GetComponent<RectTransform>().sizeDelta.x, ay);
        float uy = hight * (noneCardLen / 2); 
        unAcqusition.GetComponent<RectTransform>().sizeDelta = new Vector2(unAcqusition.GetComponent<RectTransform>().sizeDelta.x, uy);
        content.sizeDelta = new Vector2(content.sizeDelta.x, ay + uy + 110);

        UiSetting();
        GachaSetting();
    }
    void Intantiate()
    {
        int cardLen = MasicMissileChart.instance.MasicMissileType().Count; // 전체 카드 수

        // 카드 생성 
        for (int i = 0; i < cardLen; i++) // 유저가 가지고 있는 미사일 수만큼 카드 생성 
        {
            Instantiate(card, Vector2.zero, Quaternion.identity, acquisition);
        }
        if (cardLen % 2 == 1) // 유저 미사일 수가 홀수라면 빈 카드 하나 더 생성 
        {
            Instantiate(card, Vector2.zero, Quaternion.identity, acquisition);
        }
        for (int i = 0; i < cardLen; i++) // 유저가 가지고 있는 미사일 수만큼 카드 생성 
        {
            Instantiate(card, Vector2.zero, Quaternion.identity, unAcqusition);
        }
        if (cardLen % 2 == 1) // 유저 미사일 수가 홀수라면 빈 카드 하나 더 생성 
        {
            Instantiate(card, Vector2.zero, Quaternion.identity, unAcqusition);
        }
    }// 빈 카드 생성  
    void UiSetting() 
    {
        List<string> masicMissileTypeList = MasicMissileChart.instance.MasicMissileType();
        int ac = 0;
        int un = 0;
        for (int i = 0; i < masicMissileTypeList.Count; i++)
        {
            string name = masicMissileTypeList[i];
            if (UserInfo.instance.GetUserMasicMissileInfo(masicMissileTypeList[i]) != null) // 해당 미사일 정보를 유저가 가지고 있다면 
            {
                UserMasicMissile userMasicMissile = UserInfo.instance.GetUserMasicMissileInfo(masicMissileTypeList[i]);
                MasicMissileChartInfo masicMissileInfo = MasicMissileChart.instance.GetMasicMissileInfo(masicMissileTypeList[i])[userMasicMissile.Upgrade];
              
                acquisition.GetChild(ac).Find("Name").gameObject.SetActive(true);
                acquisition.GetChild(ac).Find("Name").GetComponent<Text>().text = masicMissileTypeList[i] + " Lv. " + (userMasicMissile.Upgrade + 1);
               
                acquisition.GetChild(ac).Find("IconBtn").gameObject.SetActive(true);
                acquisition.GetChild(ac).Find("IconBtn").Find("Image").GetComponent<Image>().sprite = masicMissileInfo.Image;
                acquisition.GetChild(ac).Find("IconBtn").GetComponent<Button>().onClick.RemoveAllListeners();
                if (userMasicMissile.isEqip)
                {
                    acquisition.GetChild(ac).Find("IconBtn").Find("Toggle").GetComponent<Toggle>().isOn = true;
                    acquisition.GetChild(ac).Find("IconBtn").Find("사용중").gameObject.SetActive(true);
                }
                else
                {
                    acquisition.GetChild(ac).Find("IconBtn").Find("Toggle").GetComponent<Toggle>().isOn = false;
                    acquisition.GetChild(ac).Find("IconBtn").Find("사용중").gameObject.SetActive(false);
                 
                    acquisition.GetChild(ac).Find("IconBtn").GetComponent<Button>().onClick.AddListener(() => {
                        UserInfo.instance.EqipMasicMissile(name);
                        UiSetting();
                        UserInfo.instance.SaveMasicMissile(() => { });
                    });
                }
               

                acquisition.GetChild(ac).Find("능력").gameObject.SetActive(true);
                acquisition.GetChild(ac).Find("능력").GetComponent<Text>().text = masicMissileInfo.AbilityType.ToString() + " + " + masicMissileInfo.AbilityNum + "%";
              
                acquisition.GetChild(ac).Find("합성버튼").gameObject.SetActive(true);
                acquisition.GetChild(ac).Find("합성버튼").GetComponent<Button>().onClick.RemoveAllListeners();
                acquisition.GetChild(ac).Find("합성버튼").Find("Text").GetComponent<Text>().text = userMasicMissile.Num + "/" + masicMissileInfo.ConbinationNum + " 합성";
                acquisition.GetChild(ac).Find("합성버튼").Find("Gagefore").GetComponent<Image>().fillAmount = userMasicMissile.Num / (float)masicMissileInfo.ConbinationNum;
                switch (userMasicMissile.Num / masicMissileInfo.ConbinationNum >= 1)
                {
                    case true: // 유저의 카드수가 합성수보다 많을 경우 
                        int needNum = masicMissileInfo.ConbinationNum;
                        acquisition.GetChild(ac).Find("합성버튼").GetComponent<Button>().onClick.AddListener(() => {
                            UserInfo.instance.UpgradeMasicMissile(name, needNum);
                            UserInfo.instance.SaveMasicMissile(() => { });
                            UiSetting();
                        });
                        acquisition.GetChild(ac).Find("합성버튼").Find("RedIcon").gameObject.SetActive(true);
                        break;
                    case false:
                        acquisition.GetChild(ac).Find("합성버튼").Find("RedIcon").gameObject.SetActive(false);
                        break;
                }
                ac++;
            }
            else
            {
                MasicMissileChartInfo masicMissileInfo = MasicMissileChart.instance.GetMasicMissileInfo(masicMissileTypeList[i])[0];
                unAcqusition.GetChild(un).Find("Name").gameObject.SetActive(true);
                unAcqusition.GetChild(un).Find("Name").GetComponent<Text>().text = masicMissileTypeList[i];

                unAcqusition.GetChild(un).Find("IconBtn").gameObject.SetActive(true);
                unAcqusition.GetChild(un).Find("IconBtn").Find("Image").GetComponent<Image>().sprite = masicMissileInfo.Image;
                unAcqusition.GetChild(un).Find("IconBtn").Find("Image").GetComponent<Image>().color = Color.gray;
                unAcqusition.GetChild(un).Find("IconBtn").GetComponent<Button>().onClick.RemoveAllListeners();
                unAcqusition.GetChild(un).Find("IconBtn").Find("Toggle").GetComponent<Toggle>().isOn = false;
                unAcqusition.GetChild(un).Find("IconBtn").Find("Lock").gameObject.SetActive(true);
                unAcqusition.GetChild(un).Find("IconBtn").Find("사용중").gameObject.SetActive(false);

                unAcqusition.GetChild(un).Find("능력").gameObject.SetActive(true);
                unAcqusition.GetChild(un).Find("능력").GetComponent<Text>().text = masicMissileInfo.AbilityType.ToString() + " + " + masicMissileInfo.AbilityNum + "%";

                unAcqusition.GetChild(un).Find("합성버튼").gameObject.SetActive(true);
                unAcqusition.GetChild(un).Find("합성버튼").GetComponent<Image>().sprite = noneBtnSprite;
                unAcqusition.GetChild(un).Find("합성버튼").GetComponent<Button>().onClick.RemoveAllListeners();
                unAcqusition.GetChild(un).Find("합성버튼").Find("Text").GetComponent<Text>().text = "0/0 합성";
                unAcqusition.GetChild(un).Find("합성버튼").Find("RedIcon").gameObject.SetActive(false);
                un++;
            }
        }

        redIcon.SetActive(CheckRedIcon());
    }// 카드 정보 입력 

    public bool CheckRedIcon()
    {
        for (int i = 0; i < UserInfo.instance.userMasicMissiles.Count; i++)
        {
            string name = UserInfo.instance.userMasicMissiles[i].Name;
            int upgrade = UserInfo.instance.userMasicMissiles[i].Upgrade;
            int num = UserInfo.instance.userMasicMissiles[i].Num;
            int max = MasicMissileChart.instance.GetMasicMissileInfo(name)[upgrade].ConbinationNum;
            if (num >= max)
            {
                return true;
            }
        }
        return false;
    }

    void GachaSetting() // 가챠정보 입력 
    {
        int masicMissile_M_Num = GachaChart.instance.gachaChartInfos.MasicMissile_Crystal_Num;
        // 가격
        oneBtn.Find("Text").GetComponent<Text>().text = masicMissile_M_Num.ToString();
        tenBtn.Find("Text").GetComponent<Text>().text = (masicMissile_M_Num * 6).ToString();

        // 버튼 기능 
        oneBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        oneBtn.GetComponent<Button>().onClick.AddListener(() => {
            if (UserInfo.instance.crystal >= masicMissile_M_Num)
            {
                MoneyManager.instance.CrystalSub(masicMissile_M_Num);
                UserInfo.instance.SaveMoney(()=> { });
                GachaBoxOpen01();
            }
        });
        tenBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        tenBtn.GetComponent<Button>().onClick.AddListener(() => {
            if (UserInfo.instance.crystal >= masicMissile_M_Num * 6)
            {
                MoneyManager.instance.CrystalSub(masicMissile_M_Num * 6);
                UserInfo.instance.SaveMoney(() => { });
                GachaBoxOpen06();
            }
          
        });
   
    }
    void GachaBoxOpen01()
    {
        masicMissileGachaResult.Open01();
    }
    void GachaBoxOpen06()
    {
        masicMissileGachaResult.Open06();
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
}
