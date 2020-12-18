using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsePolish : MonoBehaviour
{
    public GameObject popup;
    public Toggle toggle;
    public Loading loading;
    public void UsePolishOpen()
    {
        popup.SetActive(true);
    }

    public void OK()
    {
        if (!toggle.isOn)
        {
            Alram.instance.PopupOpen("이용약관에 동의 해주세요.", () => { Alram.instance.popup.SetActive(false); });
            return;
        }
        Param param = new Param();
        param.Add("UsePolish", toggle.isOn);
        BackendGameInfo.instance.PrivateTableUpdate("UserInfo", param);

        popup.SetActive(false);

        loading.LoadingOpen();
    }
}
