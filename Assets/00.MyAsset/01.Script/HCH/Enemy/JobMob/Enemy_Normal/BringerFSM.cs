using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerFSM : EnemyFSM, IIdle, IPatrol, ITrace, IAttack_1, ISkill_1
{
    #region Variable

    [Header(" - Elapsed Time For State Change"), Min(0.0f)]
    [SerializeField] float waitToPatrolTime;
    [SerializeField] float waitToTraceTime;
    [SerializeField] float waitToAttackTime;
    [SerializeField] float waitToSkillTime;

    [Header("Max Aggro Duration: After end duration, change to patrol")]
    [SerializeField] float aggroDuration = 10;

    WaitForSeconds waitToPatrol, waitToTrace, waitToAttack, waitToSkill;
    Coroutine Co_Patrol, Co_Trace;

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
        yield return StartCoroutine(EnemyIdle());
        while (true)
        {
            Co_Patrol = StartCoroutine(EnemyPatrol());
            yield return new WaitUntil(() => GetDistanceB2WPlayer() <= detectRange);
            StopCoroutine(Co_Patrol);
            yield return Co_Trace = StartCoroutine(EnemyTrace());
        }
    }

    public IEnumerator EnemyIdle()
    {
        print("나 가만히 있는다!");
        anim.SetTrigger("ToIdle");
        yield return new WaitForSeconds(3.0f);
    }

    public IEnumerator EnemyPatrol()
    {
        float randomDirTime = 0;
        Vector2 moveVec = RandomVec();

        print("나 움직인다!");
        anim.SetTrigger("ToWalk");
        anim.SetFloat("WalkSpeed", 0.45f);

        while (true)
        {
            if (randomDirTime < Random.Range(3.5f, 5f))
            {
                randomDirTime += Time.deltaTime;
            }
            else
            {
                if (Random.Range(0, 5) <= 2) yield return StartCoroutine(EnemyIdle());
                randomDirTime = 0;
                moveVec = RandomVec();
            }
            transform.Translate(moveVec * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator EnemyTrace()
    {
        float traceCount = 0;

        print("나 너 쫒아간다!");
        anim.SetFloat("WalkSpeed", 1.35f);
        while (true)
        {
            if (GetDistanceB2WPlayer() < attackRange)
            {
                traceCount = 0;
                if (HCH.Well512.Next() > HCH.Well512.Next())
                    yield return StartCoroutine(EnemyAttack_1());
                else
                    yield return StartCoroutine(EnemySkill_1());
            }
            else if (GetDistanceB2WPlayer() < detectRange)
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Walk"))
                {
                    yield return null;
                    continue;
                }

                traceCount = 0;
                transform.Translate(Vector2.right * (PlayerSystem.Instance.Player.transform.position - transform.position).normalized.x * moveSpeed * 2.25f * Time.deltaTime);
            }
            else
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Walk"))
                {
                    yield return null;
                    continue;
                }

                if (traceCount <= aggroDuration)
                {
                    transform.Translate(Vector2.right * (PlayerSystem.Instance.Player.transform.position - transform.position).normalized.x * moveSpeed * 2.25f * Time.deltaTime);
                    traceCount += Time.deltaTime;
                }
                else break;
            }
            yield return null;
        }
    }

    public IEnumerator EnemyAttack_1()
    {
        print("나 너 때린다!");
        anim.SetTrigger("ToAttack");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Idle"));
    }

    public IEnumerator EnemySkill_1()
    {
        print("이거 아프다!");
        anim.SetTrigger("ToSkill");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Idle"));
    }

    Vector2 RandomVec() => HCH.Well512.Next() > HCH.Well512.Next() ? Vector2.right : Vector2.left;
    public void SetAttackSpeed() => anim.SetFloat("AttackSpeed", attackSpeed);
    public void GetDamage()
    {

    }

    #endregion
}
