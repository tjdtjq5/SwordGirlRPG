using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OkAlram : MonoBehaviour
{
    public static OkAlram instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject blackPannel;
    public GameObject alramObj;
    public Text text;

    public void Open(string text)
    {
        blackPannel.SetActive(true);
        alramObj.SetActive(true);
        this.text.text = text;
    }
    public void Close()
    {
        blackPannel.SetActive(false);
        alramObj.SetActive(false);
    }
}
