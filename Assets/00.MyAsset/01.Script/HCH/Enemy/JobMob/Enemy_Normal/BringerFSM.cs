using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bringer_Variable
{
    [Header(" - Elapsed Time For State Change"), Min(0.0f)]
    [SerializeField] internal float waitToPatrolTime;
    [SerializeField] internal float waitToTraceTime;
    [SerializeField] internal float waitToAttackTime;
    [SerializeField] internal float waitToSkillTime;

    [Header(" - Related to Bringer's skill")]
    [SerializeField] internal GameObject skill_GodHand;
    [SerializeField] internal int maxSkillEffectPoolCount;
    [SerializeField, Range(0f, 1f)] internal float skillEffectTiming;
    [SerializeField] internal float skillWeight = 800;

    [Header(" - Related to Bringer's attack")]
    [SerializeField] internal Collider2D attackCol;

    [Space(15)]
    [Tooltip("Max Aggro Duration: After end duration, change to patrol")]
    [SerializeField] internal float aggroDuration = 10;

    [Header(" - Sound")]
    [SerializeField] internal AudioClip[] attackVoiceClips;
    [SerializeField] internal AudioClip[] skillEffectClips_GodHand;

    internal GameObject[] skillEffects;

    internal WaitForSeconds waitToPatrol, waitToTrace, waitToAttack, waitToSkill;

    internal Coroutine Co_Patrol, Co_Trace, Co_Attack;

    internal EnemyHP enemyHP;
}

public class BringerFSM : EnemyFSM, IIdle, IPatrol, ITrace, IAttack_1, ISkill_1
{
    #region Variable

    [SerializeField] Bringer_Variable bringer_Variable;

    [SerializeField] float yValue = 6;

    #endregion

    #region Property
    EnemyHP EnemyHP => bringer_Variable.enemyHP = bringer_Variable.enemyHP ? bringer_Variable.enemyHP : GetComponent<EnemyHP>();

    #endregion

    #region Unity Life Cycle

    /// <summary>
    /// 
    /// </summary>
    new void Awake()
    {
        base.Awake();
        bringer_Variable.waitToPatrol = new WaitForSeconds(bringer_Variable.waitToPatrolTime);
        bringer_Variable.waitToTrace = new WaitForSeconds(bringer_Variable.waitToTraceTime);
        bringer_Variable.waitToAttack = new WaitForSeconds(bringer_Variable.waitToAttackTime);
        bringer_Variable.waitToSkill = new WaitForSeconds(bringer_Variable.waitToSkillTime);
    }

    private new void Start()
    {
        base.Start();
        bringer_Variable.skillEffects = new GameObject[bringer_Variable.maxSkillEffectPoolCount];
        bringer_Variable.skillEffects = HCH.GameObjectPool.GeneratePool(bringer_Variable.skill_GodHand, bringer_Variable.maxSkillEffectPoolCount, FolderSystem.Instance.Bringer_SkillPool, false);
    }

    #endregion

    #region Implementation Place 

    protected override IEnumerator Co_Pattern()
    {
        yield return new WaitForSeconds(waitStart);

        bringer_Variable.Co_Patrol = StartCoroutine(EnemyPatrol());
        yield return new WaitUntil(() => GetDistanceB2WPlayer() <= playerDetectRange);
        bringer_Variable.Co_Trace = StartCoroutine(EnemyTrace());
        StopCoroutine(bringer_Variable.Co_Patrol);
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

        anim.SetBool("IsWalk", true);
        //anim.SetFloat("WalkSpeed", 0.45f);

        while (true)
        {
            //if (GetDistanceB2WPlayer() <= detectRange)
            //{
            //    bringer_Variable.Co_Trace = StartCoroutine(EnemyTrace());
            //    StopCoroutine(bringer_Variable.Co_Patrol);
            //}

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
            if (!IsNotWall)
            {
                moveVec *= -1;
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }

            transform.Translate(moveVec * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator EnemyTrace()
    {
        float traceCount = 0;

        while (true)
        {
            //if(GetDistanceB2WPlayerYValue() > yValue)
            //{
            //    anim.SetBool("ToWalk", false);
            //    if(bringer_Variable.Co_Trace != null) StopCoroutine(bringer_Variable.Co_Trace);
            //    yield return new WaitForSeconds(2f);
            //    bringer_Variable.Co_Patrol = StartCoroutine(EnemyPatrol());
            //}
            FlipCheck();
            if (GetDistanceB2WPlayer() < attackRange)
            {
                anim.SetBool("IsWalk", false);
                traceCount = 0;
                if (new HCH_Random.MersenneTwister().Genrand_Int32(3) < bringer_Variable.skillWeight)
                    yield return bringer_Variable.Co_Attack = StartCoroutine(EnemyAttack_1());
                else
                    yield return bringer_Variable.Co_Attack = StartCoroutine(EnemySkill_1());

                yield return StartCoroutine(EnemyIdle());
            }
            else if (GetDistanceB2WPlayer() < playerDetectRange)
            {
                //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Walk"))
                //{
                //    yield return null;
                //    continue;
                //}

                traceCount = 0;
                yield return StartCoroutine(Move());
            }
            else
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Walk"))
                {
                    yield return null;
                    continue;
                }

                if (traceCount <= bringer_Variable.aggroDuration)
                {
                    yield return StartCoroutine(Move());

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
        //print("나 너 때린다!");
        anim.SetTrigger("ToAttack");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Attack"));

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f);
        SoundManager.Instance.PlayVoiceOneShot(bringer_Variable.attackVoiceClips);
        anim.SetFloat("AttackSpeed", 0.2f);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f);
        anim.SetFloat("AttackSpeed", 1.05f);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Damaged")) { StopCoroutine(bringer_Variable.Co_Attack); }
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.53f);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Damaged")) { StopCoroutine(bringer_Variable.Co_Attack); }
        bringer_Variable.attackCol.enabled = true;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.57f);
        bringer_Variable.attackCol.enabled = false;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f);
        anim.SetFloat("AttackSpeed", 0.85f);


        yield return bringer_Variable.waitToAttack;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Idle"));
    }

    public IEnumerator EnemySkill_1()
    {
        EnemyHP.SetAbsolute(true);

        FlipCheck();
        //print("이거 아프다!");

        anim.SetTrigger("ToSkill");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Cast"));

        anim.SetFloat("AttackSpeed", 0.65f);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Damaged")) { StopCoroutine(bringer_Variable.Co_Attack); }
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= bringer_Variable.skillEffectTiming);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Damaged")) { StopCoroutine(bringer_Variable.Co_Attack); }
        GodHand();

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f);
        anim.SetFloat("AttackSpeed", 0.3f);

        EnemyHP.SetAbsolute(false);

        yield return bringer_Variable.waitToSkill;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Idle"));
    }

    Vector2 RandomVec() => new HCH_Random.MersenneTwister().Genrand_Int32(1) % 2 == 0 ? Vector2.right : Vector2.left;

    public void SetAttackSpeed() => anim.SetFloat("AttackSpeed", attackSpeed);

    /// <summary> Use Bringer's Skill </summary>
    void GodHand() => HCH.GameObjectPool.PopObjectFromPool(bringer_Variable.skillEffects, PlayerSystem.Instance.Player.transform.position + Vector3.up * 2.5f);

    #endregion
}
