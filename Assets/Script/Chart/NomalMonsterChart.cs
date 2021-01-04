using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class NomalMonsterChart : MonoBehaviour
{
    [SerializeField] string field;
    public static NomalMonsterChart instance;
    private void Awake() { instance = this; }
    public SpriteAtlas nomalMonsterSpriteAtlas;
    public NomalMonsterChartInfo[] nomalMonsterChartInfos;

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            nomalMonsterChartInfos = new NomalMonsterChartInfo[jsonData.Count];

            for (int i = 0; i < jsonData.Count; i++)
            {
                nomalMonsterChartInfos[i] = new NomalMonsterChartInfo();
                JsonData rowData = jsonData[i];
                nomalMonsterChartInfos[i].Name = rowData["Name"]["S"].ToString();
                nomalMonsterChartInfos[i].Image = nomalMonsterSpriteAtlas.GetSprite(rowData["Image"]["S"].ToString());
                nomalMonsterChartInfos[i].Hp = rowData["Hp"]["S"].ToString();
                nomalMonsterChartInfos[i].Atk = rowData["Atk"]["S"].ToString();
                nomalMonsterChartInfos[i].RewardCrystalNum = int.Parse(rowData["RewardCrystalNum"]["S"].ToString());
                nomalMonsterChartInfos[i].RewardEnhanceStoneNum = int.Parse(rowData["RewardEnhanceStoneNum"]["S"].ToString());
            }

            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class NomalMonsterChartInfo
{
    public string Name;
    public Sprite Image;
    public string Hp;
    public string Atk;
    public int RewardCrystalNum;
    public int RewardEnhanceStoneNum;
}