using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BossMonsterChart : MonoBehaviour
{
    [SerializeField] string field;
    public static BossMonsterChart instance;
    private void Awake() { instance = this; }
    public SpriteAtlas bossMonsterSpriteAtlas;
    public BossMonsterChartInfo[] bossMonsterChartInfos;

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            bossMonsterChartInfos = new BossMonsterChartInfo[jsonData.Count];

            for (int i = 0; i < jsonData.Count; i++)
            {
                bossMonsterChartInfos[i] = new BossMonsterChartInfo();
                JsonData rowData = jsonData[i];
                bossMonsterChartInfos[i].Name = rowData["Name"]["S"].ToString();
                bossMonsterChartInfos[i].Image = bossMonsterSpriteAtlas.GetSprite(rowData["Image"]["S"].ToString());
                bossMonsterChartInfos[i].Hp = rowData["Hp"]["S"].ToString();
                bossMonsterChartInfos[i].Atk = rowData["Atk"]["S"].ToString();
                bossMonsterChartInfos[i].RewardCrystalNum = int.Parse(rowData["RewardCrystalNum"]["S"].ToString());
                bossMonsterChartInfos[i].RewardEnhanceStoneNum = int.Parse(rowData["RewardEnhanceStoneNum"]["S"].ToString());
            }

            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class BossMonsterChartInfo
{
    public string Name;
    public Sprite Image;
    public string Hp;
    public string Atk;
    public int RewardCrystalNum;
    public int RewardEnhanceStoneNum;
}