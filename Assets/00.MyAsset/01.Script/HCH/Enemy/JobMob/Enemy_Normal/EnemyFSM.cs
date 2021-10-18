using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyFSM : MonoBehaviour
{
    #region Variable

    [SerializeField] float basicAttackDmg = 10;
    [Header("Max 3 Skills")]
    [SerializeField] float[] skillAttackDmg = new float[3];
    [SerializeField] float atkSpeed = 1.0f, moveSpeed = 1.0f;

    #endregion

    #region Property



    #endregion

    #region Unity Life Cycle

    void Start()
    {

    }

    void Update()
    {

    }

    #endregion

    #region Implementation Place 



    #endregion
}
