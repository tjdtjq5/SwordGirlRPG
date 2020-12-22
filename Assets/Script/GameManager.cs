using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Function;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [ContextMenu("adsfdsaf")]
    public void Test()
    {
        string a = "10000";
        string b = "35153";
        Debug.Log(a.Length + "   :   "  + b.Length);
        Debug.Log(MyMath.ValueToString(a) + "   :   " + MyMath.ValueToString(b));
    }
}
