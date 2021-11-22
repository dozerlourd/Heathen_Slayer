using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData : MonoBehaviour
{
    [SerializeField] List<DungeonData> DungeonDatas = new List<DungeonData>();
    private int currDungeonIndex;

    public DungeonData CurrDungeon => DungeonDatas[currDungeonIndex];

    #region Implemetation Place

    public void NextDungeon()
    {
        //���� �������� ���� ��(��Ż) Ȱ��ȭ
        if (GetCurrDungeonIndex() < GetDungeonCount())
        {
            currDungeonIndex++;
        }
    }

    public void MinusEnemyCount()
    {
        CurrDungeon.MinusEnemyCount();
        if (CurrDungeon.GetEnemyCount() <= 0)
        {
            NextDungeon();
        }
    }

    #region Getter
    public int GetDungeonCount() => DungeonDatas.Count - 1;
    public int GetCurrDungeonIndex() => currDungeonIndex;
    #endregion

    #endregion
}
