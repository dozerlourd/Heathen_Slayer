using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HPControllerToEnemy : MonoBehaviour
{
    #region Variable

    [SerializeField] protected float maxHP;
    [SerializeField] protected float corpseTime;

    protected bool isDead = false;

    protected float currHP;

    protected EnemyFSM enemyFSM;
    protected Animator animator;

    protected Coroutine Co_Dead;

    #endregion

    #region Property
    public float MaxHP => maxHP;

    public float CurrHP
    {
        get => currHP;
        protected set
        {
            if (!isDead)
            {
                float damage = currHP - value;
                currHP = value < 0 ? 0 : value;
                RefreshUI();
            
                if (NormalizedCurrHP <= 0)
                {
                    EnemyFSM.StopAllCoroutines();
                    Animator.SetTrigger("ToDie");
                    Co_Dead = StartCoroutine(EnemyDead());
                }
                else if(damage > 0) StartCoroutine(EnemyDamaged());
            }
        }
    }

    public float NormalizedCurrHP => CurrHP / MaxHP;

    protected EnemyFSM EnemyFSM => enemyFSM = enemyFSM ? enemyFSM : GetComponent<EnemyFSM>();
    protected Animator Animator => animator = animator ? animator : GetComponent<Animator>();

    public bool IsDead => isDead;

    #endregion

    #region Unity Life Cycle

    protected void Awake()
    {
        CurrHP = MaxHP;
    }

    protected void OnEnable()
    {
        CurrHP = MaxHP;
        GetComponent<Collider2D>().enabled = true;
        isDead = false;
    }

    #endregion

    #region Implementation Place

    public void TakeDamage(float _damage)
    {
        if (isDead) return;

        DamageUISystem.Instance.DisplayDamageText(_damage, transform);
        CurrHP -= _damage;
    }

    protected abstract void RefreshUI();

    /// <summary> When Enemy Taking Damage, Generate this method </summary>
    protected abstract IEnumerator EnemyDamaged();

    protected virtual IEnumerator EnemyDead()
    {
        StageSystem.Instance.CurrStage.MinusEnemyCount();
        //print("GetCurrDungeonIndex - " + StageSystem.Instance.CurrStage.GetCurrDungeonIndex());
        //print("GetEnemyCount - " + StageSystem.Instance.CurrStage.CurrDungeon.GetEnemyCount());
        yield return null;
    }

    #endregion
}
