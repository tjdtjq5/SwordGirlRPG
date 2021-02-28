using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.U2D;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    public SpriteAtlas damageSpriteAtlas;
    public Transform content;
    public Image criImg;

    public void DamageUI_Setting(string text)
    {
       

        // 영문 숫자 조합 
        if (text.Contains("."))
        {
            string countTypeString = text.Substring(text.Length - 2, 2);
            text = text.Substring(0, text.Length - 2);
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).GetComponent<Image>().DOFade(0, 1);

                if (i < text.Length)
                {
                    content.GetChild(i).gameObject.SetActive(true);
                    if (text[i] == '.') content.GetChild(i).GetComponent<Image>().sprite = damageSpriteAtlas.GetSprite("+".ToString());
                    else content.GetChild(i).GetComponent<Image>().sprite = damageSpriteAtlas.GetSprite(text[i].ToString());
                    content.GetChild(i).GetComponent<Image>().SetNativeSize();
                }
                else
                {
                    content.GetChild(i).gameObject.SetActive(false);
                }
            }
            content.GetChild(content.childCount - 1).gameObject.SetActive(true);
            content.GetChild(content.childCount - 1).GetComponent<Image>().sprite = damageSpriteAtlas.GetSprite(countTypeString.ToString());
            content.GetChild(content.childCount - 1).GetComponent<Image>().SetNativeSize();
        }
        else // 숫자 만 8자 이하 
        {
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).GetComponent<Image>().DOFade(0, 1);

                if (i < text.Length)
                {
                    content.GetChild(i).gameObject.SetActive(true);
                    content.GetChild(i).GetComponent<Image>().sprite = damageSpriteAtlas.GetSprite(text[i].ToString());
                    content.GetChild(i).GetComponent<Image>().SetNativeSize();
                }
                else
                {
                    content.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        transform.DOPunchScale(Vector3.one * 0.003f, 0.3f);
        transform.DOMove(transform.position + Vector3.up * 2.5f, 1.0f).OnComplete(() => { Destroy(gameObject); });

        if (criImg != null)
        {
            criImg.transform.position = new Vector2(content.GetChild(0).position.x + 0.6f , criImg.transform.position.y);

            criImg.DOFade(1, .3f).OnComplete(() => { criImg.DOFade(0, 0.2f); });

            criImg.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f);

        }
    }
    
}
