using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject blackPannel;
    public GameObject popup;

    [Header("Upper")]
    public float upperBtnDgSpeed;
    public Color selectUpperTextColor;
    public Color nonSelectUpperTextColor;
    public Color selectUpperTextOutLineColor;
    public Color nonSelectUpperTextOutLineColor;
    public int selectUpperTextSize;
    public int nonSelectUpperTextSize;
    public Transform packageBtn;
    public Outline[] packageOutLineList;
    public Transform clothBtn;
    public Outline[] clothOutLineList;
    public Transform crystalBtn;
    public Outline[] crystalOutLineList;
    public Transform groceryBtn;
    public Outline[] groceryOutLineList;
    public Transform focus;
    public Transform packageFocusPosition;
    public Transform clothFocusPosition;
    public Transform crystalFocusPosition;
    public Transform groceryFocusPosition;
    bool upperFlag = false;

    public void Open()
    {
        blackPannel.SetActive(true);
        popup.SetActive(true);
        UpperInit();
        Package();
    }
    public void Close()
    {
        blackPannel.SetActive(false);
        popup.SetActive(false);
    }

    void UpperInit()
    {
        packageBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(nonSelectUpperTextColor, 0);
        packageBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = nonSelectUpperTextSize;
        for (int i = 0; i < packageOutLineList.Length; i++)
        {
            packageOutLineList[i].DOColor(nonSelectUpperTextOutLineColor, 0);
        }

        clothBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(nonSelectUpperTextColor, 0);
        clothBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = nonSelectUpperTextSize;
        for (int i = 0; i < clothOutLineList.Length; i++)
        {
            clothOutLineList[i].DOColor(nonSelectUpperTextOutLineColor, 0);
        }

        crystalBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(nonSelectUpperTextColor, 0);
        crystalBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = nonSelectUpperTextSize;
        for (int i = 0; i < crystalOutLineList.Length; i++)
        {
            crystalOutLineList[i].DOColor(nonSelectUpperTextOutLineColor, 0);
        }

        groceryBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(nonSelectUpperTextColor, 0);
        groceryBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = nonSelectUpperTextSize;
        for (int i = 0; i < groceryOutLineList.Length; i++)
        {
            groceryOutLineList[i].DOColor(nonSelectUpperTextOutLineColor, 0);
        }
    }

    public void Package()
    {
        if (upperFlag) return;
        StartCoroutine(UpperFlagCoroutine());

        UpperInit();
        packageBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(selectUpperTextColor, upperBtnDgSpeed);
        packageBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = selectUpperTextSize;
        for (int i = 0; i < packageOutLineList.Length; i++)
        {
            packageOutLineList[i].DOColor(selectUpperTextOutLineColor, upperBtnDgSpeed);
        }

        focus.DOMoveX(packageFocusPosition.position.x, upperBtnDgSpeed);
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
    }
    public void Crystal()
    {
        if (upperFlag) return;
        StartCoroutine(UpperFlagCoroutine());

        UpperInit();
        crystalBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(selectUpperTextColor, upperBtnDgSpeed);
        crystalBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = selectUpperTextSize;
        for (int i = 0; i < crystalOutLineList.Length; i++)
        {
            crystalOutLineList[i].DOColor(selectUpperTextOutLineColor, upperBtnDgSpeed);
        }

        focus.DOMoveX(crystalFocusPosition.position.x, upperBtnDgSpeed);
    }
    public void Grocery()
    {
        if (upperFlag) return;
        StartCoroutine(UpperFlagCoroutine());

        UpperInit();
        groceryBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(selectUpperTextColor, upperBtnDgSpeed);
        groceryBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = selectUpperTextSize;
        for (int i = 0; i < groceryOutLineList.Length; i++)
        {
            groceryOutLineList[i].DOColor(selectUpperTextOutLineColor, upperBtnDgSpeed);
        }

        focus.DOMoveX(groceryFocusPosition.position.x, upperBtnDgSpeed);
    }

    IEnumerator UpperFlagCoroutine()
    {
        upperFlag = true;
        yield return new WaitForSeconds(upperBtnDgSpeed);
        upperFlag = false;
    }
}
