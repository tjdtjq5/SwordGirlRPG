using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public virtual void Hit(string damage)
    {
        PlayerController.instance.AngerAdd();
    }

}
