using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "DungeonScene")
        {
            StartCoroutine(EntryTheStage());
        }
    }

    IEnumerator EntryTheStage()
    {
        for (int i = 0; i < StageSystem.Instance.GetStageCount(); i++)
        {
            for (int j = 0; j < StageSystem.Instance.CurrStage.GetDungeonCount(); j++)
            {

            }
        }
        yield return new WaitUntil(() => StageSystem.Instance.CurrStage.CurrDungeon.GetEnemyCount() == 0);
    }
}
