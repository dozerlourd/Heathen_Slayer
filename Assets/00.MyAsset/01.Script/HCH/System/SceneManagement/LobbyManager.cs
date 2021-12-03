using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{

    #region Button Event Method

    public void OnEnterDungeonButton()
    {
        StartCoroutine(OnButtonStart());
    }

    IEnumerator OnButtonStart()
    {
        yield return StartCoroutine(SceneEffectSystem.Instance.FadeOutCoroutine());
        SceneManager.LoadScene("DungeonScene");
    }
    public void OnClickQuitButton()
    {
        Application.Quit();
    }

    #endregion
}
