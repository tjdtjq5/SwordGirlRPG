using BackEnd;
using Function;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    public RankingInfo myRankingInfo;
    public List<RankingInfo> rankingInfoList = new List<RankingInfo>();
    string violetRankingUUID = "9dd4bff0-7df4-11eb-86bb-21788bad5d71";

    int damageLength = 8;

    [ContextMenu("test")]
    public void Test()
    {
        GetVioletRanking(5,0,() => {

            for (int i = 0; i < rankingInfoList.Count; i++)
            {
                Debug.Log("닉네임 :" + rankingInfoList[i].nickname);
                Debug.Log("등수 :" + rankingInfoList[i].rank);
            }
        });
    }

    public int DamageToScore(string damage)
    {
        int unit = damage.Length * (int)(Mathf.Pow(10, damageLength));
        if (damage.Length > damageLength)
        {
            damage = damage.Substring(0, damageLength);
        }
        int data = int.Parse(damage);
        int score = unit + data;
        return score;
    }
    public string ScoreToDamage(int score)
    {
        int data = score % (int)(Mathf.Pow(10, damageLength));
        int unit = score / (int)(Mathf.Pow(10, damageLength));

        string damage = data.ToString();
        unit -= damage.Length;
        if (unit < 0) unit = 0;

        for (int i = 0; i < unit; i++)
        {
            damage = MyMath.Multiple(damage, 10);
        }

        return damage;
    }
    public void VioletScoreRegister(string damage)
    {
        int score = DamageToScore(damage);

        Param param = new Param();
        param.Add("Score", score);

        BackendGameInfo.instance.GetPrivateContents("Violet", "inDate", () => {
            string inDate = BackendGameInfo.instance.serverDataList[0];
            BackendAsyncClass.BackendAsync(Backend.GameInfo.UpdateRTRankTable, "Violet", "Score", inDate, score, (updateRTRankTableCallback) => {
                switch (updateRTRankTableCallback.GetErrorCode())
                {
                    case "ForbiddenException":
                        Debug.Log("콘솔에서 실시간 랭킹을 활성화 하지 않고 갱신 요청을 한 경우");
                        return;
                    case "BadRankData":
                        Debug.Log("콘솔에서 실시간 랭킹을 생성하지 않고 갱신 요청을 한 경우");
                        Debug.Log("콘솔에서 Public 테이블로 실시간 랭킹을 생성한 경우");
                        Debug.Log("테이블 명 혹은 colum명이 존재하지 않는 경우");
                        return;
                    case "PreConditionError":
                        Debug.Log("한국시간(UTC+9) 4시 ~ 5시 사이에 실시간 랭킹 갱신 요청을 한 경우");
                        return;
                    case "ForbiddenError":
                        Debug.Log("퍼블릭테이블의 타인정보를 수정하고자 하였을 경우");
                        return;
                    case "NotFoundException":
                        Debug.Log("존재하지 않는 tableName인 경우");
                        return;
                    case "PreconditionFailed":
                        Debug.Log("비활성화 된 tableName인 경우");
                        return;
                }
            });
        });
    }
    public void GetVioletRanking(int count, int offset,System.Action sucess)
    {
        if (count + offset > 100) { Debug.Log("랭킹 100명까지만 불러올수 있습니다."); return; }

        rankingInfoList.Clear();

        BackendAsyncClass.BackendAsync(Backend.RTRank.GetRTRankByUuid, violetRankingUUID, 100, (rankingByUuidCallback) => {

            switch (rankingByUuidCallback.GetStatusCode())
            {
                case "404":
                    Debug.Log("랭킹 uuid가 틀린 경우");
                    break;
            }

            JsonData jsonData = rankingByUuidCallback.GetReturnValuetoJSON()["rows"];
            for (int i = 0; i < jsonData.Count; i++)
            {
                string nickname = jsonData[i]["nickname"].ToString();
                int score = int.Parse(jsonData[i]["score"]["N"].ToString());
                int rank = int.Parse(jsonData[i]["rank"]["N"].ToString());

                rankingInfoList.Add(new RankingInfo(nickname, rank, ScoreToDamage(score)));
            }

            if (sucess != null) sucess();
        });
    }
    public void GetMyVioletRanking(System.Action sucess)
    {
        myRankingInfo = null;

        BackendAsyncClass.BackendAsync(Backend.RTRank.GetMyRTRank, violetRankingUUID, (rankingByUuidCallback) => {

            switch (rankingByUuidCallback.GetStatusCode())
            {
                case "404":
                    Debug.Log("게이머가 랭킹에 없는 경우");
                    return;
            }
            JsonData jsonData = rankingByUuidCallback.GetReturnValuetoJSON()["rows"];

            string nickname = jsonData[0]["nickname"].ToString();
            int intScore = int.Parse(jsonData[0]["score"]["N"].ToString());
            string stringScore = ScoreToDamage(intScore);
            int rank = int.Parse(jsonData[0]["rank"]["N"].ToString());

            myRankingInfo = new RankingInfo(nickname, rank, stringScore);

            sucess();
        });
    }
}
public class RankingInfo
{
    public string nickname;
    public int rank;
    public int intScore;
    public string stringScore;

    public RankingInfo(string nickname, int rank, int intScore)
    {
        this.nickname = nickname;
        this.rank = rank;
        this.intScore = intScore;
    }
    public RankingInfo(string nickname, int rank, string stringScore)
    {
        this.nickname = nickname;
        this.rank = rank;
        this.stringScore = stringScore;
    }
}

