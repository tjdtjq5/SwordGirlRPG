using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NomalMonsterManager : MonoBehaviour
{
    public GameObject mainPannel;
    public GameObject nomalPannel;

    public GameObject card;
    public Transform content;

    public int waitTimeTotalSecond;

    IEnumerator[] timeUICoruitne;

    private void Start()
    {
        Instantiated();
    }
    public void Open()
    {
        mainPannel.SetActive(false);
        nomalPannel.SetActive(true);
        UISetting();
    }
    public void BackBtn()
    {
        mainPannel.SetActive(true);
        nomalPannel.SetActive(false);

        for (int i = 0; i < timeUICoruitne.Length; i++)
        {
            if (timeUICoruitne[i] != null)
            {
                StopCoroutine(timeUICoruitne[i]);
            }
        }
    }
    void Instantiated() // 생성
    {
        int len = NomalMonsterChart.instance.nomalMonsterChartInfos.Length;

        timeUICoruitne = new IEnumerator[len];

        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        for (int i = 0; i < len; i++)
        {
            Instantiate(card, Vector3.zero, Quaternion.identity, content);
        }

        content.GetComponent<RectTransform>().sizeDelta = new Vector2(400 * len + 45, content.GetComponent<RectTransform>().sizeDelta.y);
    }

    void UISetting()
    {
        NomalMonsterChartInfo[] nomalMonsterChartInfos = NomalMonsterChart.instance.nomalMonsterChartInfos;
        for (int i = 0; i < nomalMonsterChartInfos.Length; i++)
        {
            Transform card = content.GetChild(i);

            string name = nomalMonsterChartInfos[i].Name;
            Sprite image = nomalMonsterChartInfos[i].Image;
            int rewardCrystalNum = nomalMonsterChartInfos[i].RewardCrystalNum;
            int rewardEnhanceStoneNum = nomalMonsterChartInfos[i].RewardEnhanceStoneNum;
            string conditionName = nomalMonsterChartInfos[i].ConditionName;
            UserNomalMonster userNomalMonster = UserInfo.instance.GetUserNomalMonsterInfo(name);

            card.GetComponent<Image>().sprite = image;
            card.Find("이름").GetComponent<Text>().text = name;
            card.Find("보상").Find("크리스탈").Find("Text").GetComponent<Text>().text = "x" + rewardCrystalNum;
            card.Find("보상").Find("강화석").Find("Text").GetComponent<Text>().text = "x" + rewardEnhanceStoneNum;
            card.Find("입장버튼").GetComponent<Button>().onClick.RemoveAllListeners();
            card.Find("소탕버튼").GetComponent<Button>().onClick.RemoveAllListeners();
            card.Find("Lock").gameObject.SetActive(false);

            // 조건이 없거나 조건에 충족했다면 입장가능, 소탕가능 
            if (string.IsNullOrEmpty(conditionName) || UserInfo.instance.GetUserNomalMonsterInfo(conditionName) != null)
            {

            }

            if (userNomalMonster != null) // 해당 카드에 유저정보가 들어있다면 시간 계산
            {
                DateTime clearDateTime = userNomalMonster.clearDateTime;

                BackendReturnObject servertime = Backend.Utils.GetServerTime();
                string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
                DateTime currentTime = DateTime.Parse(time);

                TimeSpan subTime = currentTime.Subtract(clearDateTime);

                if (subTime.TotalSeconds < waitTimeTotalSecond) // 방금 클리어했기에 잠금 
                {
                    card.Find("Lock").gameObject.SetActive(true);

                    if (timeUICoruitne[i] != null)
                    {
                        StopCoroutine(timeUICoruitne[i]);
                    }
                    timeUICoruitne[i] = TimeUICoruitne(card.Find("Lock").Find("시간").GetComponent<Text>(), (int)subTime.TotalSeconds);
                    StartCoroutine(timeUICoruitne[i]);

                    card.Find("입장버튼").GetComponent<Button>().onClick.RemoveAllListeners();
                    card.Find("소탕버튼").GetComponent<Button>().onClick.RemoveAllListeners();
                }
            }
           
        }
    }

    IEnumerator TimeUICoruitne(Text uiText, int totalSecond)
    {
        WaitForSeconds waitTime = new WaitForSeconds(1);
        while (totalSecond > 0)
        {
            int minute = (int)(totalSecond / 60);
            int second = (int)(totalSecond % 60);
            uiText.text = minute + " : " + second;
            totalSecond--;
            yield return waitTime;
        }
    }
}
