using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class MasicMissileChart : MonoBehaviour
{
    [SerializeField] string field;
    public static MasicMissileChart instance;
    private void Awake() { instance = this; }
    public SpriteAtlas masicMissileSpriteAtlas;
    public MasicMissileChartInfo[] masicMissileChartInfos;

    public List<string> MasicMissileType()
    {
        List<string> name = new List<string>();
        for (int i = 0; i < masicMissileChartInfos.Length; i++)
        {
            if (!name.Contains(masicMissileChartInfos[i].Name))
            {
                name.Add(masicMissileChartInfos[i].Name);
            }
        }
        return name;
    } // 모든 초기 미사일 정보 
    public List<string> MasicMissileType(GradeType gradeType) // 등급별 초기 미사일 정보
    {
        List<string> temp = new List<string>();
        for (int i = 0; i < masicMissileChartInfos.Length; i++)
        {
            if (masicMissileChartInfos[i].GradeType == gradeType && !temp.Contains(masicMissileChartInfos[i].Name))
            {
                temp.Add(masicMissileChartInfos[i].Name);
            }
        }
        return temp;
    }

    public List<MasicMissileChartInfo> GetMasicMissileInfo(string name)
    {
        List<MasicMissileChartInfo> masicMissileList = new List<MasicMissileChartInfo>();
        for (int i = 0; i < masicMissileChartInfos.Length; i++)
        {
            if (masicMissileChartInfos[i].Name == name)
            {
                masicMissileList.Add(masicMissileChartInfos[i]);
            }
        }
        return masicMissileList;
    }

    [System.Obsolete]
    public void LoadChart(System.Action loadAction)
    {
        BackendAsyncClass.BackendAsync(Backend.Chart.GetChartContents, field, (backendCallback) => {
            JsonData jsonData = backendCallback.GetReturnValuetoJSON()["rows"];
            masicMissileChartInfos = new MasicMissileChartInfo[jsonData.Count];

            for (int i = 0; i < jsonData.Count; i++)
            {
                masicMissileChartInfos[i] = new MasicMissileChartInfo();
                JsonData rowData = jsonData[i];
                masicMissileChartInfos[i].Name = rowData["Name"]["S"].ToString();
                masicMissileChartInfos[i].Image = masicMissileSpriteAtlas.GetSprite(rowData["Image"]["S"].ToString());
                masicMissileChartInfos[i].GradeType = (GradeType)(int.Parse(rowData["GradeType"]["S"].ToString()));
                masicMissileChartInfos[i].AbilityType = (AbilityType)(int.Parse(rowData["AbilityType"]["S"].ToString()));
                masicMissileChartInfos[i].AbilityNum = int.Parse(rowData["AbilityNum"]["S"].ToString());
                masicMissileChartInfos[i].ConbinationNum = int.Parse(rowData["ConbinationNum"]["S"].ToString());
            }

            if (loadAction != null) loadAction();
        });
    }
}
[System.Serializable]
public class MasicMissileChartInfo
{
    public string Name;
    public Sprite Image;
    public GradeType GradeType;
    public AbilityType AbilityType;
    public int AbilityNum;
    public int ConbinationNum;
}