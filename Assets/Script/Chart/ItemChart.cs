using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
public class ItemChart : MonoBehaviour
{
    [SerializeField] string field;
    public static ItemChart instance;
    private void Awake() { instance = this; }
    public ItemChartInfo[] itemChartInfos;
    public SpriteAtlas moneySpriteAtlas;
    public SpriteAtlas frameSpriteAtlas;

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            itemChartInfos = new ItemChartInfo[jsonData.Count];

            for (int i = 0; i < jsonData.Count; i++)
            {
                itemChartInfos[i] = new ItemChartInfo();
                JsonData rowData = jsonData[i];
                itemChartInfos[i].Name = rowData["Name"]["S"].ToString();
                itemChartInfos[i].ItemType = (ItemType)(int.Parse(rowData["ItemType"]["S"].ToString()));
                switch (itemChartInfos[i].ItemType)
                {
                    case ItemType.재화:
                        itemChartInfos[i].Image = moneySpriteAtlas.GetSprite(rowData["Image"]["S"].ToString());
                        break;
                    case ItemType.프레임:
                        itemChartInfos[i].Image = frameSpriteAtlas.GetSprite(rowData["Image"]["S"].ToString());
                        break;
                }
            }

            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class ItemChartInfo
{
    public ItemType ItemType;
    public string Name;
    public Sprite Image = null;
}

