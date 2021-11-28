using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonData : MonoBehaviour
{
    Transform initPos;

    int enemyCount = 0;
    bool isClearThisRoom = false;

    public bool IsClearThisRoom => isClearThisRoom;

    #region Implemetation Place

    #region Getter
    public int GetEnemyCount() => enemyCount;
    #endregion

    #region Setter
    public void SetCount(int count) => enemyCount = count; /*transform.GetComponentsInChildren<HPControllerToEnemy>().Length;*/

    public void DungeonClear() => isClearThisRoom = true;
    #endregion

    public int MinusEnemyCount() => enemyCount--;

    #endregion
}
