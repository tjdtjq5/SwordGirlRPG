using DG.Tweening;
using Function;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingUI : MonoBehaviour
{
    [Header("패널UI")]
    public GameObject blackPannel;
    public GameObject uiObj;

    [Header("탭UI")]
    public Sprite select_tab;
    public Sprite none_tab;
    public Image totalPower_tab;
    public Image violet_tab;
    public Image pumpkin_tab;
    public Transform totalPower_tab_position;
    public Transform violet_tab_position;
    public Transform pumpkin_tab_position;
    public Transform focus;

    [Header("카드UI")]
    public Transform content;
    public Sprite noneCard;
    public Sprite setCard;
    public Sprite myCard;
    public Sprite no1;
    public Sprite no2;
    public Sprite no3;

    [Header("내 랭킹 UI")]
    public Text myRankingText;
    public Text myScoreText;

    [Header("페이지")]
    public Button right;
    public Button left;
    public Text page_text;
    int page;
    RankingInfo myRankingInfo = null;
    List<RankingInfo> rankingInfoList = new List<RankingInfo>();

    [Header("스크립트")]
    public Ranking ranking;


    public void RankingOpen()
    {
        blackPannel.SetActive(true);
        uiObj.SetActive(true);
        TotalPowerTab();
    }
    public void RankingClose()
    {
        blackPannel.SetActive(false);
        uiObj.SetActive(false);
    }

    // 탭 
    void TabInit()
    {
        totalPower_tab.sprite = none_tab;
        violet_tab.sprite = none_tab;
        pumpkin_tab.sprite = none_tab;
    }
    public void TotalPowerTab()
    {
        TabInit();
        totalPower_tab.sprite = select_tab;
        focus.DOMoveX(totalPower_tab_position.position.x, 0);

        TotalPowerCard();
    }
    public void VioletTab()
    {
        TabInit();
        violet_tab.sprite = select_tab;
        focus.DOMoveX(violet_tab_position.position.x, 0);

        VioletCard();
    }
    public void PumpkinTab()
    {
        TabInit();
        pumpkin_tab.sprite = select_tab;
        focus.DOMoveX(pumpkin_tab_position.position.x, 0);

        PumpkinCard();
    }

    // 카드
    void CardInit()
    {
        int len = content.childCount;

        for (int i = 0; i < len; i++)
        {
            Transform card = content.GetChild(i);

            card.GetComponent<Image>().sprite = noneCard;
            card.Find("랭크").gameObject.SetActive(false);
            card.Find("프레임").gameObject.SetActive(false);
            card.Find("호칭").gameObject.SetActive(false);
            card.Find("닉네임").gameObject.SetActive(false);
            card.Find("점수").gameObject.SetActive(false);
        }
    }
    void TotalPowerCard()
    {
        CardInit();
    }
    void VioletCard()
    {
        rankingInfoList.Clear();
        myRankingInfo = null;

        page = 1;
        page_text.text = "";

        myRankingText.text = "";
        myScoreText.text = "";

        right.onClick.RemoveAllListeners();
        left.onClick.RemoveAllListeners();

        CardInit();

        // 바이올렛 랭킹 100 명 가져오기
        ranking.GetVioletRanking(100, 0, () => {
            rankingInfoList = ranking.rankingInfoList;

            Page();

            right.onClick.AddListener(() => {

                int len = content.childCount * page;
                int rankInfoLen = rankingInfoList.Count;

                if (rankInfoLen > len)
                {
                    page++;
                    Page();
                }

            });

            left.onClick.AddListener(() => {
                if (page > 1)
                {
                    page--;
                    Page();
                }
            });
        });

        // 내 랭킹 정보 가져오기
        ranking.GetMyVioletRanking(() => {
            myRankingInfo = ranking.myRankingInfo;

            if (myRankingInfo != null)
            {
                myRankingText.text = myRankingInfo.rank.ToString() + "위";
                myScoreText.text = MyMath.ValueToString(myRankingInfo.stringScore);
            }
        });
    }
    void PumpkinCard()
    {
        CardInit();
    }
    void Page()
    {
        CardInit();

        int len = content.childCount;  // 2
        int rankingInfoCount = rankingInfoList.Count; // 3
        int maxPage = rankingInfoCount / len + 1;  // 2
        page_text.text = page  + "/" + maxPage;

        int min = len * (page - 1); // 0 , 2 , 4
        int max = min + len; // 2 , 4 , 6
        if(rankingInfoCount < max) { max = rankingInfoCount; }

        int count = 0; 
        for (int i = min; i < max; i++)
        {
            RankingInfo rankingInfo = rankingInfoList[i]; 
            Transform card = content.GetChild(count);

            // 카드 이미지
            if (UserInfo.instance.nickName == rankingInfo.nickname)
            {
                card.GetComponent<Image>().sprite = myCard;
            }
            else
            {
                card.GetComponent<Image>().sprite = setCard;
            }

            // 랭크 
            card.Find("랭크").gameObject.SetActive(true);
            if (rankingInfo.rank == 1)
            {
                card.Find("랭크").GetChild(0).gameObject.SetActive(true);
                card.Find("랭크").GetChild(1).gameObject.SetActive(false);
                card.Find("랭크").GetChild(0).GetComponent<Image>().sprite = no1;
            }
            else if(rankingInfo.rank == 2)
            {
                card.Find("랭크").GetChild(0).gameObject.SetActive(true);
                card.Find("랭크").GetChild(1).gameObject.SetActive(false);
                card.Find("랭크").GetChild(0).GetComponent<Image>().sprite = no2;
            }
            else if(rankingInfo.rank == 3)
            {
                card.Find("랭크").GetChild(0).gameObject.SetActive(true);
                card.Find("랭크").GetChild(1).gameObject.SetActive(false);
                card.Find("랭크").GetChild(0).GetComponent<Image>().sprite = no3;
            }
            else
            {
                card.Find("랭크").GetChild(0).gameObject.SetActive(false);
                card.Find("랭크").GetChild(1).gameObject.SetActive(true);
                card.Find("랭크").GetChild(1).GetComponent<Text>().text = rankingInfo.rank.ToString();
            }

            // 닉네임
            card.Find("닉네임").gameObject.SetActive(true);
            card.Find("닉네임").GetComponent<Text>().text = rankingInfo.nickname;

            // 점수
            card.Find("점수").gameObject.SetActive(true);
            card.Find("점수").GetComponent<Text>().text = MyMath.ValueToString(rankingInfo.stringScore);

            count++;
        }
    }
}
