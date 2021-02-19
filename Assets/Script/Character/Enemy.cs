using Function;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string maxHp = "0";
    public string hp = "0";
    public GameObject nomalDamagePrepab;
    public GameObject criticalDamagePrepab;
    public float hitBoxPosY;

    public virtual void Hit(string damage, bool isCritical)
    {
        PlayerController.instance.AngerAdd();

        GameObject damagePrepab = null;

        switch (isCritical)
        {
            case true:
                damagePrepab = criticalDamagePrepab;
                break;
            case false:
                damagePrepab = nomalDamagePrepab;
                break;
        }

        Vector2 position = new Vector2(this.transform.position.x, this.transform.position.y + hitBoxPosY);
        damagePrepab = Instantiate(damagePrepab, position, Quaternion.identity);
        damagePrepab.GetComponent<DamageEffect>().DamageUI_Setting(MyMath.ValueToString(damage));

        hp = MyMath.Sub(hp, damage);

        if (MyMath.CompareValue(hp , "1") == -1)
        {
            Dead();
        }
    }

    public virtual void Dead()
    {

    }

}
