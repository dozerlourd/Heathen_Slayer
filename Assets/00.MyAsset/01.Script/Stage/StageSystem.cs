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
    /// ������ ������ �� ����Ǿ��� �Լ�
    /// </summary>
    public void NextStage()
    {
        //���� ���������� ���� ��(��Ż) Ȱ��ȭ
        if (GetCurrStageIndex() < GetStageCount())
        {
            currStageIndex++;
        }
        else
        {
            print("����������");
        }
    }
}
