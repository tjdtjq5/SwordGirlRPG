using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEffect : MonoBehaviour
{
    public GameObject startObj;
    public Transform BG;
    public Transform START;

    IEnumerator startPlayCoroutine;

    public void StartPlay(System.Action callback)
    {
        if (startPlayCoroutine != null) StopCoroutine(startPlayCoroutine);
        startPlayCoroutine = StartPlayCoroutine(callback);
        StartCoroutine(startPlayCoroutine);
    }

    IEnumerator StartPlayCoroutine(System.Action callback)
    {
        startObj.SetActive(true);

        BG.localPosition = new Vector2(-1963f, 182f);
        START.localPosition = new Vector2(-2552f, 182f);

        BG.DOLocalMoveX(0, 0.4f);

        yield return new WaitForSeconds(0.3f);

        START.DOLocalMoveX(0, 0.2f);

        yield return new WaitForSeconds(1.2f);

        startObj.SetActive(false);
        callback();
    }
}
