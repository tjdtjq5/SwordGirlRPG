using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HitEffect : MonoBehaviour
{
    public GameObject prepab;
    public float speed = 18f;

    public Vector3 v = new Vector3(1, 0, 0);

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (prepab != null)
            {
                GameObject missile = Instantiate(prepab, this.transform.position, Quaternion.identity);
                StartCoroutine(ShotCoroutine(missile));
            }
        }
    }

    IEnumerator ShotCoroutine(GameObject prepab)
    {
        while (true)
        {
            float fMove = Time.fixedDeltaTime * speed;
            prepab.transform.Translate(v * fMove);
            yield return new WaitForFixedUpdate();
        }
    }
}
    
