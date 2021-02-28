using Function;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinTree : Enemy
{
    public SkeletonAnimation skeletonAnimation;

    bool isEvolution2 = false;
    bool isEvolution3 = false;
    bool isEvolution4 = false;
    bool levelUpTouchStateFlag = false;

    IEnumerator initialCoroutine;
    IEnumerator levelTouchStateCoroutine;

    public GameObject effectPrepab;
    public Transform[] coinTransformList;

    [Header("코인트리 UI")]
    public Image hpGage_fore;
    public Text levelText;
    public Button levelUp_Btn;

    public void Start()
    {
        initialCoroutine = InitialCoroutine();
        StartCoroutine(initialCoroutine);

        LevelText_Setting();
    }

    public void SettingCoinTreeHp() // 체력 풀피로 셋팅 
    {
        int currentLevel = UserInfo.instance.coinTreeLevel;
        hp = CoinTreeChart.instance.GetCoinTreeHp(currentLevel);
        maxHp = CoinTreeChart.instance.GetCoinTreeHp(currentLevel);

        isEvolution2 = false;
        isEvolution3 = false;
        isEvolution4 = false;

        HpGage_Setting();
    }
    void LevelUp() // 코인트리 레벨업 
    {
        UserInfo.instance.CoinTreeLevelUp();
        LevelText_Setting();
        UserInfo.instance.SaveCoinTree(() => { });

        levelUp_Btn.onClick.RemoveAllListeners();
        levelUpTouchStateFlag = false;
    }
    void LevelTouchState()
    {
        levelUpTouchStateFlag = true;

        levelUp_Btn.onClick.AddListener(() => {
            if (levelTouchStateCoroutine != null) StopCoroutine(levelTouchStateCoroutine);
            levelTouchStateCoroutine = LevelTouchStateCoroutine();
            StartCoroutine(levelTouchStateCoroutine);
        });
    }
    IEnumerator LevelTouchStateCoroutine()
    {
        string currentAni = skeletonAnimation.AnimationName;
        if (!currentAni.Contains("degeneration"))
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "5_degeneration", false);
            skeletonAnimation.AnimationState.AddAnimation(0, "1_wait", true, 0);
        }

        yield return new WaitForSeconds(1.5f);

        LevelUp();

        StopCoroutine(initialCoroutine);
        StartCoroutine(initialCoroutine);
    }

    IEnumerator InitialCoroutine()
    {
        yield return null;

        WaitForSeconds waitTime = new WaitForSeconds(10);

        while (true)
        {
            if (!levelUpTouchStateFlag)
            {
                SettingCoinTreeHp();
                skeletonAnimation.AnimationState.SetAnimation(0, "1_wait", true);
            }
            yield return waitTime;
        }
    }

    void HpGage_Setting() // hp gage UI 셋팅 
    {
        if (MyMath.CompareValue(hp, "0") == -1) return;

        float fillAmount = 1 - MyMath.Amount(hp, maxHp);
        hpGage_fore.fillAmount = fillAmount;
    }

    void LevelText_Setting() // 레벨 텍스트 셋팅
    {
        int level = UserInfo.instance.coinTreeLevel;

        string b = "Level ";
        if (level < 10) b += "0";

        levelText.text = b + level;
    }


    public override void Hit(string damage, bool isCritical) 
    {
        base.Hit(damage, isCritical);

        float hpAmount = MyMath.Amount(hp, maxHp);

        if (!levelUpTouchStateFlag)
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

            HpGage_Setting();
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

        LevelTouchState();
    }
}
