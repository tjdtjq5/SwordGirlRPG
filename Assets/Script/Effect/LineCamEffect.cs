using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCamEffect : MonoBehaviour
{
    public Camera theCam;

    private void Update()
    {
        float theCamSize = theCam.orthographicSize / 5;
        this.transform.localScale = new Vector2(theCamSize, theCamSize);
    }
}
