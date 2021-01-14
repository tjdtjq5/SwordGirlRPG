using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassManager : MonoBehaviour
{
    public Transform middlePannel;
    public GameObject passPannel;

    public void Open()
    {
        for (int i = 0; i < middlePannel.childCount; i++)
        {
            middlePannel.GetChild(i).gameObject.SetActive(false);
        }
        passPannel.SetActive(true);
    }
    
}
