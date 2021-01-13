using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class DayByQuestManager : MonoBehaviour
{
    public Transform middlePannel;
    public GameObject dayByQuestPannel;

    public Transform dayContent;

    public SpriteAtlas moneySpriteAtlas;

    [Header("스크립트")]
    public QuestManager questManager;
    public MonsterUIManager monsterUIManager;
    public NomalMonsterManager nomalMonsterManager;
    public ItemGetManager ItemGetManager;

    [ContextMenu("테스트")]
    public void Test()
    {
        UserInfo.instance.PushUserDayByQuestCount(1);
        UISetting();
    }

    public void Open()
    {
        for (int i = 0; i < middlePannel.childCount; i++)
        {
            middlePannel.GetChild(i).gameObject.SetActive(false);
        }
        dayByQuestPannel.SetActive(true);
        UISetting();
    }
    public void UISetting()
    {
        DayByQuestChartInfo[] dayByQuestChartInfos = DayByQuestChart.instance.dayByQuestChartInfos;
        List<DayByQuestChartInfo> completeDayByQuestChartInfos = new List<DayByQuestChartInfo>();

        int j = 0;
        for (int i = 0; i < dayByQuestChartInfos.Length; i++) // 보상수령 x 
        {
            string title = dayByQuestChartInfos[i].Title;
            string content = dayByQuestChartInfos[i].Content;
            int count = dayByQuestChartInfos[i].Count;
            int point = dayByQuestChartInfos[i].Point;
            MoneyType reward01 = dayByQuestChartInfos[i].Reward01;
            int reward01Count = dayByQuestChartInfos[i].Reward01Count;
            MoneyType reward02 = dayByQuestChartInfos[i].Reward02;
            int reward02Count = dayByQuestChartInfos[i].Reward02Count;
            MoneyType mainReward = dayByQuestChartInfos[i].MainReward;
            int mainRewardCount = dayByQuestChartInfos[i].MainRewardCount;

            int userCount = UserInfo.instance.GetUserDayByQuestCount(i);
            bool userComplete = UserInfo.instance.GetUserDayByQuestComplete(i);
            bool isSucess = userCount >= count ? true : false;

            if (!userComplete)
            {
                Transform card = dayContent.GetChild(j);

                card.GetComponent<Image>().color = Color.white;

                card.Find("활약도").GetComponent<Text>().text = "활약도 + " + point;
                card.Find("대표보상").Find("Icon").GetComponent<Image>().sprite = moneySpriteAtlas.GetSprite(mainReward.ToString());
                card.Find("대표보상").Find("Icon").GetComponent<Image>().SetNativeSize();
                card.Find("대표보상").Find("Icon").GetComponent<Image>().color = Color.white;
                card.Find("대표보상").Find("카운트").GetComponent<Text>().text = "보상 : 크리스탈 x " + mainRewardCount;

                card.Find("퀘스트제목").Find("Title").GetComponent<Text>().text = title;
                card.Find("퀘스트제목").Find("Content").GetComponent<Text>().text = content;

                card.Find("횟수").GetComponent<Text>().text = userCount + "/" + count;

                card.Find("보상01").Find("Icon").GetComponent<Image>().sprite = moneySpriteAtlas.GetSprite(reward01.ToString());
                card.Find("보상01").Find("Icon").GetComponent<Image>().SetNativeSize();
                card.Find("보상01").Find("Icon").GetComponent<Image>().color = Color.white;
                card.Find("보상01").Find("Count").GetComponent<Text>().text = reward01Count.ToString();
                card.Find("보상01").Find("RedIcon").gameObject.SetActive(isSucess);

                card.Find("보상02").Find("Icon").GetComponent<Image>().sprite = moneySpriteAtlas.GetSprite(reward02.ToString());
                card.Find("보상02").Find("Icon").GetComponent<Image>().SetNativeSize();
                card.Find("보상02").Find("Icon").GetComponent<Image>().color = Color.white;
                card.Find("보상02").Find("Count").GetComponent<Text>().text = reward02Count.ToString();
                card.Find("보상02").Find("RedIcon").gameObject.SetActive(isSucess);

                card.Find("바로가기").gameObject.SetActive(false);
                card.Find("수령하기").gameObject.SetActive(false);
                card.Find("완료").gameObject.SetActive(false);

                int ID = i;
                if (!userComplete && !isSucess)
                {
                    card.Find("바로가기").gameObject.SetActive(true);
                    card.Find("바로가기").GetComponent<Button>().onClick.RemoveAllListeners();
                    card.Find("바로가기").GetComponent<Button>().onClick.AddListener(() => {
                        OnClickMove(ID);
                    });
                }
                if (!userComplete && isSucess)
                {
                    List<MoneyType> moneyTypeList = new List<MoneyType>();
                    List<int> countList = new List<int>();
                    moneyTypeList.Add(reward01);
                    countList.Add(reward01Count);
                    moneyTypeList.Add(reward02);
                    countList.Add(reward02Count);
                    moneyTypeList.Add(mainReward);
                    countList.Add(mainRewardCount);

                    card.Find("수령하기").gameObject.SetActive(true);
                    card.Find("수령하기").GetComponent<Button>().onClick.RemoveAllListeners();
                    card.Find("수령하기").GetComponent<Button>().onClick.AddListener(() => {
                        OnClickReward(ID, moneyTypeList.ToArray(), countList.ToArray());
                    });
                }

                j++;
            }
            else
            {
                completeDayByQuestChartInfos.Add(dayByQuestChartInfos[i]);
            }
        }

        for (int i = 0; i < completeDayByQuestChartInfos.Count; i++) // 보상수령 o
        {
            string title = completeDayByQuestChartInfos[i].Title;
            string content = completeDayByQuestChartInfos[i].Content;
            int count = completeDayByQuestChartInfos[i].Count;
            int point = completeDayByQuestChartInfos[i].Point;
            MoneyType reward01 = completeDayByQuestChartInfos[i].Reward01;
            int reward01Count = completeDayByQuestChartInfos[i].Reward01Count;
            MoneyType reward02 = completeDayByQuestChartInfos[i].Reward02;
            int reward02Count = completeDayByQuestChartInfos[i].Reward02Count;
            MoneyType mainReward = completeDayByQuestChartInfos[i].MainReward;
            int mainRewardCount = completeDayByQuestChartInfos[i].MainRewardCount;

            Transform card = dayContent.GetChild(j);
            card.GetComponent<Image>().color = Color.gray;

            card.Find("활약도").GetComponent<Text>().text = "활약도 + " + point;
            card.Find("대표보상").Find("Icon").GetComponent<Image>().sprite = moneySpriteAtlas.GetSprite(mainReward.ToString());
            card.Find("대표보상").Find("Icon").GetComponent<Image>().SetNativeSize();
            card.Find("대표보상").Find("Icon").GetComponent<Image>().color = Color.gray;
            card.Find("대표보상").Find("카운트").GetComponent<Text>().text = "보상 : 크리스탈 x " + mainRewardCount;

            card.Find("퀘스트제목").Find("Title").GetComponent<Text>().text = title;
            card.Find("퀘스트제목").Find("Content").GetComponent<Text>().text = content;

            card.Find("횟수").GetComponent<Text>().text = count + "/" + count;

            card.Find("보상01").Find("Icon").GetComponent<Image>().sprite = moneySpriteAtlas.GetSprite(reward01.ToString());
            card.Find("보상01").Find("Icon").GetComponent<Image>().SetNativeSize();
            card.Find("보상01").Find("Icon").GetComponent<Image>().color = Color.gray;
            card.Find("보상01").Find("Count").GetComponent<Text>().text = reward01Count.ToString();
            card.Find("보상01").Find("RedIcon").gameObject.SetActive(false);

            card.Find("보상02").Find("Icon").GetComponent<Image>().sprite = moneySpriteAtlas.GetSprite(reward02.ToString());
            card.Find("보상02").Find("Icon").GetComponent<Image>().SetNativeSize();
            card.Find("보상02").Find("Icon").GetComponent<Image>().color = Color.gray;
            card.Find("보상02").Find("Count").GetComponent<Text>().text = reward02Count.ToString();
            card.Find("보상02").Find("RedIcon").gameObject.SetActive(false);

            card.Find("바로가기").gameObject.SetActive(false);
            card.Find("수령하기").gameObject.SetActive(false);

            card.Find("완료").gameObject.SetActive(true);

            j++;
        }
    }

    public void OnClickMove(int ID) // 바로가기
    {
        switch (ID)
        {
            case 0: // 핫 타임 
                return;
            case 1: // 일반 몬스터 3회 클 
                monsterUIManager.Open();
                nomalMonsterManager.Open();
                break;
            case 2: // 보스 몬스터 3회 클 
                monsterUIManager.Open();
                nomalMonsterManager.Open();
                break;
            case 3: // 가챠 1회 
                return;
            case 4: // 암흑 1회
                monsterUIManager.Open();
                break;
            case 5: // 마녀 
                monsterUIManager.Open();
                break;
        }
        questManager.Close();
    }
    public void OnClickReward(int dayQuestID ,MoneyType[] moneyTypes, int[] count) // 보상받기
    {
        UserInfo.instance.PushUserDayByQuestComplete(dayQuestID);
        UISetting();

        UserInfo.instance.SaveUserDayByQuest(() => { ItemGetManager.Open(moneyTypes, count); });
    }
}
