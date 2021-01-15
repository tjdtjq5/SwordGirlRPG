using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PassManager : MonoBehaviour
{
    public Transform middlePannel;
    public GameObject passPannel;

    public Transform content;

    public SpriteAtlas moneySpriteAtlas;

    public ItemGetManager itemGetManager;

    public void Open()
    {
        for (int i = 0; i < middlePannel.childCount; i++)
        {
            middlePannel.GetChild(i).gameObject.SetActive(false);
        }
        passPannel.SetActive(true);
        UISetting();
    }
    
    public void UISetting()
    {
        int len = PassChart.instance.passChartInfos.Length;
        int myPoint = UserInfo.instance.GetUserPassPoint();
        for (int i = 0; i < len; i++)
        {
            Transform card = content.GetChild(i);
            PassChartInfo passInfo = PassChart.instance.passChartInfos[i];

            int ID = i;
            int needPoint = passInfo.Point;
            bool nomalCompleteFlag = UserInfo.instance.GetUserPassNomalComplete(i);
            bool passCompleteFlag = UserInfo.instance.GetUserPassPassComplete(i);
            MoneyType nomalReward = passInfo.NomalReward;
            int nomalRewardCount = passInfo.NomalRewardCount;
            MoneyType passReward = passInfo.PassReward;
            int passRewardCount = passInfo.PassRewardCount;

            card.Find("활약도").Find("Text").GetComponent<Text>().text = needPoint.ToString();

            card.Find("기본보상").GetComponent<Button>().onClick.RemoveAllListeners();
            card.Find("패스보상").GetComponent<Button>().onClick.RemoveAllListeners();

            card.Find("기본보상").Find("MoneyTypeImg").GetComponent<Image>().sprite = moneySpriteAtlas.GetSprite(nomalReward.ToString());
            card.Find("기본보상").Find("MoneyTypeImg").GetComponent<Image>().SetNativeSize();
            card.Find("기본보상").Find("Count").GetComponent<Text>().text = nomalRewardCount.ToString();
            card.Find("기본보상").Find("완료").gameObject.SetActive(nomalCompleteFlag);
            card.Find("기본보상").Find("RedIcon").gameObject.SetActive(false);

            if (myPoint >= needPoint)
            {
                card.Find("기본보상").Find("RedIcon").gameObject.SetActive(!nomalCompleteFlag);

                if (!nomalCompleteFlag)
                {
                    card.Find("기본보상").GetComponent<Button>().onClick.AddListener(() => {
                        UserInfo.instance.PushUserPassNomalComplete(ID);
                        UserInfo.instance.SaveUserPass(() => {
                            itemGetManager.Open(nomalReward, nomalRewardCount);
                            UISetting();
                        });
                    });
                }
            }


            card.Find("패스보상").Find("MoneyTypeImg").GetComponent<Image>().sprite = moneySpriteAtlas.GetSprite(passReward.ToString());
            card.Find("패스보상").Find("MoneyTypeImg").GetComponent<Image>().SetNativeSize();
            card.Find("패스보상").Find("Count").GetComponent<Text>().text = passRewardCount.ToString();
            card.Find("패스보상").Find("완료").gameObject.SetActive(passCompleteFlag);
            card.Find("패스보상").Find("RedIcon").gameObject.SetActive(false);

            if (myPoint >= needPoint)
            {
                card.Find("패스보상").Find("RedIcon").gameObject.SetActive(!passCompleteFlag);

                if (!passCompleteFlag)
                {
                    card.Find("패스보상").GetComponent<Button>().onClick.AddListener(() => {
                        UserInfo.instance.PushUserPassPassComplete(ID);
                        UserInfo.instance.SaveUserPass(() => {
                            itemGetManager.Open(passReward, passRewardCount);
                            UISetting();
                        });
                    });
                }
            }

            if (!nomalCompleteFlag || !passCompleteFlag) // 카드 활성화 
            {
                card.GetComponent<Image>().color = Color.white;
                card.Find("활약도").GetComponent<Image>().color = Color.white;
          
            }
            else // 카드 비활성화
            {
                card.GetComponent<Image>().color = Color.gray;
                card.Find("활약도").GetComponent<Image>().color = Color.gray;
                card.Find("기본보상").Find("RedIcon").gameObject.SetActive(false);
                card.Find("패스보상").Find("RedIcon").gameObject.SetActive(false);
            }
        }
    }
}
