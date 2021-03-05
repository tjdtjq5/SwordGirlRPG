using BackEnd;
using Function;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    string violetRankingUUID = "9dd4bff0-7df4-11eb-86bb-21788bad5d71";

    int damageLength = 8;

    [ContextMenu("test")]
    public void Test()
    {
      
    }

    public int DamageToScore(string damage)
    {
        int unit = damage.Length * (int)(Mathf.Pow(10, damageLength));
        if (damage.Length > damageLength)
        {
            damage = damage.Substring(0, damageLength);
        }
        int data = int.Parse(damage);
        int score = unit + data;
        return score;
    }
    public string ScoreToDamage(int score)
    {
        int data = score % (int)(Mathf.Pow(10, damageLength));
        int unit = score / (int)(Mathf.Pow(10, damageLength));

        string damage = data.ToString();
        unit -= damage.Length;
        if (unit < 0) unit = 0;

        for (int i = 0; i < unit; i++)
        {
            damage = MyMath.Multiple(damage, 10);
        }

        return damage;
    }
    public void VioletScoreRegister(string damage)
    {
        int score = DamageToScore(damage);

        
    }
}
