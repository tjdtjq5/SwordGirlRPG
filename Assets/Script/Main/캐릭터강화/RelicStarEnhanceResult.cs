using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class RelicStarEnhanceResult : MonoBehaviour
{
    public GameObject balckPannel;
    public GameObject resultPannel;

    public Transform before;
    public Transform after;

    public Button okBtn;

    public SpriteAtlas gradeSpriteAtlas;

    public void Open(string relicName)
    {
        balckPannel.SetActive(true);
        resultPannel.SetActive(true);

        UserRelic userRelic = UserInfo.instance.GetUserRelicInfo(relicName);
        RelicChartInfo beforeRelic = RelicChart.instance.GetRelicChartInfo(relicName)[(int)userRelic.gradeType - 1];
        RelicChartInfo afterRelic = RelicChart.instance.GetRelicChartInfo(relicName)[(int)userRelic.gradeType];
        before.Find("아이콘").Find("Icon").GetComponent<Image>().sprite = beforeRelic.Image;
        before.Find("아이콘").Find("별등급").GetComponent<Image>().sprite = gradeSpriteAtlas.GetSprite(beforeRelic.GradeType.ToString());
        before.Find("아이콘").Find("별등급").GetComponent<Image>().SetNativeSize();
        before.Find("능력치").GetComponent<Text>().text = "<color=#5DFF2F>" + beforeRelic.EnhanceAbilityType.ToString() + " " + (userRelic.upgrade + 1) * beforeRelic.EnhanceAbilityIncrease + "%</color>\n" +
                                                                "<color=#FF9200>" + beforeRelic.StarEnhanceAbilityType.ToString() + " " + beforeRelic.StarEnhanceAbilityIncrease + "%</color>";

        after.Find("아이콘").Find("Icon").GetComponent<Image>().sprite = afterRelic.Image;
        after.Find("아이콘").Find("별등급").GetComponent<Image>().sprite = gradeSpriteAtlas.GetSprite(afterRelic.GradeType.ToString());
        after.Find("아이콘").Find("별등급").GetComponent<Image>().SetNativeSize();
        after.Find("능력치").GetComponent<Text>().text = "<color=#5DFF2F>" + afterRelic.EnhanceAbilityType.ToString() + " " + (userRelic.upgrade + 1) * afterRelic.EnhanceAbilityIncrease + "%</color>\n" +
                                                                "<color=#FF9200>" + afterRelic.StarEnhanceAbilityType.ToString() + " " + afterRelic.StarEnhanceAbilityIncrease + "%</color>";
    }

    public void Close()
    {
        balckPannel.SetActive(false);
        resultPannel.SetActive(false);
    }
}
