using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EqipManager : MonoBehaviour
{
    public GameObject blackPannel;
    public GameObject popup;
    [Header("스크립트")]
    public WeaponeManager weaponeManager;
    public RelicManager relicManager;
    public ClothManager clothManager;

    [Header("Upper")]
    public float upperBtnDgSpeed;
    public Color selectUpperTextColor;
    public Color nonSelectUpperTextColor;
    public Color selectUpperTextOutLineColor;
    public Color nonSelectUpperTextOutLineColor;
    public int selectUpperTextSize;
    public int nonSelectUpperTextSize;
    public Transform weaponeBtn;
    public Outline[] weaponeOutLineList;
    public Transform accBtn;
    public Outline[] accOutLineList;
    public Transform relicsBtn;
    public Outline[] relicsOutLineList;
    public Transform clothBtn;
    public Outline[] clothOutLineList;
    public Transform focus;
    public Transform weaponeFocusPosition;
    public Transform accFocusPosition;
    public Transform relicsFocusPosition;
    public Transform clothFocusPosition;
    bool upperFlag = false;
    [Header("Middle")]
    public GameObject weaponeObj;
    public GameObject relicObj;
    public GameObject clothObj;

    public void Open()
    {
        blackPannel.SetActive(true);
        popup.SetActive(true);
        UpperInit();
        Weapone();
    }
    public void Close()
    {
        blackPannel.SetActive(false);
        popup.SetActive(false);
    }

    void UpperInit()
    {
        weaponeBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(nonSelectUpperTextColor, 0);
        weaponeBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = nonSelectUpperTextSize;
        for (int i = 0; i < weaponeOutLineList.Length; i++)
        {
            weaponeOutLineList[i].DOColor(nonSelectUpperTextOutLineColor, 0);
        }
        weaponeManager.RedIconCheck();

        accBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(nonSelectUpperTextColor, 0);
        accBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = nonSelectUpperTextSize;
        for (int i = 0; i < accOutLineList.Length; i++)
        {
            accOutLineList[i].DOColor(nonSelectUpperTextOutLineColor, 0);
        }

        relicsBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(nonSelectUpperTextColor, 0);
        relicsBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = nonSelectUpperTextSize;
        for (int i = 0; i < relicsOutLineList.Length; i++)
        {
            relicsOutLineList[i].DOColor(nonSelectUpperTextOutLineColor, 0);
        }

        clothBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(nonSelectUpperTextColor, 0);
        clothBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = nonSelectUpperTextSize;
        for (int i = 0; i < clothOutLineList.Length; i++)
        {
            clothOutLineList[i].DOColor(nonSelectUpperTextOutLineColor, 0);
        }

        weaponeObj.SetActive(false);
        relicObj.SetActive(false);
        clothObj.SetActive(false);
    }

    public void Weapone()
    {
        if (upperFlag) return;
        StartCoroutine(UpperFlagCoroutine());

        UpperInit();
        weaponeBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(selectUpperTextColor, upperBtnDgSpeed);
        weaponeBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = selectUpperTextSize;
        for (int i = 0; i < weaponeOutLineList.Length; i++)
        {
            weaponeOutLineList[i].DOColor(selectUpperTextOutLineColor, upperBtnDgSpeed);
        }

        focus.DOMoveX(weaponeFocusPosition.position.x, upperBtnDgSpeed);

        weaponeObj.SetActive(true);
        weaponeManager.Open();
    }
    public void Acc()
    {
        if (upperFlag) return;
        StartCoroutine(UpperFlagCoroutine());

        UpperInit();
        accBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(selectUpperTextColor, upperBtnDgSpeed);
        accBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = selectUpperTextSize;
        for (int i = 0; i < accOutLineList.Length; i++)
        {
            accOutLineList[i].DOColor(selectUpperTextOutLineColor, upperBtnDgSpeed);
        }
        focus.DOMoveX(accFocusPosition.position.x, upperBtnDgSpeed);
    }
    public void Relics()
    {
        if (upperFlag) return;
        StartCoroutine(UpperFlagCoroutine());

        UpperInit();
        relicsBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(selectUpperTextColor, upperBtnDgSpeed);
        relicsBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = selectUpperTextSize;
        for (int i = 0; i < relicsOutLineList.Length; i++)
        {
            relicsOutLineList[i].DOColor(selectUpperTextOutLineColor, upperBtnDgSpeed);
        }
        focus.DOMoveX(relicsFocusPosition.position.x, upperBtnDgSpeed);

        relicObj.SetActive(true);
        relicManager.Open();
    }
    public void Cloth()
    {
        if (upperFlag) return;
        StartCoroutine(UpperFlagCoroutine());

        UpperInit();
        clothBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(selectUpperTextColor, upperBtnDgSpeed);
        clothBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = selectUpperTextSize;
        for (int i = 0; i < clothOutLineList.Length; i++)
        {
            clothOutLineList[i].DOColor(selectUpperTextOutLineColor, upperBtnDgSpeed);
        }
        focus.DOMoveX(clothFocusPosition.position.x, upperBtnDgSpeed);
        clothObj.SetActive(true);
        clothManager.Open();
    }
    IEnumerator UpperFlagCoroutine()
    {
        upperFlag = true;
        yield return new WaitForSeconds(upperBtnDgSpeed);
        upperFlag = false;
    }
}
