using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rogue_Variable
{
    [Header(" - Related to Assassin's Skill ----- VanishAttack")]
    [SerializeField] internal GameObject effect_VanishAttack;
    [SerializeField] internal int vanishCount;
    [SerializeField] internal Transform vanishEffectTr;

    [Header(" - Related to Assassin's Skill ----- Shuriken")]
    [SerializeField] internal GameObject Skill_Shuriken;
    [SerializeField] internal int shurikenCount;
    [SerializeField] internal Transform shurikenTr;
    [SerializeField] internal float shurikenSpeed;
    [SerializeField] internal float shurikenDelayTime;

    [Header(" - Related to Assassin's attack")]
    [SerializeField] internal Collider2D[] attackCols;

    [SerializeField, Range(0f, 1f)] internal float effectTiming;

    [Header(" - Check Distance")]
    [SerializeField] internal float blinkDist;

    [SerializeField] internal float IdleDelayTime;

    [Header(" - Sound")]
    [SerializeField] internal AudioClip[] voiceClips;
    [SerializeField] internal AudioClip[] attackVoiceClips;
    [SerializeField] internal AudioClip[] skillVoiceClips_Vanish;
    [SerializeField] internal AudioClip[] skillVoiceClips_Shuriken;

    internal EnemyHP enemyHP;

    internal GameObject[] skillEffects_Shuriken;

    internal GameObject[] effect_VanishAttacks;

    internal Coroutine Co_Patterns;

    internal float tempVelocityY = 0;

    internal bool isFall = false;
}

public class RogueFSM : EnemyFSM, IIdle, ITrace, IAttack_1, IAttack_2, ISkill_1, ISkill_2, ISkill_3
{
    #region Variable

    [SerializeField]
    Rogue_Variable rogue_Variable;

    #endregion

    #region Property

    EnemyHP EnemyHP => rogue_Variable.enemyHP = rogue_Variable.enemyHP ? rogue_Variable.enemyHP : GetComponent<EnemyHP>();

    #endregion

    #region Unity Life Cycle

    private new void Awake()
    {
        base.Awake();

        rogue_Variable.skillEffects_Shuriken = new GameObject[rogue_Variable.shurikenCount];
        rogue_Variable.effect_VanishAttacks = new GameObject[rogue_Variable.vanishCount];

        rogue_Variable.skillEffects_Shuriken = HCH.GameObjectPool.GeneratePool(rogue_Variable.Skill_Shuriken, rogue_Variable.shurikenCount, FolderSystem.Instance.Rogue_SkillPool);

        rogue_Variable.effect_VanishAttacks = HCH.GameObjectPool.GeneratePool(rogue_Variable.effect_VanishAttack, rogue_Variable.vanishCount, FolderSystem.Instance.Rogue_SkillPool);
    }

    new void Update()
    {
        base.Update();
        anim.SetBool("IsGround", isGround);

        if (!rogue_Variable.isFall && !isGround && anim.GetCurrentAnimatorStateInfo(0).IsName("Rogue_Jump") || anim.GetCurrentAnimatorStateInfo(0).IsName("Rogue_Run"))
        {
            rogue_Variable.isFall = true;
            anim.SetTrigger("ToFall");
        }

        float tempY = transform.position.y - rogue_Variable.tempVelocityY;
        if (Mathf.Abs(tempY) <= 0.01f) tempY = 0;
        anim.SetFloat("VelocityY", tempY);

        if (!isGround)
            rogue_Variable.tempVelocityY = transform.position.y;
        else
            rogue_Variable.isFall = false;
    }

    #endregion

    #region Implementation Place

    protected override IEnumerator Co_Pattern()
    {
        yield return new WaitForSeconds(waitStart);

        while (true)
        {
            if (EnemyHP.NormalizedCurrHP >= 0.7f)
                yield return StartCoroutine(Pattern_1());

            else if (EnemyHP.NormalizedCurrHP >= 0.3f)
                yield return StartCoroutine(Pattern_2());

            else
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
        yield return EnemySkill_2();
        yield return EnemyIdle();
        yield return EnemyAttack_2();
        yield return EnemyIdle();
        yield return EnemyAttack_1();
        yield return EnemyIdle();
        yield return EnemySkill_1();
        yield return EnemyIdle();
        yield return EnemyAttack_2();
        yield return EnemyIdle();
        yield return EnemySkill_2();
        yield return EnemyIdle();
        yield return EnemySkill_2();
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

            yield return StartCoroutine(Move());
            //print(GetDistanceB2WPlayerYValue());
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
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.25f);
        SoundManager.Instance.PlayVoiceOneShot(rogue_Variable.attackVoiceClips);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.56f);
        //rogue_Variable.attackCols[0].enabled = true;
        //(PolygonCollider2D)attackCols[0].
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.67f);
        //rogue_Variable.attackCols[0].enabled = false;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Rogue_Idle"));
    }

    public IEnumerator EnemyAttack_2()
    {
        yield return StartCoroutine(EnemyTrace());

        anim.SetTrigger("ToAttack_2");
        yield return null;
        FlipCheck();
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.25f);
        SoundManager.Instance.PlayVoiceOneShot(rogue_Variable.attackVoiceClips);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.56f);
        rogue_Variable.attackCols[0].enabled = true;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.67f);
        rogue_Variable.attackCols[0].enabled = false;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Rogue_Idle"));
    }

    public IEnumerator EnemySkill_1()
    {
        EnemyHP.SetAbsolute(true);
        anim.SetBool("IsAirOpensive", true);
        anim.SetTrigger("ToSkill_VanishAttack");
        yield return null;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f);
        SoundManager.Instance.PlayVoiceOneShot(rogue_Variable.skillVoiceClips_Vanish);
        transform.position = PlayerSystem.Instance.Player.transform.position + Vector3.up * 3f;
        FlipCheck();

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f);
        rogue_Variable.attackCols[1].enabled = true;

        HCH.GameObjectPool.PopObjectFromPool(rogue_Variable.effect_VanishAttacks, rogue_Variable.vanishEffectTr.position);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.82f);
        rogue_Variable.attackCols[1].enabled = false;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f);

        EnemyHP.SetAbsolute(false);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Rogue_Idle"));
        anim.SetBool("IsAirOpensive", false);
        yield return null;
    }

    public IEnumerator EnemySkill_2()
    {
        EnemyHP.SetAbsolute(true);
        anim.SetTrigger("ToSkill_Shuriken");
        yield return null;
        FlipCheck();
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.25f);
        SoundManager.Instance.PlayVoiceOneShot(rogue_Variable.skillVoiceClips_Shuriken);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f);

        GameObject shuriken = HCH.GameObjectPool.PopObjectFromPool(rogue_Variable.skillEffects_Shuriken, rogue_Variable.shurikenTr.position);
        shuriken.GetComponent<Rigidbody2D>().velocity = new Vector2(flipValue * rogue_Variable.shurikenSpeed, 0);
        shuriken.TryGetComponent(out SpriteRenderer spRenderer);
        if (spRenderer) spRenderer.flipX = spriteRenderer.flipX;

        EnemyHP.SetAbsolute(false);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Rogue_Idle"));
        yield return new WaitForSeconds(rogue_Variable.shurikenDelayTime);
    }

    public IEnumerator EnemySkill_3()
    {
        throw new NotImplementedException();
    }

    #endregion
}
