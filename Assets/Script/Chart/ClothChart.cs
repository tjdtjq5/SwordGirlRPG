using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ClothChart : MonoBehaviour
{
    [SerializeField] string field;
    public static ClothChart instance;
    private void Awake() { instance = this; }
    public SpriteAtlas clothSpriteAtlas;
    public SpriteAtlas clothIconSpriteAtlas;
    public ClothChartInfo[] clothChartInfos;

    public List<string> ClothType()
    {
        List<string> name = new List<string>();
        for (int i = 0; i < clothChartInfos.Length; i++)
        {
            if (!name.Contains(clothChartInfos[i].Name))
            {
                name.Add(clothChartInfos[i].Name);
            }
        }
        return name;
    } // 모든 초기 복장정보
    public List<string> ClothType(GradeType gradeType) // 등급별 초기 복장정보
    {
        List<string> temp = new List<string>();
        for (int i = 0; i < clothChartInfos.Length; i++)
        {
            if (clothChartInfos[i].Grade == gradeType && !temp.Contains(clothChartInfos[i].Name))
            {
                temp.Add(clothChartInfos[i].Name);
            }
        }
        return temp;
    }
    public List<ClothChartInfo> GetClothChartInfo(string name)
    {
        List<ClothChartInfo> temp = new List<ClothChartInfo>();

        for (int i = 0; i < clothChartInfos.Length; i++)
        {
            if (clothChartInfos[i].Name == name)
            {
                temp.Add(clothChartInfos[i]);
            }
        }
        return temp;
    }
    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            clothChartInfos = new ClothChartInfo[jsonData.Count];

            for (int i = 0; i < jsonData.Count; i++)
            {
                clothChartInfos[i] = new ClothChartInfo();
                JsonData rowData = jsonData[i];
                clothChartInfos[i].Name = rowData["Name"]["S"].ToString();
                clothChartInfos[i].Image = clothSpriteAtlas.GetSprite(rowData["Image"]["S"].ToString());
                clothChartInfos[i].Icon = clothIconSpriteAtlas.GetSprite(rowData["Icon"]["S"].ToString());
                clothChartInfos[i].Grade = (GradeType)(int.Parse(rowData["Grade"]["S"].ToString()));
                clothChartInfos[i].HpPercent = int.Parse(rowData["HpPercent"]["S"].ToString());
                clothChartInfos[i].CombinationNum = int.Parse(rowData["CombinationNum"]["S"].ToString());
            }

            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class ClothChartInfo
{
    public string Name;
    public Sprite Image;
    public Sprite Icon;
    public GradeType Grade;
    public int HpPercent;
    public int CombinationNum;
}