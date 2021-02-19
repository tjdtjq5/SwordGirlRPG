using Function;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTree : Enemy
{
    public SkeletonAnimation skeletonAnimation;

    bool isEvolution2 = false;
    bool isEvolution3 = false;
    bool isEvolution4 = false;

    IEnumerator initialCoroutine;

    public GameObject effectPrepab;
    public Transform[] coinTransformList;

    public void Start()
    {
        initialCoroutine = InitialCoroutine();
        StartCoroutine(initialCoroutine);
    }

    public void SettingCoinTreeHp()
    {
        int currentLevel = UserInfo.instance.coinTreeLevel;
        hp = CoinTreeChart.instance.GetCoinTreeHp(currentLevel);
        maxHp = CoinTreeChart.instance.GetCoinTreeHp(currentLevel);

        isEvolution2 = false;
        isEvolution3 = false;
        isEvolution4 = false;
    }
    void LevelUp()
    {
        UserInfo.instance.CoinTreeLevelUp();
    }

    IEnumerator InitialCoroutine()
    {
        yield return null;

        WaitForSeconds waitTime = new WaitForSeconds(10);

        while (true)
        {
            SettingCoinTreeHp();
            skeletonAnimation.AnimationState.SetAnimation(0, "1_wait", true);
            yield return waitTime;
        }
    }


    public override void Hit(string damage, bool isCritical)
    {
        base.Hit(damage, isCritical);

        float hpAmount = MyMath.Amount(hp, maxHp);

        string currentAni = skeletonAnimation.AnimationName;

        if (currentAni.Contains("wait"))
        {
            if (hpAmount < 0.75f && !isEvolution2)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "2_evolution", false);
                skeletonAnimation.AnimationState.AddAnimation(0, "2_wait", true, 0);
                isEvolution2 = true;
            }
            else if (hpAmount < 0.5f && !isEvolution3)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "3_evolution2", false);
                skeletonAnimation.AnimationState.AddAnimation(0, "3_wait", true, 0);
                isEvolution3 = true;
            }
            else if (hpAmount < 0.25f && !isEvolution4)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "4_evolution", false);
                skeletonAnimation.AnimationState.AddAnimation(0, "4_wait", true, 0);
                isEvolution4 = true;
            }
        }

        int r = Random.Range(0, coinTransformList.Length - 1);
        int r2 = Random.Range(1, 4);
        string effectName = "dead" + r2;

        GameObject effect = Instantiate(effectPrepab, coinTransformList[r].position, Quaternion.identity, this.transform);
        effect.GetComponent<CoinTreeEffect>().Setting(effectName);
    }

    public override void Dead()
    {
        base.Dead();

        SettingCoinTreeHp();
        LevelUp();

        StopCoroutine(initialCoroutine);
        StartCoroutine(initialCoroutine);

        string currentAni = skeletonAnimation.AnimationName;
        if (!currentAni.Contains("degeneration"))
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "5_degeneration", false);
            skeletonAnimation.AnimationState.AddAnimation(0, "1_wait", true, 0);
        }
    }
}
