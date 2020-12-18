using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleLogin : MonoBehaviour
{
    public BackendLogin backendLogin;
    void Awake()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder()
                .RequestServerAuthCode(false)
                .RequestEmail()
                .RequestIdToken()
                .Build());
        PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();
    }
    public void OnLogin()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool bSuccess) =>
            {
                if (bSuccess)
                {
                    Debug.Log("구글 로그인 성공");
                    if(PlayGamesPlatform.Instance.localUser.authenticated)
                    {
                        string tokenID = PlayGamesPlatform.Instance.GetIdToken();
                        backendLogin.GoogleAuth(tokenID);
                    }
                }
                else
                {
                    Debug.Log("구글 로그인 실패");
                }
            });
        }
    }
}
