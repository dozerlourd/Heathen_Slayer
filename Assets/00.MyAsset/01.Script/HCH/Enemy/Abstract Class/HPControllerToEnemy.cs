using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HPControllerToEnemy : MonoBehaviour
{
    #region Variable

    [SerializeField] protected float maxHP;

    protected float currHP;

    protected EnemyFSM enemyFSM;
    protected Animator animator;

    #endregion

    #region Property
    public float MaxHP => maxHP;

    public float CurrHP { get => currHP;
        protected set
        {
            currHP = value < 0 ? 0 : value;
            RefreshUI(value);
            if (NormalizedCurrHP <= 0) Animator.SetTrigger("ToDie");
            else EnemyDamaged();
        }
    }

    public float NormalizedCurrHP => CurrHP / MaxHP;

    protected EnemyFSM EnemyFSM => enemyFSM = enemyFSM ? enemyFSM : GetComponent<EnemyFSM>();
    protected Animator Animator => animator = animator ? animator : GetComponent<Animator>();

    #endregion

    #region Unity Life Cycle

    protected void OnEnable()
    {
        CurrHP = MaxHP;
    }

    #endregion

    #region Implementation Place

    public void TakeDamage(float _damage) => CurrHP -= _damage;
    
    protected abstract void RefreshUI(float _val);

    /// <summary> When Enemy Taking Damage, Generate this method </summary>
    protected abstract void EnemyDamaged();

    #endregion
}
