using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonData : MonoBehaviour
{
    int enemyCount = 0;

    #region Implemetation Place

    #region Getter
    public int GetEnemyCount() => enemyCount;
    #endregion

    #region Setter
    public void SetCount(int count) => enemyCount = count; /*transform.GetComponentsInChildren<HPControllerToEnemy>().Length;*/
    #endregion

    public int MinusEnemyCount() => enemyCount--;

    #endregion
}
