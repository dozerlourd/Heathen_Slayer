using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonData : MonoBehaviour
{
    [SerializeField] Transform enemiesFloder;
    [SerializeField] bool isBossRoom = false;

    [SerializeField] Transform initPos;

    int enemyCount = 0;
    bool isClearThisRoom = false;
    bool isNext = false;
    bool isJoin = false;

    public bool IsBossRoom => isBossRoom;
    public bool IsClearThisRoom => isClearThisRoom;
    public bool IsNext { get => isNext; set => isNext = value; }
    public bool IsJoin { get => isJoin; set => isJoin = value; }

    public Transform InitPos => initPos;

    #region Unity Life Cycle

    private void Start()
    {
        for (int i = 0; i < enemiesFloder.childCount; i++)
        {
            enemiesFloder.GetChild(i).gameObject.SetActive(false);
        }
    }

    #endregion

    #region Implemetation Place

    #region Getter
    public int GetEnemyCount() => enemyCount;

    #endregion

    #region Setter
    public void SetCount(int count) => enemyCount = count; /*transform.GetComponentsInChildren<HPControllerToEnemy>().Length;*/

    public void OnEnemies()
    {
        for (int i = 0; i < enemiesFloder.childCount; i++)
        {
            enemiesFloder.GetChild(i).gameObject.SetActive(true);
        }
        SetCount(enemiesFloder.childCount);
    }

    public void DungeonClear() => isClearThisRoom = true;
    #endregion

    public int MinusEnemyCount() => enemyCount--;

    #endregion
}
