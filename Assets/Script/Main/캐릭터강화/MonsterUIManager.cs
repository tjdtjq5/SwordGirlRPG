using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUIManager : MonoBehaviour
{
    public GameObject blackPannel;
    public GameObject pannel;
    public GameObject mainUIPannel;
    public GameObject nomalMonsterPannel;

    public Text punishTiketText;

    [Header("스크립트")]
    public NomalMonsterManager nomalMonsterManager;

    public void Open()
    {
        blackPannel.SetActive(true);
        pannel.SetActive(true);
        mainUIPannel.SetActive(true);
        nomalMonsterPannel.SetActive(false);
        UISetting();
    }
    public void Close()
    {
        if (nomalMonsterPannel.activeSelf)
        {
            nomalMonsterManager.BackBtn();
        }

        blackPannel.SetActive(false);
        pannel.SetActive(false);
    }
    void UISetting()
    {
        punishTiketText.text = "x" + UserInfo.instance.punishTiket;
    }
}
