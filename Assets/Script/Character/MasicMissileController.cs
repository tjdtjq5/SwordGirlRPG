using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasicMissileController : MonoBehaviour
{
    string damage;
    bool shotFlag;

    float speed = 18f;


    public void Shot(string damage)
    {
        this.damage = damage;
        shotFlag = true;

        float r = Random.Range(-3, 10);
        this.transform.rotation = Quaternion.Euler(0, 0, r);
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
            float fMove = Time.fixedDeltaTime * speed;
            transform.Translate(Vector2.right * fMove);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<Enemy>().Hit(damage);
            Destroy();
        }
    }
}
