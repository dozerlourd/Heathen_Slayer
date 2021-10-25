using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HPControllerToEnemy : MonoBehaviour
{
    #region Variable

    [SerializeField] protected float maxHP;

    protected float currHP;

    #endregion

    #region Property
    public float MaxHP => maxHP;


    public float CurrHP { get => currHP;
        protected set
        {
            currHP = value;
            EnemyDamaged();
            RefreshUI(value);
        }
    }

    #endregion

    #region Unity Life Cycle



    #endregion

    #region Implementation Place

    public void TakeDamage(float _damage) => CurrHP -= _damage;

    protected abstract void RefreshUI(float _val);

    /// <summary> When Enemy Taking Damage, Generate this method </summary>
    protected abstract void EnemyDamaged();

    #endregion
}
