using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class FrameChart : MonoBehaviour
{
    [SerializeField] string field;
    public static FrameChart instance;
    private void Awake() { instance = this; }
    public FrameChartInfo[] frameChartInfos;
    public SpriteAtlas frameSpriteAtlas;

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            frameChartInfos = new FrameChartInfo[jsonData.Count];

            for (int i = 0; i < jsonData.Count; i++)
            {
                frameChartInfos[i] = new FrameChartInfo();
                JsonData rowData = jsonData[i];
                frameChartInfos[i].Image = frameSpriteAtlas.GetSprite(rowData["Image"]["S"].ToString());
                frameChartInfos[i].SubName = rowData["SubName"]["S"].ToString();
                frameChartInfos[i].AbilityType = (AbilityType)int.Parse(rowData["AbilityType"]["S"].ToString());
                frameChartInfos[i].AbilityCount = float.Parse(rowData["AbilityCount"]["S"].ToString());
            }

            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class FrameChartInfo
{
    public Sprite Image;
    public string SubName;
    public AbilityType AbilityType;
    public float AbilityCount;
}