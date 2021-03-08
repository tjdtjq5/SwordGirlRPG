using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasicMissileController : MonoBehaviour
{
    string damage;
    bool isCritical;
    bool shotFlag;

    float speed = 0.4f;

    Transform target;
    Vector2 targetPos;

    float r;
    public void Shot(string damage, bool isCritical, Transform target)
    {
        this.damage = damage;
        this.isCritical = isCritical;
        shotFlag = true;
        this.target = target;

        r = Random.Range(-0.7f, 1.5f);

        targetPos = new Vector2(target.position.x, this.transform.position.y + r);
    }

    void Destroy()
    {
        shotFlag = false;
        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        if (shotFlag)
        {
            //float fMove = Time.fixedDeltaTime * speed;
            //  transform.Translate(Vector2.right * fMove);

            // 타겟이 있을 경우 
            if (target != null)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, targetPos, speed);

                if (Vector2.Distance(this.transform.position, targetPos) < 0.1f)
                {
                    if (target.tag == "Enemy")
                    {
                        target.GetComponent<Enemy>().Hit(damage, isCritical);
                    }
                    Destroy();
                }
            }
            else // 타겟이 사라질 경우 
            {
                Destroy();
            }
        }
    }
}
