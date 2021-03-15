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
    public GameObject violetObj;
    public GameObject pumpkinObj;
    [Header("화면전환")]
    public FadeInOut fadeInOut;
    public Intro intro;
    public StartEffect startEffect;
    [Header("배경")]
    public GameObject forest_BG;
    public GameObject violet_BG;
    public GameObject pumpkin_BG;
    [Header("카메라")]
    public Camera theCam;
    [Header("스크립트")]
    public Deliveryfairy deliveryfairy;
    public Violet violet;
    public Pumpkin pumpkin;
    [Header("SetOff")]
    public GameObject[] setOffList;
    [Header("UI")]
    public GameObject violet_ui;
    public GameObject pumpkin_ui;


    [ContextMenu("Test")]
    public void Test()
    {
        PumpkinPlay(1);
    }
    public void VioletPlay() // 바이올렛 시작 
    {
        SetOff();

        deliveryfairy.Stop();

        playerController.Hp_Initialized();
        playerController.Hp_UI_Setting();
        playerController.DontPlay();
        playerController.deadDelegate = violet.GameEnd;

        violetObj.transform.position = new Vector2(19, violetObj.transform.position.y);

        fadeInOut.ScreenFadeInOut(() => {

            violet.Init();

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

        violetObj.SetActive(true);
        violetObj.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "move", true);
        violetObj.transform.DOMoveX(9.5f, 1.5f);

        yield return new WaitForSeconds(1.5f);

        violetObj.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "extra", false);

        yield return new WaitForSeconds(0.8f);

        violetObj.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "wait", true);

        yield return new WaitForSeconds(0.5f);

        violet.Play();

        callback();
    }

    public void PumpkinPlay(int level)
    {
        SetOff();

        deliveryfairy.Stop();

        playerController.Hp_Initialized();
        playerController.Hp_UI_Setting();
        playerController.DontPlay();

        pumpkinObj.transform.position = new Vector2(15, pumpkinObj.transform.position.y);

        fadeInOut.ScreenFadeInOut(() => {

            // 코인트리 제거
            coinTree.SetActive(false);
            theCam.transform.position = new Vector3(2.2f, 1, -10);
            // 배경 변경
            forest_BG.gameObject.SetActive(false);
            pumpkin_BG.gameObject.SetActive(true);

            theCam.DOOrthoSize(6.5f, 0.5f);
            intro.IntroPlay(() => {
                StartCoroutine(SetPumpkin(level, () => {
                    startEffect.StartPlay(() => {
                        playerController.Play();
                    });
                }));
            });
        });
    }

    IEnumerator SetPumpkin(int level,System.Action callback)
    {
        yield return new WaitForSeconds(0.5f);

        pumpkinObj.SetActive(true);
        pumpkinObj.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "start", true);
        pumpkinObj.transform.DOMoveX(9.5f, 1.5f);

        yield return new WaitForSeconds(1.5f);

        pumpkinObj.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "wait", true);

        yield return new WaitForSeconds(0.5f);

        pumpkin.Play(level);

        callback();
    }

    public void SetLobby(System.Action callback)
    {
        fadeInOut.ScreenFadeInOut(() =>
        {
            callback();

            // 코인트리 생성 , 몬스터 제거 
            coinTree.SetActive(true);
            violetObj.SetActive(false);
            pumpkinObj.SetActive(false);

            // 배경 변경
            forest_BG.gameObject.SetActive(true);
            violet_BG.gameObject.SetActive(false);
            pumpkin_BG.gameObject.SetActive(false);

            // 카메라이동 
            theCam.DOOrthoSize(5f, 0);
            theCam.transform.position = new Vector3(0, 0, -10);

            // 택배요정 재시작 
            deliveryfairy.Init();

            // 유아이 보이기
            SetOn();

            // 유아이 제거하기 
            violet_ui.SetActive(false);
            pumpkin_ui.SetActive(false);
        });
    }

    void SetOff()
    {
        for (int i = 0; i < setOffList.Length; i++)
        {
            setOffList[i].SetActive(false);
        }
    }
    void SetOn()
    {
        for (int i = 0; i < setOffList.Length; i++)
        {
            setOffList[i].SetActive(true);
        }
    }
}
