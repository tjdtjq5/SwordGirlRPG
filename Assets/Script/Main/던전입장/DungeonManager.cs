using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    [Header("캐릭터")]
    public PlayerController playerController;
    public GameObject coinTree;
    public GameObject violet;
    [Header("화면전환")]
    public FadeInOut fadeInOut;
    public Intro intro;
    public StartEffect startEffect;
    [Header("배경")]
    public GameObject forest_BG;
    public GameObject violet_BG;
    [Header("카메라")]
    public Camera theCam;

    [ContextMenu("Test")]
    public void Test()
    {
        theCam.DOOrthoSize(5f, 0);
        theCam.transform.position = new Vector3(0, 0, -10);
        VioletPlay();
    }
    public void VioletPlay() // 바이올렛 시작 
    {
        playerController.DontPlay();
      

        fadeInOut.ScreenFadeInOut(() => {

            // 코인트리 제거
            coinTree.SetActive(false);
            theCam.transform.position = new Vector3(2.2f, 1, -10);
            // 배경 변경
            forest_BG.gameObject.SetActive(false);
            violet_BG.gameObject.SetActive(true);

            theCam.DOOrthoSize(6.5f, 0.5f);
            intro.IntroPlay(() => {
                StartCoroutine(SetViolet(() => {
                    startEffect.StartPlay(() => {
                        playerController.Play();
                    });
                }));
            });
       
        });
    }

    IEnumerator SetViolet(System.Action callback)
    {
        yield return new WaitForSeconds(0.5f);

        violet.SetActive(true);
        violet.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "move", true);
        violet.transform.position = new Vector2(15, -2.91f);
        violet.transform.DOMoveX(8.5f, 1.5f);

        yield return new WaitForSeconds(1.5f);

        violet.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "extra", false);

        yield return new WaitForSeconds(0.8f);

        violet.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "wait", true);

        yield return new WaitForSeconds(0.5f);

        violet.GetComponent<Violet>().Play();

        callback();
    }
}
