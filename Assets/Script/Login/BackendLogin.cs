using BackEnd;
using GooglePlayGames.BasicApi;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackendLogin : MonoBehaviour
{
    public Nickname nickname;
    public UsePolish usePolish;
    public Loading loading;

    private void Awake()
    {
        Backend.Initialize(() =>
        {
            // 초기화 성공한 경우 실행
            if (Backend.IsInitialized)
            {
                // example
                // 버전체크 -> 업데이트
                Debug.Log("뒤끝 초기화 성공");
            }
            // 초기화 실패한 경우 실행
            else
            {
                Debug.Log("뒤끝 초기화 실패");
            }
        });
    }

    public void GoogleAuth(string tokenID)
    {
        if (!Backend.IsInitialized) return;

        BackendReturnObject bro = Backend.BMember.AuthorizeFederation(tokenID, FederationType.Google);
        if (bro.IsSuccess())
        {
            Debug.Log("뒤끝 구글방식 로그인 성공");

            BackendAsyncClass.BackendAsync(Backend.BMember.GetUserInfo, (getUserInfoCallback) => {
                JsonData nicknameJsonData = getUserInfoCallback.GetReturnValuetoJSON()["row"]["nickname"];

                if (nicknameJsonData == null)
                {
                    nickname.PopupOpen();
                    Debug.Log("닉네임이 없다.");
                }
                else
                {
                    Debug.Log("닉네임이 있다.");
                    UserInfo.instance.nickName = nicknameJsonData.ToString();

                    BackendGameInfo.instance.GetPrivateContents("UserInfo", "UsePolish", () => {
                        bool isOn = bool.Parse(BackendGameInfo.instance.serverDataList[0]);
                        if (isOn)
                        {
                            loading.LoadingOpen();
                        }
                        else
                        {
                            usePolish.UsePolishOpen();
                        }
                    }, () => {
                        usePolish.UsePolishOpen();
                    });
                }
            });
        }
        else
        {
            Debug.Log("뒤끝 구글방식 로그인 실패");
        }
    }

    public void TestAuth()
    {
        if (!Backend.IsInitialized) return;

        Backend.BMember.CustomLogin("id", "password");

        BackendAsyncClass.BackendAsync(Backend.BMember.GetUserInfo, (getUserInfoCallback) => {
            JsonData nicknameJsonData = getUserInfoCallback.GetReturnValuetoJSON()["row"]["nickname"];
            if (nicknameJsonData == null)
            {
                nickname.PopupOpen();
                Debug.Log("닉네임이 없다.");
            }
            else
            {
                Debug.Log("닉네임이 있다.");
                UserInfo.instance.nickName = nicknameJsonData.ToString();

                BackendGameInfo.instance.GetPrivateContents("UserInfo", "UsePolish", () => {
                    bool isOn = bool.Parse(BackendGameInfo.instance.serverDataList[0]);
                    if (isOn)
                    {
                        loading.LoadingOpen();
                    }
                    else
                    {
                        usePolish.UsePolishOpen();
                    }      
                }, () => {
                    usePolish.UsePolishOpen();
                });
            }
        });
    }


}
