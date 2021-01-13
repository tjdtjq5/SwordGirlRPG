using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuestManager : MonoBehaviour
{
    public GameObject blackPannel;
    public GameObject questPannel;

    [Header("버튼ui")]
    public Transform pass;
    public Transform quest;
    public Transform extraQuest;

    public float upperBtnDgSpeed;

    public Sprite aUpperBtnSprite;
    public Sprite bUpperBtnSprite;

    public Transform focus;

    private void Start()
    {
        QuestBtn();
    }

    public void Open()
    {
        blackPannel.SetActive(true);
        questPannel.SetActive(true);
    }
    public void Close()
    {
        blackPannel.SetActive(false);
        questPannel.SetActive(false);
    }

    public void PassBtn()
    {
        pass.Find("Btn").GetComponent<Image>().sprite = aUpperBtnSprite;
        quest.Find("Btn").GetComponent<Image>().sprite = bUpperBtnSprite;
        extraQuest.Find("Btn").GetComponent<Image>().sprite = bUpperBtnSprite;

        float moveX = pass.Find("Position").position.x;
        focus.DOMoveX(moveX, upperBtnDgSpeed);
    }
    public void QuestBtn()
    {
        pass.Find("Btn").GetComponent<Image>().sprite = bUpperBtnSprite;
        quest.Find("Btn").GetComponent<Image>().sprite = aUpperBtnSprite;
        extraQuest.Find("Btn").GetComponent<Image>().sprite = bUpperBtnSprite;

        float moveX = quest.Find("Position").position.x;
        focus.DOMoveX(moveX, upperBtnDgSpeed);

        questPannel.GetComponent<DayByQuestManager>().Open();
    }
    public void ExtraBtn()
    {
        pass.Find("Btn").GetComponent<Image>().sprite = bUpperBtnSprite;
        quest.Find("Btn").GetComponent<Image>().sprite = bUpperBtnSprite;
        extraQuest.Find("Btn").GetComponent<Image>().sprite = aUpperBtnSprite;

        float moveX = extraQuest.Find("Position").position.x;
        focus.DOMoveX(moveX, upperBtnDgSpeed);
    }
}
