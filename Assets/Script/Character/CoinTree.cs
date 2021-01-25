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


    public override void Hit(string damage)
    {
        base.Hit(damage);

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

        if (hpAmount < 0.75f)
        {
            GameObject effect = Instantiate(effectPrepab, new Vector2(this.transform.position.x, this.transform.position.y + 2), Quaternion.identity, this.transform);
            effect.GetComponent<CoinTreeEffect>().Setting("dead1");
        }
        else if (hpAmount < 0.5f)
        {
            GameObject effect = Instantiate(effectPrepab, new Vector2(this.transform.position.x, this.transform.position.y + 2), Quaternion.identity, this.transform);
            effect.GetComponent<CoinTreeEffect>().Setting("dead2");
        }
        else if (hpAmount < 0.25f)
        {
            GameObject effect = Instantiate(effectPrepab, new Vector2(this.transform.position.x, this.transform.position.y + 2), Quaternion.identity, this.transform);
            effect.GetComponent<CoinTreeEffect>().Setting("dead3");
        }
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
