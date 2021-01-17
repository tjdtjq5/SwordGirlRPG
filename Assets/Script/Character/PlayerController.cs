using DG.Tweening;
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

    [Header("분노")]
    int angerPoint = 100;
    int maxAngerPoint = 100;
    int chargeAngerPoint = 10;
    int angerDuration = 10;
    public Image foreAnger;
    bool angerFlag = false;

    // 공격
    public Button touchBtn;
    float attackSpeed = 0.3f;
    WaitForSeconds attackSpeedWaitTime;
    IEnumerator auto_M_Attack_Coroutine;
    IEnumerator auto_C_Attack_Coroutine;

    // 애니메이션
    public SkeletonAnimation skeletonAnimation;
    public SkeletonGhost skeletonGhost;

    [Header("마력검기")]
    public GameObject masicMissilePrepab;
    public Transform masicMissileTransform;
    public Transform trash;

    private void Start()
    {
        attackFlagWaitTime = new WaitForSeconds(attackSpeedCycle);
        attackSpeedWaitTime = new WaitForSeconds(attackSpeed);

        skeletonGhost.enabled = false;
        Auto_M_Attack(true);
        touchBtn.onClick.RemoveAllListeners();
        touchBtn.onClick.AddListener(() => { M_Attack(); });
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

    public void AngerState()
    {
        if (angerPoint < maxAngerPoint || angerFlag)
        {
            return;
        }
        angerFlag = true;

        ChangeAttack_M_To_C();
        skeletonGhost.enabled = true;

        foreAnger.DOFillAmount(0, angerDuration).OnComplete(()=> {
            angerFlag = false;
            angerPoint = 0;
            AngerUISetting();
            skeletonGhost.enabled = false;
            ChangeAttack_C_To_M();
        });
    }

    public Transform GetTarget() // 가장 가까운 적 한기 찾기 없으면 null 반환
    {
        Vector2 playerPosition = this.transform.position;
        float radius = 10;
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

        int r = Random.Range(1, 4);
        skeletonAnimation.AnimationState.SetAnimation(0, "attack_nomal_" + r, false);
        skeletonAnimation.AnimationState.AddAnimation(0, "idle_1", true,0);

        GameObject masicMissile = Instantiate(masicMissilePrepab, masicMissileTransform.position, Quaternion.identity, trash);
        masicMissile.GetComponent<MasicMissileController>().Shot(target, "10");
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

        if (Vector2.Distance(character.position, targetPos) > 0.5f)
        {
            touchBtn.onClick.RemoveAllListeners();
            Auto_C_Attack(false);
            TargetDash(target, () => {
                if (!angerFlag) return; // 분노상태가 아닐경우는 리턴

                skeletonAnimation.AnimationState.AddAnimation(0, "idle_2", true, 0);
                touchBtn.onClick.AddListener(() => { C_Attack(); });
                Auto_C_Attack(true);
            });
            return;
        }

        int r = Random.Range(1, 6);
        skeletonAnimation.AnimationState.SetAnimation(0, "attack_anger_" + r, false);
        skeletonAnimation.AnimationState.AddAnimation(0, "idle_2", true, 0);
        Debug.Log(skeletonAnimation.AnimationName);
        target.GetComponent<Enemy>().Hit("10");
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
            yield return attackSpeedWaitTime;

            C_Attack();
        }
    }

    void TargetDash(Transform target ,System.Action callback) // 타겟을 향해 대쉬 
    {
        if (character.position.x > target.position.x)
        {
            character.rotation = Quaternion.Euler(0, 180, 0);
        }

        skeletonAnimation.AnimationState.SetAnimation(0, "dash", false);
        character.DOMoveX(target.position.x, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => {
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
            skeletonAnimation.AnimationState.SetAnimation(0, "idle_2", true);

            touchBtn.onClick.AddListener(() => { C_Attack(); });
            Auto_C_Attack(true);
        });
    }

    void ChangeAttack_C_To_M()  // 근접공격 -> 원거리 공격
    {
        touchBtn.onClick.RemoveAllListeners();
        Auto_C_Attack(false);

        TargetDash(this.transform, () => {
            skeletonAnimation.AnimationState.SetAnimation(0, "idle_1", true);

            touchBtn.onClick.AddListener(() => { M_Attack(); });
            Auto_M_Attack(true);
        });
    }
}
