using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUIManager : MonoBehaviour
{
    public GameObject blackPannel;
    public GameObject monsterUIPannel;

    public Text punishTiketText;
    public void Open()
    {
        blackPannel.SetActive(true);
        monsterUIPannel.SetActive(true);
        UISetting();
    }
    public void Close()
    {
        blackPannel.SetActive(false);
        monsterUIPannel.SetActive(false);
    }
    void UISetting()
    {
        punishTiketText.text = "x" + UserInfo.instance.punishTiket;
    }
}
