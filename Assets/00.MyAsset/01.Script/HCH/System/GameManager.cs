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

    #region Variable

    Coroutine Co_EntryTheStage;

    #endregion

    private void Start()
    {
        Co_EntryTheStage = StartCoroutine(EntryTheStage());
    }

    IEnumerator EntryTheStage()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "DungeonScene");



        for (int i = 0; i < StageSystem.Instance.GetStageCount(); i++)
        {
            for (int j = 0; j < StageSystem.Instance.CurrStage.GetDungeonCount(); j++)
            {
                if (StageSystem.Instance.CurrStage.CurrDungeon.IsBossRoom)
                {
                    GameObject go = Instantiate(Resources.Load("BossHPBarCanvas")) as GameObject;
                    go.name = "BossHPBarCanvas";
                }
                PlayerSystem.Instance.Player.transform.position = StageSystem.Instance.CurrStage.CurrDungeon.InitPos.position;

                yield return new WaitForSeconds(0.1f);

                yield return StartCoroutine(SceneEffectSystem.Instance.FadeInCoroutine());

                // 던전 패턴을 생성 or 던전 패턴을 만들어놓고 좌표로 이동
                StageSystem.Instance.CurrStage.CurrDungeon.OnEnemies();

                StageSystem.Instance.CurrStage.CurrDungeon.IsJoin = true;

                yield return new WaitUntil(() => StageSystem.Instance.CurrStage.CurrDungeon.GetEnemyCount() == 0 &&
                                                 StageSystem.Instance.CurrStage.CurrDungeon.IsClearThisRoom &&
                                                 StageSystem.Instance.CurrStage.CurrDungeon.IsNext);

                if (!StageSystem.Instance.CurrStage.CurrDungeon.IsBossRoom) {
                    StageSystem.Instance.CurrStage.NextDungeon();
                }

                yield return StartCoroutine(SceneEffectSystem.Instance.FadeOutCoroutine());
            }
            if (StageSystem.Instance.GetStageCount() == StageSystem.Instance.GetCurrStageIndex())
            {
                //을리윽세스 패널 띄우기
                print("을리윽세스");
            }
            else
            {
                StageSystem.Instance.NextStage();
            }
        }
    }
}
