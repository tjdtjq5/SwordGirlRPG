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
    public void DamageUI_Setting(string text)
    {
        string countTypeString = text.Substring(text.Length - 2, 2);
        text = text.Substring(0, text.Length - 2);
        for (int i = 0; i < content.childCount; i++)
        {
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

        float currentY = this.transform.position.y;
        this.transform.DOMoveY(currentY + 1.8f, 1.2f).SetEase(Ease.Unset).OnComplete(()=> {
            Destroy(this.gameObject);
        });
    }
    
}
