using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEffect : MonoBehaviour
{
    public GameObject startObj;
    public Transform BG;
    public Transform START;

    public GameObject rightStarEffect;
    public GameObject leftStarEffect;

    IEnumerator startPlayCoroutine;

    [ContextMenu("Test")]
    public void Test()
    {
        StartPlay(() => { });
    }

    public void StartPlay(System.Action callback)
    {
        if (startPlayCoroutine != null) StopCoroutine(startPlayCoroutine);
        startPlayCoroutine = StartPlayCoroutine(callback);
        StartCoroutine(startPlayCoroutine);
    }

    IEnumerator StartPlayCoroutine(System.Action callback)
    {
        startObj.SetActive(true);

        rightStarEffect.gameObject.SetActive(false);
        leftStarEffect.gameObject.SetActive(false);

        BG.localPosition = new Vector2(-1963f, 182f);
        START.localPosition = new Vector2(-2552f, 182f);

        BG.DOLocalMoveX(0, 0.4f);

        yield return new WaitForSeconds(0.3f);

        START.DOLocalMoveX(0, 0.2f);

        yield return new WaitForSeconds(0.2f);

        rightStarEffect.gameObject.SetActive(true);
        leftStarEffect.gameObject.SetActive(true);

        rightStarEffect.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "spawn", false);
        leftStarEffect.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "spawn", false);

        yield return new WaitForSeconds(0.33f);

        rightStarEffect.GetComponent<SkeletonGraphic>().AnimationState.ClearTrack(0);
        leftStarEffect.GetComponent<SkeletonGraphic>().AnimationState.ClearTrack(0);
        rightStarEffect.gameObject.SetActive(false);
        leftStarEffect.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.67f);

        startObj.SetActive(false);
        callback();
    }
}
