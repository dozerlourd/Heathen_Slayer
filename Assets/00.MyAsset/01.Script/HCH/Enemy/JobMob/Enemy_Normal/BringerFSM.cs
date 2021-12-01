using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bringer_Variable
{
    [Header(" - Related to Bringer's skill")]
    [SerializeField] internal GameObject skill_GodHand;
    [SerializeField] internal int maxSkillEffectPoolCount;
    [SerializeField, Range(0f, 1f)] internal float skillEffectTiming;
    [SerializeField] internal float skillWeight = 800;

    [Header(" - Related to Bringer's attack")]
    [SerializeField] internal Collider2D attackCol;
    [SerializeField] internal float waitToAttack, waitToSkill;

    [Space(15)]
    [Tooltip("Max Aggro Duration: After end duration, change to patrol")]
    [SerializeField] internal float aggroDuration = 10;

    [Header(" - Sound")]
    [SerializeField] internal AudioClip[] attackVoiceClips;
    [SerializeField] internal AudioClip[] skillEffectClips_GodHand;

    internal GameObject[] skillEffects;


    internal Coroutine Co_Patrol, Co_Trace, Co_Attack;

    internal EnemyHP enemyHP;
}

public class BringerFSM : EnemyFSM, IIdle, IPatrol, ITrace, IAttack_1, ISkill_1
{
    #region Variable

    [SerializeField] Bringer_Variable bringer_Variable;

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
        while (true)
        {
            print("start");
            bringer_Variable.Co_Patrol = StartCoroutine(EnemyPatrol());
            yield return new WaitUntil(() => GetDistanceB2WPlayer() <= playerDetectRange);
            StopCoroutine(bringer_Variable.Co_Patrol);
            yield return bringer_Variable.Co_Trace = StartCoroutine(EnemyTrace());
            print("end");
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
        float randomPatrolTime = Random.Range(3.5f, 5f);
        Vector2 moveVec = RandomVec();

        while (true)
        {
            spriteRenderer.flipX = moveVec == Vector2.right ? true : false;

            if (randomDirTime < randomPatrolTime)
            {
                randomDirTime += Time.deltaTime;
            }
            else
            {
                randomPatrolTime = Random.Range(3.5f, 5f);
                randomDirTime = 0;
                moveVec = RandomVec();
            }

            if (!IsNotWall)
            {
                moveVec *= -1;
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }

            anim.SetBool("IsWalk", moveVec != Vector2.zero);
            transform.Translate(moveVec * moveSpeed * 0.5f * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator EnemyTrace()
    {
        float traceCount = 0;

        while (true)
        {
            FlipCheck();
            if (GetDistanceB2WPlayer() < attackRange)
            {
                anim.SetBool("IsWalk", false);
                traceCount = 0;
                if (HCH_Random.Random.Genrand_Int32(3) < bringer_Variable.skillWeight)
                    yield return bringer_Variable.Co_Attack = StartCoroutine(EnemyAttack_1());
                else
                    yield return bringer_Variable.Co_Attack = StartCoroutine(EnemySkill_1());

                yield return StartCoroutine(EnemyIdle());
            }
            else if (GetDistanceB2WPlayer() < playerDetectRange)
            {
                traceCount = 0;
                anim.SetBool("IsWalk", true);
                yield return StartCoroutine(Move());
            }
            else
            {
                //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Bringer_Walk"))
                //{
                //    yield return null;
                //    continue;
                //}

                if (traceCount <= bringer_Variable.aggroDuration)
                {
                    anim.SetBool("IsWalk", true);
                    yield return StartCoroutine(Move());

                    traceCount += Time.deltaTime;
                }
                else
                {
                    yield break;
                }
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
        anim.SetFloat("AttackSpeed", 0.75f);

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

    Vector2 RandomVec() => HCH_Random.Random.Genrand_Int32(1) <= 3 ? Vector2.zero : HCH_Random.Random.Genrand_Int32(1) >= 6 ? Vector2.right : Vector2.left;

    public void SetAttackSpeed() => anim.SetFloat("AttackSpeed", attackSpeed);

    /// <summary> Use Bringer's Skill </summary>
    void GodHand() => HCH.GameObjectPool.PopObjectFromPool(bringer_Variable.skillEffects, PlayerSystem.Instance.Player.transform.position + Vector3.up * 2.5f);

    #endregion
}
