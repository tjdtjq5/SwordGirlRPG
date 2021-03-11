using BackEnd.Tcp;
using DG.Tweening;
using Function;
using Spine;
using Spine.Unity;
using Spine.Unity.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
    }

    public Transform character;

    // 공격간격
    float attackSpeedCycle = 0.1f; // 공격속도 간격 
    bool attackFlag = false;
    WaitForSeconds attackFlagWaitTime;
    float targetDistance = 1; // 근접공격 간격 거리

    [Header("분노")]
    float angerPoint = 0; // 시작시 분노상태
    int maxAngerPoint = 100;
    int chargeAngerPoint = 10;
    float angerDuration = 5;
    public Image foreAnger;
    public Sprite none_Anger_Sprite;
    public Sprite max_Anger_Sprite_01;
    public Sprite max_Anger_Sprite_02;
    public GameObject max_Text_Obj;
    IEnumerator gageMaxEffectCoroutine;
    bool gageMaxEffectFlag = false;
    bool angerFlag = false;
    IEnumerator fillAmountCoroutine;
    public Camera theCam;
    Vector3 originCamPos;
    bool camEffectFlag;
    public Image whitePannel;
    public GameObject angerHitObj;

    // 공격
    public Button touchBtn;
    float attackSpeed = 6f;
    WaitForSeconds attackSpeedWaitTime;
    IEnumerator auto_M_Attack_Coroutine;
    IEnumerator auto_C_Attack_Coroutine;

    // 애니메이션
    public Animator animator;
    public SkeletonGhost skeletonGhost;
    public Transform dash_ani;
    public GameObject lineEffect;
    IEnumerator faceCoroutine;
    public ParticleSystem dustParticle;

    // 스킨
    public SkeletonMecanim skeletonMecanim;

    [Header("마력검기")]
    public GameObject masicMissilePrepab;
    public Transform masicMissileTransform;
    public Transform trash;

    [Header("체력")]
    public Profile profile;
    public string currentHp;

    [Header("데미지 이펙트 프리팹")]
    public GameObject nomal_damageEffect;
    public GameObject critical_damageEffect;

    // 멈춤기능 
    bool dontPlayFlag = false;

    // 대쉬 
    bool isDash = false;

    // 델리게이트 
    public delegate void DeadDelegate();
    public DeadDelegate deadDelegate;

    private void Start()
    {
        attackFlagWaitTime = new WaitForSeconds(attackSpeedCycle);

        Hp_Initialized();
        Hp_UI_Setting();

        Play();

        SkinChange();
    }

    void AngerUISetting()
    {
        foreAnger.fillAmount = angerPoint / (float)maxAngerPoint;

        if (foreAnger.fillAmount == 1)
        {
            if (!gageMaxEffectFlag)
            {
                GageMaxEffectPlay();
            }
        }
        else
        {
            max_Text_Obj.SetActive(false);
        }
        if (foreAnger.fillAmount == 0)
        {
            GageMaxEffectStop();
        }
    }

    void GageMaxEffectPlay()
    {
        gageMaxEffectFlag = true;

        if (gageMaxEffectCoroutine != null) StopCoroutine(gageMaxEffectCoroutine);
        gageMaxEffectCoroutine = GageMaxEffectCoroutine();
        StartCoroutine(gageMaxEffectCoroutine);
    }
    void GageMaxEffectStop()
    {
        gageMaxEffectFlag = false;

        max_Text_Obj.SetActive(false);
        foreAnger.sprite = none_Anger_Sprite;
        if (gageMaxEffectCoroutine != null) StopCoroutine(gageMaxEffectCoroutine);
    }
    IEnumerator GageMaxEffectCoroutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.15f);
        max_Text_Obj.SetActive(true);

        while (true)
        {
            foreAnger.sprite = max_Anger_Sprite_01;
            yield return waitTime;
            foreAnger.sprite = max_Anger_Sprite_02;
            yield return waitTime;
        }
    }
    public void AngerAdd()
    {
        if (angerFlag) return;

        angerPoint += chargeAngerPoint;
        if (angerPoint > maxAngerPoint) angerPoint = maxAngerPoint;

        AngerUISetting();
    }

    public string Atk()
    {
        string atk = UserInfo.instance.GetAtk();
        float atkPercent = UserInfo.instance.GetAtkPercent();
        float angerDamage = UserInfo.instance.GetAngerDamage();

        string total = MyMath.Multiple(atk, atkPercent);
        if(angerFlag) total = MyMath.Multiple(total, angerDamage);

        return total;
    }

    public void AngerState()
    {
        if (angerPoint < maxAngerPoint || angerFlag || dontPlayFlag)
        {
            return;
        }
        angerFlag = true;

        ChangeAttack_M_To_C();
        skeletonGhost.enabled = true;


        // 유저의 분노시간과 디폴트 분노시간을 더한 값
        float angerTime = UserInfo.instance.GetAngerTime();

        if (fillAmountCoroutine != null) StopCoroutine(fillAmountCoroutine);
        fillAmountCoroutine = FillAmountCoroutine(angerDuration + angerTime, () => {
            angerPoint = 0;
            StopAngerState();
        });
        StartCoroutine(fillAmountCoroutine);

    }

    IEnumerator FillAmountCoroutine(float time ,System.Action callback)
    {
        float distance = time * maxAngerPoint;
        float w = 0.05f;
        float minuse = w * maxAngerPoint; // waitTime 
        float y = distance / minuse;
        float x = maxAngerPoint / y;

        WaitForSeconds waitTime = new WaitForSeconds(w);

        while (foreAnger.fillAmount > 0)
        {
            float a = angerPoint;
            angerPoint -= x;
            AngerUISetting();
            float b = angerPoint;

            if ((int)(a / 5) != (int)(b / 5) && foreAnger.fillAmount < 0.95f)
            {
                float rx = UnityEngine.Random.Range(1.5f, 3f);
                float ry = UnityEngine.Random.Range(1f, 2.5f);
                Vector2 rPos = new Vector2(character.position.x + rx, character.position.y + ry);
                Instantiate(angerHitObj, rPos, Quaternion.identity, trash);
            }
       
            yield return waitTime;
        }

        callback();
    }

    void AngerCamEffectPlay()
    {
        if (camEffectFlag) return;

        originCamPos = theCam.transform.position;
        camEffectFlag = true;



        theCam.DOShakePosition(0.05f , 0.5f , 10, 50, false).OnComplete(()=> {
            theCam.transform.DOMove(originCamPos, 0.02f).OnComplete(()=> {
                camEffectFlag = false;
            });
        });
    }

    void StopAngerState()
    {
        if (fillAmountCoroutine != null) StopCoroutine(fillAmountCoroutine);

        angerFlag = false;
        AngerUISetting();
        skeletonGhost.enabled = false;
        ChangeAttack_C_To_M();
    }

    public Transform GetTarget() // 가장 가까운 적 한기 찾기 없으면 null 반환
    {
        if (dontPlayFlag) // 움직이지 말기 설정시 null 반환
        {
            return null;
        }

        Vector2 playerPosition = this.transform.position;
        float radius = 15;
        RaycastHit2D[] hit = Physics2D.CircleCastAll(playerPosition, radius, Vector2.zero);

        List<Transform> enemyTransform = new List<Transform>();
        List<float> enemyDistance = new List<float>();

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i] && hit[i].transform.tag == "Enemy")
            {
                enemyTransform.Add(hit[i].transform);
                enemyDistance.Add(Vector2.Distance(hit[i].transform.position, playerPosition));
            }
        }


        enemyDistance.Sort();

        for (int i = 0; i < enemyTransform.Count; i++)
        {
            if (Vector2.Distance(enemyTransform[i].position, playerPosition) == enemyDistance[0])
            {
                return enemyTransform[i];
            }
        }

        return null;
    }

    IEnumerator AttackFlagCoroutine() // 공격간격 코르틴
    {
        attackFlag = true;
        yield return attackFlagWaitTime;
        attackFlag = false;
    }

    void M_Attack() // 원거리 공격 
    {
        if (attackFlag) // 다른 공격 상태일 경우 
        {
            return;
        }
        Transform target = GetTarget();
        if (target == null) // 타겟이 없는경우 
        {
            return;
        }

        int aniR = UnityEngine.Random.Range(1, 4);

        animator.SetTrigger("attack_nomal_" + aniR);

        GameObject masicMissile = Instantiate(masicMissilePrepab, masicMissileTransform.position, Quaternion.Euler(0,0,0), trash);

        string damage = Atk();
        float critialPercent = UserInfo.instance.GetCriticalPercent();
      

        float r = UnityEngine.Random.Range(0, 100);
        if (r < critialPercent) // 크리티컬 
        {
            float criticalDamage = UserInfo.instance.GetCriticalDamagePercent();
            damage = MyMath.Multiple(damage, criticalDamage);
            masicMissile.GetComponent<MasicMissileController>().Shot(damage, true, target);

        }
        else  // 노말 
        {
            masicMissile.GetComponent<MasicMissileController>().Shot(damage, false, target);
        }

        StartCoroutine(AttackFlagCoroutine());

        FaceAttackAni(0.3f);

      //  dustParticle.Play();
    }
     
    void Auto_M_Attack(bool flag) // 원거리 자동 공격
    {
        switch (flag)
        {
            case true:
                if (auto_M_Attack_Coroutine != null) StopCoroutine(auto_M_Attack_Coroutine);
                auto_M_Attack_Coroutine = Auto_M_Attack_Coroutine();
                StartCoroutine(auto_M_Attack_Coroutine);
                break;
            case false:
                if (auto_M_Attack_Coroutine != null) StopCoroutine(auto_M_Attack_Coroutine);
                break;
        }
    }
    
    IEnumerator Auto_M_Attack_Coroutine() // 원거리 자동 공격 코르틴
    {
        while (true)
        {
            float userAutoSpeed = UserInfo.instance.GetAutoAtkSpeed() / 100 * attackSpeed;
            float autoSpeed = attackSpeed - userAutoSpeed;
            attackSpeedWaitTime = new WaitForSeconds(autoSpeed);
            yield return attackSpeedWaitTime;
            M_Attack();
        }
    } 

    void C_Attack() // 근거리공격
    {
        Transform target = GetTarget();

        if (target == null)
        {
            return;
        }

        Vector2 targetPos = new Vector2(target.position.x, character.position.y);

        if (Vector2.Distance(character.position, targetPos) > 0.5f + System.Math.Abs(targetDistance))
        {
            touchBtn.onClick.RemoveAllListeners();
            Auto_C_Attack(false);
            TargetDash(target, () => {
                if (!angerFlag) return; // 분노상태가 아닐경우는 리턴
                Auto_C_Attack(true);
            });
            return;
        }

        string damage = Atk();
        float critialPercent = UserInfo.instance.GetCriticalPercent();
        float r = UnityEngine.Random.Range(0, 100);
        if (r < critialPercent) // 크리티컬 
        {
            float criticalDamage = UserInfo.instance.GetCriticalDamagePercent();
            damage = MyMath.Multiple(damage, criticalDamage);
            target.GetComponent<Enemy>().Hit(damage, true);

        }
        else  // 노말 
        {
            target.GetComponent<Enemy>().Hit(damage, false);
        }

        AngerCamEffectPlay();

       // dustParticle.Play();
    }
    void Auto_C_Attack(bool flag) // 근거리 자동공격 
    {
        switch (flag)
        {
            case true:
                animator.SetTrigger("attack_anger");
                lineEffect.gameObject.SetActive(true);
                if (auto_C_Attack_Coroutine != null) StopCoroutine(auto_C_Attack_Coroutine);
                auto_C_Attack_Coroutine = Auto_C_Attack_Coroutine();
                StartCoroutine(auto_C_Attack_Coroutine);
                break;
            case false:
                lineEffect.gameObject.SetActive(false);
                if (auto_C_Attack_Coroutine != null) StopCoroutine(auto_C_Attack_Coroutine);
                break;
        }
    }
    IEnumerator Auto_C_Attack_Coroutine() // 근거리 자동공격 코르틴
    {
        while (true)
        {
            //    float userAutoSpeed = UserInfo.instance.GetAutoAtkSpeed() / 100 * attackSpeed;
            //   float autoSpeed = attackSpeed - userAutoSpeed;
            float autoSpeed = 0.1f;
            attackSpeedWaitTime = new WaitForSeconds(autoSpeed);
            yield return attackSpeedWaitTime;

            C_Attack();
        }
    }

    void TargetDash(Transform target ,System.Action callback) // 타겟을 향해 대쉬 
    {
        isDash = true;

        if (target.GetComponent<BoxCollider2D>())
        {
            targetDistance = target.GetComponent<BoxCollider2D>().size.x / 2;
        }
        else
        {
            targetDistance = 0;
        }

        if (character.position.x > target.position.x)
        {
            character.rotation = Quaternion.Euler(0, 180, 0);
            targetDistance *= -1;

            dash_ani.rotation = Quaternion.Euler(0, 180, 0);
            dash_ani.position = new Vector2(character.position.x + 0.5f, dash_ani.position.y);
            dash_ani.GetComponent<Animator>().SetTrigger("end");
        }
        else
        {
            dash_ani.rotation = Quaternion.Euler(0, 0, 0);
            dash_ani.position = new Vector2(character.position.x - 0.5f, dash_ani.position.y);
            dash_ani.GetComponent<Animator>().SetTrigger("dash");
        }

        animator.SetTrigger("dash");
        character.DOMoveX(target.position.x - targetDistance, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => {
            isDash = false;

            character.rotation = Quaternion.Euler(0, 0, 0);
            callback();
        });
    }

    void ChangeAttack_M_To_C() // 원거리공격 -> 근접공격 
    {
        if (!angerFlag) // 분노상태가 아닐경우 
        {
            return;
        }

        Transform target = GetTarget();
        if (target == null) // 주위에 몬스터가 없음
        {
            Invoke("ChangeAttack_M_To_C", 0.1f);
            return;
        } 

        touchBtn.onClick.RemoveAllListeners();
        Auto_M_Attack(false);

        whitePannel.DOFade(0.7f, 0.07f).OnComplete(() => { whitePannel.DOFade(0, 0.07f); });

        TargetDash(target, () => {
            Auto_C_Attack(true);
        });
    }
    void ChangeAttack_C_To_M()  // 근접공격 -> 원거리 공격
    {
        touchBtn.onClick.RemoveAllListeners();
        Auto_C_Attack(false);

        whitePannel.DOFade(0.7f, 0.07f).OnComplete(() => { whitePannel.DOFade(0, 0.07f); });


        TargetDash(this.transform, () => {
            animator.SetTrigger("idle");

            touchBtn.onClick.AddListener(() => { M_Attack(); });
            Auto_M_Attack(true);
        });
    }
    /// 체력
    public void Hp_Initialized() // 체력 초기화 
    {
        string playerHp = UserInfo.instance.GetHp();
        float playerHpPercent = UserInfo.instance.GetHpPercent();

        string maxHp = MyMath.Multiple(playerHp, playerHpPercent);

        currentHp = maxHp;
    }
    public void Hp_UI_Setting() // 체력 정보 UI셋팅 
    {
        string playerHp = UserInfo.instance.GetHp();
        float playerHpPercent = UserInfo.instance.GetHpPercent();

        string maxHp = MyMath.Multiple(playerHp, playerHpPercent);

        profile.Hp_UI_Setting(currentHp, maxHp);
    }

    void FaceAttackAni(float time)
    {
        if (faceCoroutine != null) StopCoroutine(faceCoroutine);
        faceCoroutine = FaceCoroutine(time);
        StartCoroutine(faceCoroutine);
    }

    // 얼굴 애니
    IEnumerator FaceCoroutine(float time)
    {
        animator.SetTrigger("face_attack");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("face_flash");
    }

    // 스킨 변경 
    public void SkinChange()
    {
        string eqipClothName = UserInfo.instance.GetEqipCloth().name;
        string eqipWeaponeName = UserInfo.instance.GetEqipWeapone().name;

        string clothSkinName = ClothChart.instance.GetClothChartInfo(eqipClothName)[0].SkinName;
        string weaponeSkinName = WeaponeChart.instance.GetWeaponeChartInfo(eqipWeaponeName)[0].SkinName;


        Skin combined = new Skin("combined");

        List<string> SkinList = new List<string>();
        SkinList.Add(clothSkinName);
        SkinList.Add(weaponeSkinName);

        foreach (var skinName in SkinList)
        {
            Skin skin = skeletonMecanim.skeleton.Data.FindSkin2(skinName);

            if (skin != null)
            {
                combined.AddSkin(skin);
            }
        }

        skeletonMecanim.skeleton.Skin = null;
        skeletonMecanim.skeleton.SetSkin(combined);
        skeletonMecanim.Skeleton.SetSlotsToSetupPose();
        skeletonMecanim.LateUpdate();
      
    }

    // 행동 멈춤 
    public void DontPlay()
    {
        if (angerFlag)
        {
            StopAngerState();
        }

        dontPlayFlag = true;
    }
    public void Play()
    {
        dontPlayFlag = false;

        skeletonGhost.enabled = false;
        Auto_M_Attack(true);
        touchBtn.onClick.RemoveAllListeners();
        touchBtn.onClick.AddListener(() => { M_Attack(); });
    }
    
    public void Hit(string damage, bool isCritical)
    {
        return;
        if (isDash) return; // 대쉬중일때는 무적상태 

        // 데미지 이펙트 프리팹 
        GameObject damagePrepab = null;
        float prepabY = 1.3F;

        switch (isCritical)
        {
            case true:
                damagePrepab = Instantiate(critical_damageEffect, new Vector2(character.position.x, character.position.y + prepabY), Quaternion.identity, this.transform);
                break;
            case false:
                damagePrepab = Instantiate(nomal_damageEffect, new Vector2(character.position.x, character.position.y + prepabY), Quaternion.identity, this.transform);
                break;
        }

        damagePrepab.GetComponent<DamageEffect>().DamageUI_Setting(MyMath.ValueToString(damage));

        // 체력 깍이게 하기 
        currentHp = MyMath.Sub(currentHp, damage);
        Hp_UI_Setting();

        // 죽었다면 
        if (MyMath.CompareValue(currentHp , "0") < 1)
        {
            if (deadDelegate != null)
            {
                deadDelegate();
            }
        }
    }

    [ContextMenu("test")]
    void Test()
    {
        SkinChange();
    }
}
