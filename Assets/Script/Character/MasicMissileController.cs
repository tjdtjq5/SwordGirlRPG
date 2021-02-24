using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasicMissileController : MonoBehaviour
{
    string damage;
    bool isCritical;
    bool shotFlag;

    float speed = 18f;

    float r;
    public void Shot(string damage, bool isCritical)
    {
        this.damage = damage;
        this.isCritical = isCritical;
        shotFlag = true;

        r = Random.Range(-0.05f, 0.1f);
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
            // transform.Translate(new Vector3(0,r,1) * fMove);
            // transform.transform.position = new Vector3(transform.transform.position.x, transform.transform.position.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<Enemy>().Hit(damage, isCritical);
            Destroy();
        }
    }
}
