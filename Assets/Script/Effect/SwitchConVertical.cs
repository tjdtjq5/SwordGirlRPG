using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchConVertical : MonoBehaviour
{
    public Scrollbar scrollbar;
    public Transform min;
    public Transform max;
    public Transform switchCon;


    float distance = 0;

    private void Start()
    {
        distance = max.position.y - min.position.y;
    }

    void Update()
    {
        float value = min.position.y + distance * (1-scrollbar.value);
        value = Mathf.Clamp(value, max.position.y, min.position.y);
        switchCon.DOMoveY(value, 0);
    }
}
