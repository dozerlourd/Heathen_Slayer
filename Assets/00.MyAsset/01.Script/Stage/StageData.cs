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
        //다음 던전으로 가는 문(포탈) 활성화
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
            print("던전 클리어");
            CurrDungeon.DungeonClear();
        }
    }

    #region Getter
    public int GetDungeonCount() => DungeonDatas.Count;
    public int GetCurrDungeonIndex() => currDungeonIndex;
    #endregion

    #endregion
}
