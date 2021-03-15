using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTextEffect : MonoBehaviour
{
    public float moveY;
    
    public void Set(string text, Color color)
    {
        this.GetComponent<Text>().text = text;
        this.GetComponent<Text>().color = color;

        float y = this.transform.position.y - moveY;
        this.transform.DOMoveY(y, 0.6f).OnComplete(() => {
            Destroy(this.gameObject);
        });

        this.GetComponent<Text>().DOFade(0.3f, 0.6f);
    }
    
}
