using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameManager : MonoBehaviour
{
    [Header("프리팹")]
    public GameObject cardPrepab;

    [Header("UI")]
    public GameObject frameObj;
    public Transform content;
    public Text nicknameText;
    public Text powerText;
    public Text buffText;
    public Image myClothIcon;
    public Image myFrame;

    [Header("스크립트")]
    public Profile profile;

    void Start()
    {
        Init();
    }

    public void Open()
    {
        frameObj.SetActive(true);
        UserFrameInfoSetting();
    }
    public void Close()
    {
        frameObj.SetActive(false);
    }

    // 프레임 테이블 정보 불러오기  - UI 셋팅 
    void Init()
    {
        nicknameText.text = "";
        powerText.text = "";
        buffText.text = "";

        FrameChartInfo[] frames = FrameChart.instance.frameChartInfos;

        for (int i = 0; i < frames.Length; i++)
        {
            Transform card = Instantiate(cardPrepab, Vector2.zero, Quaternion.identity, content).transform;
            card.Find("Frame").GetComponent<Image>().sprite = frames[i].Image;
            card.Find("호칭").GetComponent<Text>().text = frames[i].SubName;
        }
    }

    // 유저 프레임 정보 불러오기 - UI 셋팅 
    void UserFrameInfoSetting()
    {
        string eqipClothName = UserInfo.instance.GetEqipCloth().name;
        myClothIcon.sprite = ClothChart.instance.GetClothChartInfo(eqipClothName)[0].Icon;

        FrameChartInfo[] frames = FrameChart.instance.frameChartInfos;

        myFrame.sprite = frames[0].Image;

        UserFrame userEqipFrame = UserInfo.instance.GetEqipFrame();
        string eqipFrameSubName = "";

        // 장착중인 프레임이 있을경우 
        if (userEqipFrame != null)
        {
            eqipFrameSubName = userEqipFrame.subName;
            nicknameText.text = UserInfo.instance.nickName + "     호칭 : <" + userEqipFrame.subName + ">";
        }
        else // 없을경우
        {
            nicknameText.text = UserInfo.instance.nickName;
        }

        powerText.text = "전투력 : " + profile.GetTotalPower();

        buffText.text = "";

        for (int i = 0; i < frames.Length; i++)
        {
            Transform card = content.GetChild(i);

            card.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();

            // 장착중인 프레임 
            if (eqipFrameSubName == frames[i].SubName)
            {
                card.Find("EqipCheckBOX").gameObject.SetActive(true);
                myFrame.sprite = frames[i].Image;
            }
            else // 아닐경우
            {
                card.Find("EqipCheckBOX").gameObject.SetActive(false);

                // 가지고 있는 프레임이라면 
                if (UserInfo.instance.IsExistFrame(frames[i].SubName))
                {
                    string framename = frames[i].SubName;
                    card.Find("Button").GetComponent<Button>().onClick.AddListener(() => {
                        UserInfo.instance.EqipFrame(framename);
                        UserInfo.instance.SaveUserFrame(()=> { });
                        UserFrameInfoSetting();
                    });
                }
            }

            // 가지고 있는 프레임이라면 
            if (UserInfo.instance.IsExistFrame(frames[i].SubName))
            {
                card.Find("Lock").gameObject.SetActive(false);

                buffText.text += frames[i].AbilityType + " : " + frames[i].AbilityCount + "\n";
            }
            else // 아니라면
            {
                card.Find("Lock").gameObject.SetActive(true);
            }
        } 
    }


}
