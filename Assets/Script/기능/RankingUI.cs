using BackEnd;
using DG.Tweening;
using Function;
using LitJson;
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

    // 랭킹 정보
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

        myRankingText.text = "";
        myScoreText.text = "";

        CardInit();

        // 바이올렛 랭킹 100 명 가져오기
        ranking.GetVioletRanking(100, 0, () => {
            rankingInfoList = ranking.rankingInfoList;

            RankingInfoSetting();
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
    void RankingInfoSetting() // 랭킹 ui정보 셋팅 
    {
        CardInit();

        int rankingInfoCount = rankingInfoList.Count; // 3
        int len = rankingInfoCount;
        if (content.childCount < rankingInfoCount) len = content.childCount;

        for (int i = 0; i < len; i++)
        {
            RankingInfo rankingInfo = rankingInfoList[i]; 
            Transform card = content.GetChild(i);

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

            StartCoroutine(FrameInfoSetting(card, rankingInfo.nickname));
        }
    }

    IEnumerator FrameInfoSetting(Transform card, string nickname)
    {
        yield return null;

        BackendAsyncClass.BackendAsync(Backend.Social.GetGamerIndateByNickname, nickname, (indateCallback) => {
            string gamerIndate = indateCallback.Rows()[0]["inDate"]["S"].ToString();

            BackendAsyncClass.BackendAsync(Backend.GameInfo.GetPublicContentsByGamerIndate, "PublicUserInfo", gamerIndate, (publicDataCallback) => {
                switch (publicDataCallback.GetStatusCode())
                {
                    case "200": // 성공 
                        break;
                    case "404":
                        Debug.Log("존재하지 않는 gamerIndate를 입력한 경우 && 존재하지 않는 tableName인 경우");
                        return;
                    case "400": 
                        Debug.Log("public table 아닌 table의 조회를 시도한 경우 && limit이 100이상인 경우");
                        return;
                    case "412": 
                        Debug.Log("비활성화 된 table의 조회를 시도한 경우");
                        return;
                    default:
                        break;
                }

                card.Find("프레임").gameObject.SetActive(true);
                card.Find("프레임").Find("Icon").GetComponent<Image>().sprite = ClothChart.instance.clothChartInfos[0].Icon;
                card.Find("호칭").gameObject.SetActive(true);
                card.Find("프레임").Find("frame").GetComponent<Image>().sprite = FrameChart.instance.frameChartInfos[0].Image;
                card.Find("호칭").GetComponent<Text>().text = FrameChart.instance.frameChartInfos[0].SubName;

                if (publicDataCallback.GetReturnValuetoJSON()[0].Count == 0) // 테이블에 해당 유저의 정보가 아무것도 없는 경우
                {
                }
                else
                {
                    for (int i = 0; i < publicDataCallback.GetReturnValuetoJSON()[0].Count; i++)
                    {
                        JsonData jsonData = publicDataCallback.GetReturnValuetoJSON()[0][i];

                        // 복장
                        if (jsonData.Keys.Contains("Cloth"))
                        {
                            JsonData keyData = jsonData["Cloth"][0];
                            for (int j = 0; j < keyData.Count; j++)
                            {
                                string[] data = keyData[j][0].ToString().Split('/');
                                if (bool.Parse(data[3]))
                                {
                                    card.Find("프레임").Find("Icon").GetComponent<Image>().sprite = ClothChart.instance.GetClothChartInfo(data[0])[0].Icon;
                                    break;
                                }
                            }
                        }

                        // 프레임
                        if (jsonData.Keys.Contains("Frame"))
                        {
                            JsonData keyData = jsonData["Frame"][0];
                            for (int j = 0; j < keyData.Count; j++)
                            {
                                string[] data = keyData[j][0].ToString().Split('/');

                                if (bool.Parse(data[1]))
                                {
                                    card.Find("프레임").Find("frame").GetComponent<Image>().sprite = FrameChart.instance.GetFrameChartInfo(data[0]).Image;
                                    card.Find("호칭").GetComponent<Text>().text = FrameChart.instance.GetFrameChartInfo(data[0]).SubName;
                                    break;
                                }
                            }
                        }
                    }
                }
            });
        });
    }
}
