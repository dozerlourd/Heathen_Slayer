using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rogue_Variable
{
    [Header(" - Related to Assassin's Skill")]
    //[SerializeField] internal GameObject Skill_;

    [Header(" - Related to Assassin's attack")]
    [SerializeField] internal Collider2D[] attackCols;

    //[SerializeField] internal int[] maxSkillEffectPoolCounts;
    [SerializeField, Range(0f, 1f)] internal float effectTiming;

    [Header(" - Check Distance")]
    [SerializeField] internal float blinkDist;

    [SerializeField] internal float IdleDelayTime;

    internal int pace;

    internal GameObject[] skillEffects_1;
    internal GameObject[] skillEffects_2;

    internal EnemyHP enemyHP;

    internal Coroutine Co_Patterns;
}

public class RogueFSM : EnemyFSM, IIdle, ITrace, IAttack_1, IAttack_2, ISkill_1, ISkill_2, ISkill_3
{
    #region Variable

    [SerializeField]
    Rogue_Variable rogue_Variable;

    float tempVelocityY = 0;
    bool isFall = false;

    #endregion

    #region Property

    EnemyHP EnemyHP => rogue_Variable.enemyHP = rogue_Variable.enemyHP ? rogue_Variable.enemyHP : GetComponent<EnemyHP>();

    #endregion

    #region Unity Life Cycle

    private new void Awake()
    {
        base.Awake();
    }

    private new void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Co_Pattern());
    }

    private void Update()
    {
        anim.SetBool("IsGround", isGround);

        if (!isFall && !isGround && anim.GetCurrentAnimatorStateInfo(0).IsName("Rogue_Jump") || anim.GetCurrentAnimatorStateInfo(0).IsName("Rogue_Run"))
        {
            isFall = true;
            anim.SetTrigger("ToFall");
        }

        float tempY = transform.position.y - tempVelocityY;
        if (Mathf.Abs(tempY) <= 0.01f) tempY = 0;
        anim.SetFloat("VelocityY", tempY);

        if (!isGround)
            tempVelocityY = transform.position.y;
        else
            isFall = false;
    }

    #endregion

    #region Implementation Place

    protected override IEnumerator Co_Pattern()
    {
        while (EnemyHP.NormalizedCurrHP >= 0.7f)
        {
            yield return StartCoroutine(Pattern_1());
        }
        yield return new WaitForSeconds(2.0f);
        while (EnemyHP.NormalizedCurrHP >= 0.4f)
        {
            yield return StartCoroutine(Pattern_2());
        }
        yield return new WaitForSeconds(2.0f);
        while (true)
        {
            yield return StartCoroutine(Pattern_3());
        }
    }

    #region Patterns

    IEnumerator Pattern_1()
    {
        yield return EnemyIdle();
        // 나중에 랜덤으로 공격 패턴 실행하게끔
        yield return EnemyAttack_2();
        yield return EnemyIdle();
        yield return EnemyAttack_2();
        yield return EnemyIdle();
        yield return EnemyAttack_1();
        yield return EnemyIdle();
        yield return EnemyAttack_2();
        yield return EnemyIdle();
        yield return EnemyAttack_1();
        yield return EnemyIdle();
        yield return EnemySkill_1();
        yield return EnemyIdle();
        yield return EnemyAttack_2();
        yield return EnemyIdle();
        yield return EnemyAttack_2();
        yield return EnemyIdle();
        yield return EnemySkill_1();
        yield return EnemyIdle();
    }

    IEnumerator Pattern_2()
    {
        yield return null;
    }

    IEnumerator Pattern_3()
    {
        yield return null;
    }

    #endregion

    public IEnumerator EnemyIdle()
    {
        yield return new WaitForSeconds(rogue_Variable.IdleDelayTime);
    }

    public IEnumerator EnemyTrace()
    {
        while (GetDistanceB2WPlayer() > attackRange)
        {
            FlipCheck();
            anim.SetBool("IsWalk", true);
            yield return null;
            transform.Translate(Vector2.right * flipValue * moveSpeed * Time.deltaTime);
            yield return null;
        }
        anim.SetBool("IsWalk", false);
    }

    public IEnumerator EnemyAttack_1()
    {
        yield return StartCoroutine(EnemyTrace());
        FlipCheck();

        //print("Attack_1");
        anim.SetTrigger("ToAttack_1");
        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.56f);
        rogue_Variable.attackCols[0].enabled = true;
        //(PolygonCollider2D)attackCols[0].
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.67f);
        rogue_Variable.attackCols[0].enabled = false;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Rogue_Idle"));
    }

    public IEnumerator EnemyAttack_2()
    {
        yield return StartCoroutine(EnemyTrace());
        FlipCheck();

        anim.SetTrigger("ToAttack_2");
        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.56f);
        rogue_Variable.attackCols[1].enabled = true;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.67f);
        rogue_Variable.attackCols[1].enabled = false;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Rogue_Idle"));
    }

    public IEnumerator EnemySkill_1()
    {
        anim.SetTrigger("ToSkill_VanishAttack");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f);
        transform.position = PlayerSystem.Instance.Player.transform.position + Vector3.up * 3f;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.65f);
        rogue_Variable.attackCols[1].enabled = true;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.72f);
        rogue_Variable.attackCols[1].enabled = false;
        FlipCheck();
        yield return null;
    }

    public IEnumerator EnemySkill_2()
    {
        throw new NotImplementedException();
    }

    public IEnumerator EnemySkill_3()
    {
        throw new NotImplementedException();
    }

    #endregion
}
