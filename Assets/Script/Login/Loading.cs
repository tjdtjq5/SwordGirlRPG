using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public GameObject obj;
    public Image fore;
    public Text progressText;

    public GameObject touchPannel;

    IEnumerator loadingCoroutine;

    public void LoadingOpen()
    {
        obj.SetActive(true);

        fore.fillAmount = 0;
        progressText.text = "Loading ...0%";
        loadingCoroutine = LoadingCoroutine();
        StartCoroutine(loadingCoroutine);
    }

    IEnumerator LoadingCoroutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.02f);

        while (fore.fillAmount < 0.99f)
        {
            fore.fillAmount += 0.02f;
            progressText.text = "Loading ..."+ (int)(fore.fillAmount*100) + "%";
            yield return waitTime;
        }

        LoadChart(() => {
            obj.SetActive(false);

            touchPannel.SetActive(true);
            touchPannel.GetComponent<Button>().onClick.RemoveAllListeners();
            touchPannel.GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("Main"); });
        });
    }

    void LoadChart(System.Action callback)
    {
        CharacterEnhanceAbilityChart.instance.LoadChart(() => {
            MasicMissileChart.instance.LoadChart(() => {
                GachaChart.instance.LoadChart(() => {
                    WeaponeChart.instance.LoadChart(() => {
                        RelicChart.instance.LoadChart(() => {
                            ClothChart.instance.LoadChart(() => {
                                NomalMonsterChart.instance.LoadChart(() => {
                                    BossMonsterChart.instance.LoadChart(() => {
                                        DayByQuestChart.instance.LoadChart(() => {
                                            WeekQuestChart.instance.LoadChart(() => {
                                                UserInfo.instance.Load(() => { callback(); });
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    }
}
