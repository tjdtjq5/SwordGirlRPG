using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public GameObject introObj;
    public Transform BG;
    public Transform Grory;
    public Transform For;
    public Transform MoMo;
    public Transform Kingdom;

    IEnumerator introPlayCoroutine;

    [ContextMenu("테스트")]
    void Test()
    {
        IntroPlay(() => { });
    }

    public void IntroPlay(System.Action callback)
    {
        if (introPlayCoroutine != null) StopCoroutine(introPlayCoroutine);
        introPlayCoroutine = IntroPlayCoroutine(callback);
        StartCoroutine(introPlayCoroutine);
    }

    IEnumerator IntroPlayCoroutine(System.Action callback)
    {
        introObj.SetActive(true);

        BG.localPosition = new Vector2(-1963f, 182f);
        Grory.localPosition = new Vector2(-2552f, 182f);
        For.localPosition = new Vector2(-2217f, 182f);
        MoMo.localPosition = new Vector2(-1883f, 182f);
        Kingdom.localPosition = new Vector2(-1429f, 182f);

        BG.DOLocalMoveX(0, 0.4f);

        yield return new WaitForSeconds(0.3f);

        Grory.DOLocalMoveX(-589, 0.2f);
        For.DOLocalMoveX(-254, 0.3f);
        MoMo.DOLocalMoveX(78, 0.4f);
        Kingdom.DOLocalMoveX(534, 0.5f);

        yield return new WaitForSeconds(1.2f);

        introObj.SetActive(false);
        callback();
    }
}
