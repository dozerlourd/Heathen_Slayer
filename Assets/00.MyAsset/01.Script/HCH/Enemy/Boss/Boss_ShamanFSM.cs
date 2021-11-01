using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shaman_Variable
{
    [Header(" - Related to Shaman's Skill")]
    [SerializeField] internal GameObject Skill_PoisonDart;
    [SerializeField] internal GameObject Skill_PoisonExplosion;
    [SerializeField] internal GameObject[] Skill_PoisonArea;

    [Header(" - Related to Shaman's attack")]
    [SerializeField] internal Collider2D[] attackCols;
    [SerializeField] internal Transform dartFirePos;

    [Tooltip("0 => Dart \n 1 => Explosion")]
    [SerializeField] internal int[] maxSkillEffectPoolCounts;
    [SerializeField, Range(0f, 1f)] internal float[] skillEffectTiming;
    [Tooltip("The speed of moving darts")]
    [SerializeField] internal float dartSpeed;

    [Header(" - Elapsed Time For State Change"), Min(0.0f)]
    [SerializeField] internal float IdleDelayTime;
    [SerializeField] internal float dartDelayTime;
    [SerializeField] internal float areaSkillTime;

    [Header(" - Check Distance")]
    [SerializeField] internal float teleportingDist;

    internal Shaman_PoisonArea[] poisonArea;

    internal GameObject[] skillEffects_PoisonDart;
    internal GameObject[] skillEffects_PoisonExplosion;

    internal BossHP_Shaman bossHP;

    internal Coroutine Co_Patterns;
}

public class Boss_ShamanFSM : EnemyFSM, IIdle, ITrace, IAttack_1, IAttack_2, ISkill_1, ISkill_2, ISkill_3
{
    #region Variable

    [Space(30)]
    [SerializeField] Shaman_Variable shaman_Variable;

    #endregion

    #region Property

    BossHP_Shaman BossHP => shaman_Variable.bossHP = shaman_Variable.bossHP ? shaman_Variable.bossHP : GetComponent<BossHP_Shaman>();

    #endregion

    #region Unity Life Cycle

    private new void Awake()
    {
        base.Awake();   
        for (int i = 0; i < shaman_Variable.attackCols.Length; i++)
        {
            shaman_Variable.attackCols[i].enabled = false;
        }
        
        #region Generate ObjectPool
        shaman_Variable.skillEffects_PoisonDart = new GameObject[shaman_Variable.maxSkillEffectPoolCounts[0]];
        shaman_Variable.skillEffects_PoisonExplosion = new GameObject[shaman_Variable.maxSkillEffectPoolCounts[1]];
        shaman_Variable.poisonArea = new Shaman_PoisonArea[shaman_Variable.Skill_PoisonArea.Length];

        shaman_Variable.skillEffects_PoisonDart = HCH.Pool.GeneratePool(shaman_Variable.Skill_PoisonDart, shaman_Variable.maxSkillEffectPoolCounts[0], FolderSystem.Instance.Shaman_SkillPool);
        shaman_Variable.skillEffects_PoisonExplosion = HCH.Pool.GeneratePool(shaman_Variable.Skill_PoisonExplosion, shaman_Variable.maxSkillEffectPoolCounts[1], FolderSystem.Instance.Shaman_SkillPool);

        // Poison Area
        for (int i = 0; i < shaman_Variable.Skill_PoisonArea.Length; i++)
        {
            shaman_Variable.Skill_PoisonArea[i].transform.SetParent(FolderSystem.Instance.Shaman_SkillPool);
            shaman_Variable.Skill_PoisonArea[i].SetActive(false);

            shaman_Variable.poisonArea[i] = shaman_Variable.Skill_PoisonArea[i].GetComponent<Shaman_PoisonArea>();
        }
        #endregion
    }

    private new void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Co_Pattern());
    }

    #endregion

    #region Implementation Place 

    protected override IEnumerator Co_Pattern()
    {
        while(BossHP.NormalizedCurrHP >= 0.7f)
        {
            yield return StartCoroutine(Pattern_1());
        }
        anim.SetTrigger("isStunned");
        yield return new WaitForSeconds(2.0f);
        while(BossHP.NormalizedCurrHP >= 0.4f)
        {
            yield return StartCoroutine(Pattern_2());
        }
        anim.SetTrigger("isStunned");
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
        yield return EnemySkill_1();
        yield return EnemySkill_2(Area_1(5));
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
        print("Idle");
        anim.SetTrigger("ToIdle");
        yield return new WaitForSeconds(shaman_Variable.IdleDelayTime);
    }

    public IEnumerator EnemyTrace()
    {
        //print("Trace");
        while (GetDistanceB2WPlayer() > attackRange)
        {
            if(GetDistanceB2WPlayer() > shaman_Variable.teleportingDist)
            {
                transform.position = playerPos + Vector2.up * 2f;
                yield return StartCoroutine(EnemyAttack_1());
                break;
            }
            FlipCheck();
            anim.SetBool("ToWalk", true);
            yield return null;
            transform.Translate(Vector2.right * flipValue * moveSpeed * Time.deltaTime);
            yield return null;
        }
        anim.SetBool("ToWalk", false);
    }

    public IEnumerator EnemyAttack_1()
    {
        yield return StartCoroutine(EnemyTrace());
        FlipCheck();

        //print("Attack_1");
        anim.SetTrigger("ToAttack_1");
        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.46f);
        shaman_Variable.attackCols[0].enabled = true;
        //(PolygonCollider2D)attackCols[0].
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.53f);
        shaman_Variable.attackCols[0].enabled = false;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Shaman_Idle"));
    }

    public IEnumerator EnemyAttack_2()
    {
        yield return StartCoroutine(EnemyTrace());
        FlipCheck();

        anim.SetTrigger("ToAttack_2");
        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.36f);
        shaman_Variable.attackCols[1].enabled = true;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.41f);
        shaman_Variable.attackCols[1].enabled = false;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.56f);
        shaman_Variable.attackCols[1].enabled = true;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.61f);
        shaman_Variable.attackCols[1].enabled = false;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Shaman_Idle"));
    }

    public IEnumerator EnemySkill_1()
    {
        FlipCheck();
        yield return null;

        //print("Skill_Dart");
        anim.SetTrigger("ToSkill_Dart");
        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.625f);
        GameObject dartClone = HCH.Pool.PopObjectFromPool(shaman_Variable.skillEffects_PoisonDart);
        dartClone.transform.position = shaman_Variable.dartFirePos.position;
        dartClone.GetComponent<Rigidbody2D>().velocity = new Vector2(flipValue * shaman_Variable.dartSpeed, 0);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Shaman_Idle"));
        yield return new WaitForSeconds(shaman_Variable.dartDelayTime);
    }

    public IEnumerator EnemySkill_2()
    {
        print("Skill_Area");
        anim.SetBool("ToSkill_Area", true);
        anim.SetTrigger("SkillStart");
        yield return null;

        yield return Area_2();

        anim.SetBool("ToSkill_Area", false);
    }

    public IEnumerator EnemySkill_2(IEnumerator Co)
    {
        print("Skill_Area");
        anim.SetBool("ToSkill_Area", true);
        anim.SetTrigger("SkillStart");
        yield return null;

        yield return Co;

        anim.SetBool("ToSkill_Area", false);
    }

    public IEnumerator EnemySkill_3()
    {
        yield return null;
    }

    public IEnumerator Area_1(float duration)
    {
        for (int i = 0; i < shaman_Variable.poisonArea.Length; i++)
        {
            shaman_Variable.poisonArea[i].gameObject.SetActive(true);
            shaman_Variable.poisonArea[i].TogglePoisonArea();
        }
        yield return new WaitForSeconds(duration);

        for (int i = 0; i < shaman_Variable.poisonArea.Length; i++)
        {
            shaman_Variable.poisonArea[i].TogglePoisonArea();
            shaman_Variable.poisonArea[i].gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(1);
    }

    public IEnumerator Area_2()
    {
        for (int i = 0; i < shaman_Variable.poisonArea.Length; i++)
        {
            shaman_Variable.poisonArea[i].gameObject.SetActive(true);
            shaman_Variable.poisonArea[i].TogglePoisonArea();
        }
        yield return new WaitForSeconds(1);
    }

    #endregion
}
