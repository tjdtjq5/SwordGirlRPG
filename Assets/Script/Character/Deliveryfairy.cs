using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deliveryfairy : MonoBehaviour
{
    public Transform fairy;
    public SkeletonData skeletonData;
    public SkeletonAnimation ani;

    [Header("설정")]
    public float moveSpeed;
    public float coolTime;

    IEnumerator playCoroutine;
    IEnumerator moveCoroutine;
    IEnumerator touchCoroutine;
    Vector2 initPosition = new Vector2(30, 0);
    Vector2 endPosition = new Vector2(-15, 0);

    bool touchFlag = false;

    private void Start()
    {

        Init();
    }

    public void Init()
    {
        Stop();

        touchFlag = false;
        fairy.position = initPosition;

        Play();
    }
    public void Play()
    {
        if (playCoroutine != null) StopCoroutine(playCoroutine);
        playCoroutine = PlayCoroutine();
        StartCoroutine(playCoroutine);
    }
    IEnumerator PlayCoroutine()
    {
        yield return new WaitForSeconds(coolTime);
        Move();
    }
    void Move()
    {

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = MoveCoroutine();
        StartCoroutine(moveCoroutine);

        ani.AnimationState.SetAnimation(0, "move", true);

        fairy.gameObject.SetActive(true);
    }
    IEnumerator MoveCoroutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.02f);

        while (fairy.position.x > endPosition.x)
        {
            fairy.position = new Vector2(fairy.position.x - moveSpeed, fairy.position.y);
            yield return waitTime;
        }

        Init();
    }

    public void Stop()
    {
        fairy.gameObject.SetActive(false);
        if (playCoroutine != null) StopCoroutine(playCoroutine);
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        fairy.position = initPosition;
    }
    public void TouchFairy()
    {
        if (touchFlag) return;

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);

        if (touchCoroutine != null) StopCoroutine(touchCoroutine);
        touchCoroutine = TouchCoroutine();
        StartCoroutine(touchCoroutine);

        ani.AnimationState.SetAnimation(0, "click", false);

        touchFlag = true;
    }
    IEnumerator TouchCoroutine()
    {
        yield return new WaitForSeconds(1.45f);

        Init();
    }
}
