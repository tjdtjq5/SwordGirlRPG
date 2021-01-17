using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasicMissileController : MonoBehaviour
{
    Transform target;
    string damage;
    bool shotFlag;

    float speed = 0.1f;

    public void Shot(Transform target, string damage)
    {
        this.target = target;
        this.damage = damage;
        shotFlag = true;
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
            this.transform.position = Vector2.MoveTowards(this.transform.position, target.position, speed);
            if (Vector2.Distance(this.transform.position, target.position) < 0.05f)
            {
                target.GetComponent<Enemy>().Hit(damage);
                Destroy();
            }
        }
    }
}
