using DG.Tweening;
using Function;
using Spine.Unity;
using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    float angerDuration = 10;
    public Image foreAnger;
    bool angerFlag = false;
    IEnumerator fillAmountCoroutine;

    // 공격
    public Button touchBtn;
    float attackSpeed = 6f;
    WaitForSeconds attackSpeedWaitTime;
    IEnumerator auto_M_Attack_Coroutine;
    IEnumerator auto_C_Attack_Coroutine;

    // 애니메이션
    public Animator animator;
    public SkeletonGhost skeletonGhost;

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

    private void Start()
    {
        attackFlagWaitTime = new WaitForSeconds(attackSpeedCycle);

        Hp_Initialized();
        Hp_UI_Setting();

        Play();
    }

    void AngerUISetting()
    {
        foreAnger.fillAmount = angerPoint / (float)maxAngerPoint;
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
            angerFlag = false;
            angerPoint = 0;
            AngerUISetting();
            skeletonGhost.enabled = false;
            ChangeAttack_C_To_M();
        });
        StartCoroutine(fillAmountCoroutine);

        /*
        foreAnger.DOFillAmount(0, (angerDuration + angerTime)).OnComplete(()=> {
            angerFlag = false;
            angerPoint = 0;
            AngerUISetting();
            skeletonGhost.enabled = false;
            ChangeAttack_C_To_M();
        });*/
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
            angerPoint -= x;
            AngerUISetting();
            yield return waitTime;
        }

        callback();
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

        int aniR = Random.Range(1, 4);

        animator.SetTrigger("attack_nomal_" + aniR);

        GameObject masicMissile = Instantiate(masicMissilePrepab, masicMissileTransform.position, Quaternion.identity, trash);

        string damage = Atk();
        float critialPercent = UserInfo.instance.GetCriticalPercent();
      

        float r = Random.Range(0, 100);
        if (r < critialPercent) // 크리티컬 
        {
            float criticalDamage = UserInfo.instance.GetCriticalDamagePercent();
            damage = MyMath.Multiple(damage, criticalDamage);
            masicMissile.GetComponent<MasicMissileController>().Shot(damage, true);

        }
        else  // 노말 
        {
            masicMissile.GetComponent<MasicMissileController>().Shot(damage, false);
        }

        StartCoroutine(AttackFlagCoroutine());
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
        float r = Random.Range(0, 100);
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

    }
    void Auto_C_Attack(bool flag) // 근거리 자동공격 
    {
        switch (flag)
        {
            case true:
                if (auto_C_Attack_Coroutine != null) StopCoroutine(auto_C_Attack_Coroutine);
                auto_C_Attack_Coroutine = Auto_C_Attack_Coroutine();
                StartCoroutine(auto_C_Attack_Coroutine);
                break;
            case false:
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
        }
        animator.SetTrigger("dash");
        character.DOMoveX(target.position.x - targetDistance, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => {
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

        TargetDash(target, () => {
            animator.SetTrigger("attack_anger");

            Auto_C_Attack(true);
        });
    }
    void ChangeAttack_C_To_M()  // 근접공격 -> 원거리 공격
    {
        touchBtn.onClick.RemoveAllListeners();
        Auto_C_Attack(false);

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

    }

    [ContextMenu("test")]
    void Test()
    {
        animator.SetTrigger("idle");
    }
}
