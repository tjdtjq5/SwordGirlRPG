using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CharacterEnhaceManager : MonoBehaviour
{
    public GameObject blackPannel;
    public GameObject popup;
    [Header("스크립트")]
    public AbilityManager abilityManager;

    [Header("Upper")]
    public float upperBtnDgSpeed;
    public Color selectUpperTextColor;
    public Color nonSelectUpperTextColor;
    public Color selectUpperTextOutLineColor;
    public Color nonSelectUpperTextOutLineColor;
    public int selectUpperTextSize;
    public int nonSelectUpperTextSize;
    public Transform physicalBtn;
    public Outline[] physicalOutLineList;
    public Transform skillBtn;
    public Outline[] skillOutLineList;
    public Transform masicMissileBtn;
    public Outline[] masicMissileOutLineList;
    public Transform soulBtn;
    public Outline[] soulOutLineList;
    public Transform focus;
    public Transform physicalFocusPosition;
    public Transform skillFocusPosition;
    public Transform masicMissileFocusPosition;
    public Transform soulFocusPosition;
    bool upperFlag = false;
    [Header("Middle")]
    public GameObject physicalPannel;
    public GameObject masicMissilePannel;

    public void Open()
    {
        blackPannel.SetActive(true);
        popup.SetActive(true);
        UpperInit();
        Physical();
    }
    public void Close()
    {
        blackPannel.SetActive(false);
        popup.SetActive(false);
    }

    void UpperInit()
    {
        physicalBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(nonSelectUpperTextColor,0);
        physicalBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = nonSelectUpperTextSize;
        for (int i = 0; i < physicalOutLineList.Length; i++)
        {
            physicalOutLineList[i].DOColor(nonSelectUpperTextOutLineColor, 0);
        }

        skillBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(nonSelectUpperTextColor, 0);
        skillBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = nonSelectUpperTextSize;
        for (int i = 0; i < skillOutLineList.Length; i++)
        {
            skillOutLineList[i].DOColor(nonSelectUpperTextOutLineColor, 0);
        }

        masicMissileBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(nonSelectUpperTextColor, 0);
        masicMissileBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = nonSelectUpperTextSize;
        for (int i = 0; i < masicMissileOutLineList.Length; i++)
        {
            masicMissileOutLineList[i].DOColor(nonSelectUpperTextOutLineColor, 0);
        }

        soulBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(nonSelectUpperTextColor, 0);
        soulBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = nonSelectUpperTextSize;
        for (int i = 0; i < soulOutLineList.Length; i++)
        {
            soulOutLineList[i].DOColor(nonSelectUpperTextOutLineColor, 0);
        }

        physicalPannel.SetActive(false);
        masicMissilePannel.SetActive(false);
    }

    public void Physical()
    {
        if (upperFlag) return;
        StartCoroutine(UpperFlagCoroutine());

        UpperInit();
        physicalBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(selectUpperTextColor, upperBtnDgSpeed);
        physicalBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = selectUpperTextSize;
        for (int i = 0; i < physicalOutLineList.Length; i++)
        {
            physicalOutLineList[i].DOColor(selectUpperTextOutLineColor, upperBtnDgSpeed);
        }

        focus.DOMoveX(physicalFocusPosition.position.x, upperBtnDgSpeed);

        physicalPannel.SetActive(true);
        abilityManager.Init();
    }
    public void Skill()
    {
        if (upperFlag) return;
        StartCoroutine(UpperFlagCoroutine());

        UpperInit();
        skillBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(selectUpperTextColor, upperBtnDgSpeed);
        skillBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = selectUpperTextSize;
        for (int i = 0; i < physicalOutLineList.Length; i++)
        {
            skillOutLineList[i].DOColor(selectUpperTextOutLineColor, upperBtnDgSpeed);
        }
        focus.DOMoveX(skillFocusPosition.position.x, upperBtnDgSpeed);
    }
    public void MasicMissile()
    {
        if (upperFlag) return;
        StartCoroutine(UpperFlagCoroutine());

        UpperInit();
        masicMissileBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(selectUpperTextColor, upperBtnDgSpeed);
        masicMissileBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = selectUpperTextSize;
        for (int i = 0; i < physicalOutLineList.Length; i++)
        {
            masicMissileOutLineList[i].DOColor(selectUpperTextOutLineColor, upperBtnDgSpeed);
        }
        focus.DOMoveX(masicMissileFocusPosition.position.x, upperBtnDgSpeed);

        masicMissilePannel.SetActive(true);
    }
    public void Soul()
    {
        if (upperFlag) return;
        StartCoroutine(UpperFlagCoroutine());

        UpperInit();
        soulBtn.Find("Btn").GetChild(0).GetComponent<Text>().DOColor(selectUpperTextColor, upperBtnDgSpeed);
        soulBtn.Find("Btn").GetChild(0).GetComponent<Text>().fontSize = selectUpperTextSize;
        for (int i = 0; i < physicalOutLineList.Length; i++)
        {
            soulOutLineList[i].DOColor(selectUpperTextOutLineColor, upperBtnDgSpeed);
        }
        focus.DOMoveX(soulFocusPosition.position.x, upperBtnDgSpeed);
    }
    IEnumerator UpperFlagCoroutine()
    {
        upperFlag = true;
        yield return new WaitForSeconds(upperBtnDgSpeed);
        upperFlag = false;
    }
}
