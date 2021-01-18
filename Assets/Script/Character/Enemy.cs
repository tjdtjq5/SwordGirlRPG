using Function;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject nomalDamagePrepab;
    public GameObject criticalDamagePrepab;
    public virtual void Hit(string damage)
    {
        PlayerController.instance.AngerAdd();

        GameObject damagePrepab = null;
        int r = Random.Range(0, 2);
        switch (r)
        {
            case 0:
                damagePrepab = nomalDamagePrepab;
                break;
            case 1:
                damagePrepab = criticalDamagePrepab;
                damage = MyMath.Multiple(damage, 1.5F);
                break;
        }

        Vector2 position = new Vector2(this.transform.position.x, this.transform.position.y + 0.8f);
        damagePrepab = Instantiate(damagePrepab, this.transform.position, Quaternion.identity);
        damagePrepab.GetComponent<DamageEffect>().DamageUI_Setting(MyMath.ValueToString(damage));
    }

}
