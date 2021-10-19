using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerFSM : EnemyFSM, IIdle, IPatrol, ITrace, IAttack_1, ISkill_1
{
    #region Variable

    [SerializeField] float waitToPatrolTime, waitToTraceTime, waitToAttackTime, waitToSkillTime;
    WaitForSeconds waitToPatrol, waitToTrace, waitToAttack, waitToSkill;

    #endregion

    #region Property



    #endregion

    #region Unity Life Cycle

    new void Awake()
    {
        base.Awake();
        waitToPatrol = new WaitForSeconds(waitToPatrolTime);
        waitToTrace = new WaitForSeconds(waitToTraceTime);
        waitToAttack = new WaitForSeconds(waitToAttackTime);
        waitToSkill = new WaitForSeconds(waitToSkillTime);
    }

    void Start()
    {
        StartCoroutine(Co_Pattern());
    }

    #endregion

    #region Implementation Place 

    protected override IEnumerator Co_Pattern()
    {
        while(true)
        {
            yield return StartCoroutine(EnemyIdle());
            yield return waitToTrace;
            // 나머지 패턴 기술
            yield return null;
        }
    }

    public IEnumerator EnemyIdle()
    {
        yield return null;
    }

    public IEnumerator EnemyPatrol()
    {
        yield return null;
    }

    public IEnumerator EnemyTrace()
    {
        yield return null;
    }

    public IEnumerator EnemyAttack_1()
    {
        yield return null;
    }

    public IEnumerator EnemySkill_1()
    {
        yield return null;
    }

    #endregion
}
