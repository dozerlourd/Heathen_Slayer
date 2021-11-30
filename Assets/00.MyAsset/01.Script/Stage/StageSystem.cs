using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSystem : MonoBehaviour
{
    #region Singleton

    public static StageSystem Instance;
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


    [SerializeField] List<StageData> StageDatas = new List<StageData>();
    private int currStageIndex;

    public StageData CurrStage => StageDatas[currStageIndex];

    //private void Start()
    //{
    //    print($"currDungeonIndex - {StageDatas[0].GetCurrDungeonIndex()}");
    //    print($"DungeonCount - {StageDatas[0].GetDungeonCount()}");
    //}

    public int GetCurrStageIndex() => currStageIndex;
    public int GetStageCount() => StageDatas.Count;

    /// <summary>
    /// 보스를 격파한 후 실행되야할 함수
    /// </summary>
    public void NextStage()
    {
        //다음 스테이지로 가는 문(포탈) 활성화
        if (GetCurrStageIndex() < GetStageCount())
        {
            currStageIndex++;
        }
        else
        {
            print("을리윽세스");
        }
    }
}
