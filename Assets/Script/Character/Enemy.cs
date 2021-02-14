using Function;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string maxHp;
    public string hp;
    public GameObject nomalDamagePrepab;
    public GameObject criticalDamagePrepab;
    public float hitBoxPosY;
    public virtual void Hit(string damage)
    {
        PlayerController.instance.AngerAdd();

        GameObject damagePrepab = null;
        float critialPercent = UserInfo.instance.GetCriticalPercent();
      
        float r = Random.Range(0, critialPercent);
        if (r < critialPercent)
        {
            damagePrepab = criticalDamagePrepab;
            float criticalDamage = UserInfo.instance.GetCriticalDamagePercent();
            damage = MyMath.Multiple(damage, criticalDamage);
        }
        else
        {
            damagePrepab = nomalDamagePrepab;
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
