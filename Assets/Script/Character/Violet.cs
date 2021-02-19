using Function;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Violet : Enemy
{
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

    // 플레이어
    public PlayerController playerController;

    public void Play()
    {
        // 능력치 초기화 
        Init();

        // 시간 측정 코르틴 시작 
        TimePlay();

        // 공격 코르틴 시작 
        AttackPlay();
    }

    void Init() // 능력치 초기화 
    {
        maxHp = "0";
        hp = "0";

        atk = "100";
        atkPercent = 1.2f;
        criticalPercent = 30;
        criticalDamage = 1.4f;
        atkSpeed = 5f;

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
            float atkMultiple = atkCount * atkPercent;
            string damage = MyMath.Multiple(atk, atkMultiple);

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

    public override void Hit(string damage, bool isCritical)
    {
        base.Hit(damage, isCritical);

        hitDamage = MyMath.Add(hitDamage, damage);
    }
    public override void Dead()
    {
        base.Dead();
    }
}
