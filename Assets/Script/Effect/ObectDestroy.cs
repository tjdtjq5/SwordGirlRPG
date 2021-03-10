using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObectDestroy : MonoBehaviour
{
    public float deadTime;
    void Start()
    {
        Invoke("Dead", deadTime);
    }

    void Dead()
    {
        Destroy(this.gameObject);
    }
    
}
