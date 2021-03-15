using DG.Tweening;
using Function;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pumpkin : Enemy
{
    bool playFlag = false;
    int level = 0;
    float time = 30;
    float currentTime = 0;
    bool hitAniFlag = false;
    float hitAniColltime = 1.8f;

    public SkeletonAnimation ani;
   
    [Header("UI")]
    public GameObject pumpkin_UI;
    public Text level_text;
    public Image hp_fore;
    public Image time_fore;

    [Header("게임 클리어_UI")]
    public GameObject clear_UI;


    IEnumerator timeCoroutine;



    public void Play(int level)
    {
        this.level = level;
        playFlag = true;

        // 능력치 초기화 
        Init();

        // ui셋팅
        UI_Setting();
    }

    public void Init() // 능력치 초기화 
    {
        PumpkinChartInfo pumpkinChartInfo = PumpkinChart.instance.GetPumpkinInfo(level);
        if (pumpkinChartInfo != null)
        {
            maxHp = pumpkinChartInfo.Hp;
            hp = maxHp;
            currentTime = 0;
            hitAniFlag = false;
        }
        else
        {
            Debug.Log("호박마녀 데이터 없음");
        }
    }

    public void UI_Setting()
    {
        pumpkin_UI.SetActive(true);

        hp_fore.fillAmount = 0;
        time_fore.fillAmount = 0;
        level_text.text = "Level " + string.Format("{0:D2}", level);

        pumpkin_UI.transform.localPosition = new Vector2(0, 289);
        pumpkin_UI.transform.DOLocalMoveY(-12, 0.5f).OnComplete(() => {
            HP_Setting();
            Time_Start();
        });
    }

    void HP_Setting()
    {
        float fillAmount = MyMath.Amount(hp, maxHp);
        hp_fore.DOFillAmount(fillAmount, 0.3f);
    }
    void Time_Start()
    {
        time_fore.DOFillAmount(1,0.3f);

        if (timeCoroutine != null) StopCoroutine(timeCoroutine);
        timeCoroutine = TimeCoroutine();
        StartCoroutine(timeCoroutine);
    }
    IEnumerator TimeCoroutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);
        while (currentTime < time)
        {
            yield return waitTime;
            currentTime += 0.1f;
            time_fore.DOFillAmount( 1 - (currentTime / time), 0.3f);
        }
        time_fore.DOFillAmount(0, 0.3f);

        // 게임오버

    }
    void Time_Stop()
    {
        if (timeCoroutine != null) StopCoroutine(timeCoroutine);
    }

    public override void Hit(string damage, bool isCritical)
    {
        if (!playFlag) return;

        base.Hit(damage, isCritical);
        HP_Setting();

        if (!hitAniFlag)
        {
            hitAniFlag = true;
            StartCoroutine(HitAniColltimeCoroutine());
            ani.AnimationState.SetAnimation(0, "damage", false);
            ani.AnimationState.AddAnimation(0, "wait", true, 0);
        }
    }

    IEnumerator HitAniColltimeCoroutine()
    {
        yield return new WaitForSeconds(hitAniColltime);
        hitAniFlag = false;
    }

    public override void Dead()
    {
        base.Dead();

        Time_Stop();

        // 게임 클리어
    }

    void GameClear()
    {
        playFlag = false;

        clear_UI.SetActive(true);
    }

    void GameOver()
    {
        playFlag = false;
    }
}
