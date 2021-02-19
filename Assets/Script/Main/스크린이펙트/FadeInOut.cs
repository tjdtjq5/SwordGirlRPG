using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public Image fadeImg;

    public void ScreenFadeInOut(System.Action callback) // 화면전환
    {
        fadeImg.gameObject.SetActive(true);
        fadeImg.color = Color.clear;
        fadeImg.DOFade(1, 0.8f).OnComplete(() => {
            fadeImg.DOFade(0, 0.8f);
            fadeImg.gameObject.SetActive(false);
            callback();
        });
    }
}
