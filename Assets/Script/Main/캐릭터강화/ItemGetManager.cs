using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ItemGetManager : MonoBehaviour
{
    public GameObject blackPannel;
    public GameObject itemPannel;
    public Transform itemList;
    public SpriteAtlas moneyTypeSpriteAtlas;

    public void Open(MoneyType[] moneyTypes, int[] count) 
    {
        if (moneyTypes.Length != count.Length)
        {
            Debug.Log("길이가 다름");
            return;
        }
        if (itemList.childCount < moneyTypes.Length)
        {
            Debug.Log("아이템 수가 너무 많음");
            return;
        }

        blackPannel.SetActive(true);
        itemPannel.SetActive(true);

        for (int i = 0; i < itemList.childCount; i++) // 초기화 
        {
            itemList.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < moneyTypes.Length; i++)
        {
            itemList.GetChild(i).gameObject.SetActive(true);

            Transform item = itemList.GetChild(i);
            item.Find("Icon").GetComponent<Image>().sprite = moneyTypeSpriteAtlas.GetSprite(moneyTypes[i].ToString());
            item.Find("Icon").GetComponent<Image>().SetNativeSize();
            item.Find("Count").GetComponent<Text>().text = count[i].ToString();

            switch (moneyTypes[i])
            {
                case MoneyType.gold:
                    MoneyManager.instance.GoldAdd(count[i].ToString());
                    break;
                case MoneyType.crystal:
                    MoneyManager.instance.CrystalAdd(count[i]);
                    break;
                case MoneyType.masicStone:
                    MoneyManager.instance.MasicStoneAdd(count[i]);
                    break;
                case MoneyType.enhanceStone:
                    MoneyManager.instance.EnhanceStoneAdd(count[i]);
                    break;
                case MoneyType.transStone:
                    MoneyManager.instance.TransStoneAdd(count[i]);
                    break;
                case MoneyType.punishTiket:
                    UserInfo.instance.punishTiket += count[i];
                    break;
            }
        }

        UserInfo.instance.SaveMoney(() => { });
    }

    public void Open(MoneyType moneyTypes, int count)
    {
        blackPannel.SetActive(true);
        itemPannel.SetActive(true);

        for (int i = 0; i < itemList.childCount; i++) // 초기화 
        {
            itemList.GetChild(i).gameObject.SetActive(false);
        }

        itemList.GetChild(0).gameObject.SetActive(true);

        Transform item = itemList.GetChild(0);
        item.Find("Icon").GetComponent<Image>().sprite = moneyTypeSpriteAtlas.GetSprite(moneyTypes.ToString());
        item.Find("Icon").GetComponent<Image>().SetNativeSize();
        item.Find("Count").GetComponent<Text>().text = count.ToString();

        switch (moneyTypes)
        {
            case MoneyType.gold:
                MoneyManager.instance.GoldAdd(count.ToString());
                break;
            case MoneyType.crystal:
                MoneyManager.instance.CrystalAdd(count);
                break;
            case MoneyType.masicStone:
                MoneyManager.instance.MasicStoneAdd(count);
                break;
            case MoneyType.enhanceStone:
                MoneyManager.instance.EnhanceStoneAdd(count);
                break;
            case MoneyType.transStone:
                MoneyManager.instance.TransStoneAdd(count);
                break;
            case MoneyType.punishTiket:
                UserInfo.instance.punishTiket += count;
                break;
        }

        UserInfo.instance.SaveMoney(() => { });
    }

    public void Close()
    {
        blackPannel.SetActive(false);
        itemPannel.SetActive(false);
    }
}
