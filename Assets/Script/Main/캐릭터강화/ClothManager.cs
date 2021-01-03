using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothManager : MonoBehaviour
{
    public GameObject card;
    public Transform content;
    public void Open()
    {
        Initialized();
        UISetting();
    }
    void Initialized() // 카드 생성 
    {
        int len = ClothChart.instance.ClothType().Count;

        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        for (int i = 0; i < len; i++)
        {
            Instantiate(card, Vector3.zero, Quaternion.identity, content);
        }

        content.GetComponent<RectTransform>().sizeDelta = new Vector2((398 * len), content.GetComponent<RectTransform>().sizeDelta.y);
    }
    void UISetting()
    {
        int len = ClothChart.instance.ClothType().Count;
        int userLen = UserInfo.instance.userCloths.Count;

        // 유저가 가지고 있는 카드 정보 
        for (int i = 0; i < userLen; i++)
        {
            UserCloth userCloth = UserInfo.instance.userCloths[i];
            string name = userCloth.name;
            int upgrade = userCloth.upgrade;
            int num = userCloth.num;
            bool isEqip = userCloth.isEqip;
            ClothChartInfo clothChartInfo = ClothChart.instance.GetClothChartInfo(name)[upgrade];
        }

        // 가지고 있지 않는 카드 정보 
        int count = userLen;
        for (int i = 0; i < len; i++)
        {
            string name = ClothChart.instance.ClothType()[i];
            UserCloth userCloth = UserInfo.instance.GetUserClothInfo(name);
            if (userCloth == null)
            {

            }
        }
    }
}
