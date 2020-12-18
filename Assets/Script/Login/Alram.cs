using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alram : MonoBehaviour
{
    public static Alram instance;
    public GameObject popup;
    private void Awake()
    {
        instance = this;
    }

    public void PopupOpen(string text, System.Action callback)
    {
        popup.SetActive(true);
        popup.transform.Find("Text").GetComponent<Text>().text = text;
        popup.transform.Find("OK").GetComponent<Button>().onClick.RemoveAllListeners();
        popup.transform.Find("OK").GetComponent<Button>().onClick.AddListener(() => { callback(); });
    }
}
