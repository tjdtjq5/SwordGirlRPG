using DG.Tweening;
using Function;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Violet : Enemy
{
    bool playFlag = false;

    // 능력치 
    string atk;
    float atkPercent;
    float criticalPercent;
    float criticalDamage;
    float atkSpeed;

    // 축적 데미지 (점수)
    string hitDamage;

    // 스파인
    public SkeletonAnimation sa;

    // 시간 
    float playTime;
    IEnumerator timeCoroutine;

    // 공격 
    int atkCount;
    IEnumerator attackCoroutine;
    public GameObject atkEffectPrepab;

    // 플레이어
    public PlayerController playerController;
    public Transform player;

    // UI
    [Header("UI")]
    public GameObject violet_UI;
    public Text damage_text;
    public Text level_text;
    public Image reward_icon;
    public Text reward_text;
    public Image hp_fore;
    int beforeLevel = 1;
    public Animator lineEffectAni;

    [Header("스크립트")]
    public VioletResult violetResult;

    public void Play()
    {
        playFlag = true;

        // 능력치 초기화 
        Init();

        // 시간 측정 코르틴 시작 
        TimePlay();

        // 공격 코르틴 시작 
        AttackPlay();

        // ui셋팅
        UI_Setting();
    }

    public void UI_Setting()
    {
        violet_UI.SetActive(true);

        hp_fore.fillAmount = 0;
        damage_text.text = "0";
        level_text.text = "Level 00";
        reward_text.text = "0";

        violet_UI.transform.localPosition = new Vector2(0, 289);
        violet_UI.transform.DOLocalMoveY(-12, 0.5f).OnComplete(()=> {
            Hp_Setting();
        });
    }

    public void Stop()
    {
        playFlag = false;

        AttackStop();
    }

    public void Init() // 능력치 초기화 
    {
        maxHp = "0";
        hp = "0";

        atk = "100";
        atkPercent = 1.2f;
        criticalPercent = 30;
        criticalDamage = 1.4f;
        atkSpeed = 5f;
        beforeLevel = 1;

        // 시간 초기화 
        playTime = 0;

        // 공격횟수 초기화 
        atkCount = 0;

        // 축적 데미지 
        hitDamage = "0";
    }

    void TimePlay()
    {
        if (timeCoroutine != null) StopCoroutine(timeCoroutine);
        timeCoroutine = TimeCoroutine();
        StartCoroutine(timeCoroutine);
    }
    void TimeStop()
    {
        if (timeCoroutine != null) StopCoroutine(timeCoroutine);
    }
    IEnumerator TimeCoroutine()
    {
        float wt = 0.5f;
        WaitForSeconds waitTime = new WaitForSeconds(wt);
        while (true)
        {
            yield return waitTime;
            playTime += wt;
        }
    }

    void AttackPlay()
    {
        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
        attackCoroutine = AttackCoroutine();
        StartCoroutine(attackCoroutine);
    }
    void AttackStop()
    {
        if (attackCoroutine != null) StopCoroutine(attackCoroutine); ;
    }

    IEnumerator AttackCoroutine()
    {
        WaitForSeconds ws = new WaitForSeconds(atkSpeed);

        while (true)
        {
            yield return ws;

            // 공격 애니 
            sa.AnimationState.SetAnimation(0, "skill", false);
            sa.AnimationState.AddAnimation(0, "wait", true, 0);

            // 데미지 
            float r = Random.Range(0, 100);
            float atkMultiple = (atkCount + 1) * atkPercent;
            string damage = MyMath.Multiple(atk, atkMultiple);

            yield return new WaitForSeconds(2);

            GameObject effectClone = Instantiate(atkEffectPrepab, player.position, Quaternion.identity, this.transform);
            effectClone.GetComponent<Animator>().SetTrigger("attack");

            yield return new WaitForSeconds(0.5f);

            if (criticalPercent < r) // 노말 
            {
                playerController.Hit(damage, false);
            }
            else // 크리티컬
            {
                damage = MyMath.Multiple(damage, criticalDamage);
                playerController.Hit(damage, true);
            }

            // 공격횟수 
            atkCount++;
        }
    }

    void Hp_Setting()
    {
        damage_text.text = MyMath.ValueToString(hitDamage);
        VioletRewardChartInfo violetRewardChartInfo = VioletRewardChart.instance.GetVioletReward(hitDamage);
        int rewardCount = violetRewardChartInfo.MasicStoneCount;
        reward_text.text = rewardCount.ToString();
        level_text.text = "Level " + string.Format("{0:D2}", violetRewardChartInfo.Level);

        string beforeD = violetRewardChartInfo.BeforeDamage;
        string d = MyMath.Sub(hitDamage, beforeD);
        string t = MyMath.Sub(violetRewardChartInfo.TotalDamage, beforeD);


        if(violetRewardChartInfo.Level != beforeLevel)
        {
            hp_fore.fillAmount = 0;

            LineEffect();

            reward_text.transform.localPosition = new Vector2(-117.5f, -0.5f);
            reward_text.transform.DOShakePosition(0.1f,5,30);
            reward_text.transform.localScale = new Vector2(1, 1);
            reward_text.transform.DOScale(new Vector2(1.3f, 1.3f), 0.05f).OnComplete(()=> {
                reward_text.transform.DOScale(new Vector2(1, 1), 0.05f);
            });

            reward_icon.transform.localPosition = new Vector2(-107, -0.5f);
            reward_icon.transform.DOShakePosition(0.1f, 5, 30);
            reward_icon.transform.localScale = new Vector2(1, 1);
            reward_icon.transform.DOScale(new Vector2(1.3f, 1.3f), 0.05f).OnComplete(() => {
                reward_icon.transform.DOScale(new Vector2(1, 1), 0.05f);
            });
        }

        float fillAmount = 1 - MyMath.Amount(d, t);
        hp_fore.DOFillAmount(fillAmount, 0.3f);

        beforeLevel = violetRewardChartInfo.Level;
    }

    void LineEffect()
    {
        lineEffectAni.SetTrigger("play");
    }


    public override void Hit(string damage, bool isCritical)
    {
        if (!playFlag) return;

        base.Hit(damage, isCritical);

        hitDamage = MyMath.Add(hitDamage, damage);

        Hp_Setting();
    }
    public override void Dead()
    {
        base.Dead();
    }

    public void GameEnd()
    {
        // 바이올렛과 플레이어의 움직임을 멈춤 
        Stop();
        playerController.DontPlay();

        violetResult.Open(hitDamage);
    }
}
