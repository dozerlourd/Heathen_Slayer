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

    [Header(" - Related to Bringer's skill")]
    [SerializeField] GameObject skill_GodHand;
    [SerializeField] int maxSkillEffectPoolCount;
    [SerializeField, Range(0f, 1f)] float skillEffectTiming;

    GameObject[] skillEffects;

    [Space(15)]
    [Tooltip("Max Aggro Duration: After end duration, change to patrol")]
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

    private new void Start()
    {
        base.Start();

        skillEffects = new GameObject[maxSkillEffectPoolCount];
        for (int i = 0; i < 5; i++)
        {
            skillEffects[i] = Instantiate(skill_GodHand);
            skillEffects[i].name = skill_GodHand.name;
            skillEffects[i].GetComponent<Bringer_GodHand>().SetDamage(skillAttackDmg[0]);
            skillEffects[i].transform.SetParent(FolderSystem.Instance.Bringer_SkillPool);
            skillEffects[i].SetActive(false);
        }

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
        //print("나 가만히 있는다!");
        anim.SetTrigger("ToIdle");
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator EnemyPatrol()
    {
        
        float randomDirTime = 0;
        Vector2 moveVec = RandomVec();

        //print("나 움직인다!");
        anim.SetTrigger("ToWalk");
        anim.SetFloat("WalkSpeed", 0.45f);

        while (true)
        {
            spriteRenderer.flipX = moveVec == Vector2.right ? true : false;

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

        //print("나 너 쫒아간다!");
        anim.SetFloat("WalkSpeed", 1.35f);
        while (true)
        {
            FlipCheck();
            if (GetDistanceB2WPlayer() < attackRange)
            {
                anim.SetBool("ToWalk", false);
                traceCount = 0;
                if (HCH.Well512.Next() > HCH.Well512.Next())
                    yield return StartCoroutine(EnemyAttack_1());
                else
                    yield return StartCoroutine(EnemySkill_1());

                yield return StartCoroutine(EnemyIdle());
            }
            else if (GetDistanceB2WPlayer() < detectRange)
            {
                anim.SetBool("ToWalk", true);
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
                anim.SetBool("ToWalk", true);
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
        FlipCheck();
        print("나 너 때린다!");
        anim.SetTrigger("ToAttack");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Attack"));

        anim.SetFloat("AttackSpeed", 0.85f);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.35f);
        anim.SetFloat("AttackSpeed", 1.2f);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f);
        anim.SetFloat("AttackSpeed", 0.85f);


        yield return waitToAttack;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Idle"));
    }

    public IEnumerator EnemySkill_1()
    {
        FlipCheck();
        //print("이거 아프다!");

        anim.SetTrigger("ToSkill");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Cast"));

        anim.SetFloat("AttackSpeed", 0.65f);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= skillEffectTiming);
        //print("이거 받아라!");
        GodHand();

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f);
        anim.SetFloat("AttackSpeed", 0.3f);

        yield return waitToSkill;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Idle"));
    }

    Vector2 RandomVec() => HCH.Well512.Next() > HCH.Well512.Next() ? Vector2.right : Vector2.left;

    public void SetAttackSpeed() => anim.SetFloat("AttackSpeed", attackSpeed);

    /// <summary> Use Bringer's Skill </summary>
    void GodHand()
    {
        for (int i = 0; i < skillEffects.Length; i++)
        {
            if(!skillEffects[i].activeInHierarchy)
            {
                skillEffects[i].SetActive(true);
                skillEffects[i].transform.position = PlayerSystem.Instance.Player.transform.position + Vector3.up * 2.5f;

                break;
            }
        }
    }

    #endregion
}
