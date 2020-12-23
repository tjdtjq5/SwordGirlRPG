using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasicMissileManager : MonoBehaviour
{
    public Transform acquisition;
    public Transform unAcqusition;
    public float hight;

    public GameObject card;

    public void Init()
    {
        Destroy();
        Intantiate();
        UiSetting();
    }
    void Destroy()
    {
        for (int i = 0; i < acquisition.childCount; i++)
        {
            Destroy(acquisition.GetChild(i).gameObject);
        }
        for (int i = 0; i < unAcqusition.childCount; i++)
        {
            Destroy(unAcqusition.GetChild(i).gameObject);
        }
    }
    void Intantiate()
    {
        int cardLen = MasicMissileChart.instance.MasicMissileType().Count; // 전체 카드 수
        int userLen = UserInfo.instance.userMasicMissiles.Count; // 유저 카드 수 
        int noneCardLen = cardLen - userLen; // 가지고 있지 않는 카드 수 

        // Transfomr길이 
        acquisition.GetComponent<RectTransform>().sizeDelta = new Vector2(acquisition.GetComponent<RectTransform>().sizeDelta.x, hight * (userLen / 2 + 1));
        unAcqusition.GetComponent<RectTransform>().sizeDelta = new Vector2(unAcqusition.GetComponent<RectTransform>().sizeDelta.x, hight * (noneCardLen / 2 + 1));

        // 카드 생성 
        for (int i = 0; i < userLen; i++) // 유저가 가지고 있는 미사일 수만큼 카드 생성 
        {
            Instantiate(card, Vector2.zero, Quaternion.identity, acquisition);
        }
        if (userLen % 2 == 1) // 유저 미사일 수가 홀수라면 빈 카드 하나 더 생성 
        {
            Instantiate(card, Vector2.zero, Quaternion.identity, acquisition);
        }
        for (int i = 0; i < noneCardLen; i++) // 유저가 가지고 있는 미사일 수만큼 카드 생성 
        {
            Instantiate(card, Vector2.zero, Quaternion.identity, unAcqusition);
        }
        if (noneCardLen % 2 == 1) // 유저 미사일 수가 홀수라면 빈 카드 하나 더 생성 
        {
            Instantiate(card, Vector2.zero, Quaternion.identity, unAcqusition);
        }
    }
    void UiSetting() 
    {
        List<string> masicMissileTypeList = MasicMissileChart.instance.MasicMissileType();
        int ac = 0;
        int un = 0;
        Debug.Log(acquisition.childCount);
        for (int i = 0; i < masicMissileTypeList.Count; i++)
        {
            if (UserInfo.instance.GetUserMasicMissileInfo(masicMissileTypeList[i]) != null) // 해당 미사일 정보를 유저가 가지고 있다면 
            {
                UserMasicMissile userMasicMissile = UserInfo.instance.GetUserMasicMissileInfo(masicMissileTypeList[i]);
                MasicMissileChartInfo masicMissileInfo = MasicMissileChart.instance.GetMasicMissileInfo(masicMissileTypeList[i])[userMasicMissile.Upgrade];
              
                acquisition.GetChild(ac).Find("Name").gameObject.SetActive(true);
                acquisition.GetChild(ac).Find("Name").GetComponent<Text>().text = masicMissileTypeList[i] + " Lv. " + (userMasicMissile.Upgrade + 1);
               
                acquisition.GetChild(ac).Find("IconBtn").gameObject.SetActive(true);
                acquisition.GetChild(ac).Find("IconBtn").Find("Image").GetComponent<Image>().sprite = masicMissileInfo.Image;
                acquisition.GetChild(ac).Find("IconBtn").Find("Toggle").GetComponent<Toggle>().isOn = true;

                acquisition.GetChild(ac).Find("능력").gameObject.SetActive(true);
                acquisition.GetChild(ac).Find("능력").GetComponent<Text>().text = masicMissileInfo.AbilityType.ToString() + " + " + masicMissileInfo.AbilityNum + "%";
              
                acquisition.GetChild(ac).Find("합성버튼").gameObject.SetActive(true);
                acquisition.GetChild(ac).Find("합성버튼").GetComponent<Button>().onClick.RemoveAllListeners();
                acquisition.GetChild(ac).Find("합성버튼").Find("Text").GetComponent<Text>().text = userMasicMissile.Num + "/" + masicMissileInfo.ConbinationNum + " 합성";
                acquisition.GetChild(ac).Find("합성버튼").Find("Gagefore").GetComponent<Image>().fillAmount = userMasicMissile.Num / (float)masicMissileInfo.ConbinationNum;
                switch (userMasicMissile.Num / masicMissileInfo.ConbinationNum >= 1)
                {
                    case true: // 유저의 카드수가 합성수보다 많을 경우 
                        acquisition.GetChild(ac).Find("합성버튼").GetComponent<Button>().onClick.AddListener(() => {
                            UserInfo.instance.UpgradeMasicMissile(masicMissileTypeList[i], masicMissileInfo.ConbinationNum);
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
             //   unAcqusition.GetChild(ac)
                un++;
            }
        }
    }
}
