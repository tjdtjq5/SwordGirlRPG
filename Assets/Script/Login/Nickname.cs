using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using BackEnd;
using LitJson;

public class Nickname : MonoBehaviour
{
    public GameObject nicknamePopup;
    public InputField nicknameInput;

    public void PopupOpen()
    {
        nicknamePopup.SetActive(true);
    }

    // 한글, 영어, 숫자만 입력 가능하게
    private bool CheckNickname()
    {
        return Regex.IsMatch(nicknameInput.text, "^[0-9a-zA-Z가-힣]*$");
    }
    // 길이가 2~10으로 입력
    bool CheckLength()
    {
        int len = nicknameInput.text.Length;
        if (len >=2 && len <= 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OK()
    {
        if (!CheckLength())
        {
            Alram.instance.PopupOpen("닉네임 길이를 2 ~ 10 으로 맞춰주세요.", () => { Alram.instance.popup.SetActive(false); });
            return;
        }
        if (!CheckNickname())
        {
            Alram.instance.PopupOpen("한글, 영어, 숫자만 입력 해주세요.", () => { Alram.instance.popup.SetActive(false); });
            return;
        }

        BackendReturnObject bro = Backend.BMember.CreateNickname(nicknameInput.text);

        if (bro.IsSuccess())
        {
            nicknamePopup.SetActive(false);


        }
        switch (bro.GetStatusCode())
        {
            case "400":
                Alram.instance.PopupOpen("한글, 영어, 숫자만 입력 해주세요.", () => { Alram.instance.popup.SetActive(false); });
                break;
            case "409":
                Alram.instance.PopupOpen("중복된 닉네임이 존재합니다.", () => { Alram.instance.popup.SetActive(false); });
                break;
        }
    }
}
