using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_ShamanFSM : EnemyFSM, IIdle, ITrace, IAttack_1, IAttack_2, ISkill_1, ISkill_2, ISkill_3
{
    #region Variable

    [Header(" - Related to Shaman's Skill")]
    [SerializeField] GameObject Skill_PoisonDart;
    [SerializeField] GameObject Skill_PoisonExplosion;
    [SerializeField] GameObject[] Skill_PoisonArea;

    [Tooltip("0 => Dart \n 1 => Explosion")]
    [SerializeField] int[] maxSkillEffectPoolCounts;
    [SerializeField, Range(0f, 1f)] float[] skillEffectTiming;
    [Tooltip("The speed of moving darts")]
    [SerializeField] float dartSpeed;

    [Header(" - Elapsed Time For State Change"), Min(0.0f)]
    [SerializeField] float IdleDelayTime;
    [SerializeField] float dartDelayTime;
    [SerializeField] float areaSkillTime;

    [Header(" - Check Distance")]
    [SerializeField] float teleportingDist;

    [Header(" - Variable objects for attack")]
    [SerializeField] Collider2D[] attackCols;
    [SerializeField] Transform dartFirePos;

    ShamanPoisonArea[] poisonArea;

    GameObject[] skillEffects_PoisonDart;
    GameObject[] skillEffects_PoisonExplosion;

    BossHP bossHP;

    Coroutine Co_Patterns;

    #endregion

    #region Property

    BossHP BossHP => bossHP = bossHP ? bossHP : GetComponent<BossHP>();

    #endregion

    #region Unity Life Cycle

    private new void Start()
    {
        base.Start();
        for (int i = 0; i < attackCols.Length; i++)
        {
            attackCols[i].enabled = false;
        }

        #region Generate ObjectPool
        skillEffects_PoisonDart = new GameObject[maxSkillEffectPoolCounts[0]];
        skillEffects_PoisonExplosion = new GameObject[maxSkillEffectPoolCounts[1]];
        poisonArea = new ShamanPoisonArea[Skill_PoisonArea.Length];

        skillEffects_PoisonDart = HCH.Pool.GeneratePool(Skill_PoisonDart, maxSkillEffectPoolCounts[0], FolderSystem.Instance.Shaman_SkillPool);
        skillEffects_PoisonExplosion = HCH.Pool.GeneratePool(Skill_PoisonExplosion, maxSkillEffectPoolCounts[1], FolderSystem.Instance.Shaman_SkillPool);

        // Poison Area
        for (int i = 0; i < Skill_PoisonArea.Length; i++)
        {
            Skill_PoisonArea[i].transform.SetParent(FolderSystem.Instance.Shaman_SkillPool);
            Skill_PoisonArea[i].SetActive(false);

            poisonArea[i] = Skill_PoisonArea[i].GetComponent<ShamanPoisonArea>();
        }
        #endregion

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
        yield return EnemySkill_2();
        yield return EnemyAttack_2();
        yield return EnemyIdle();
        yield return EnemyAttack_2();
        yield return EnemyIdle();
        yield return EnemyAttack_1();
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
        print("Idle");
        anim.SetTrigger("ToIdle");
        yield return new WaitForSeconds(IdleDelayTime);
    }

    public IEnumerator EnemyTrace()
    {
        //print("Trace");
        while (GetDistanceB2WPlayer() > attackRange)
        {
            if(GetDistanceB2WPlayer() > teleportingDist)
            {
                transform.position = playerPos + Vector2.up * 2f;
                yield return StartCoroutine(EnemyAttack_1());
                break;
            }
            FlipCheck();
            anim.SetBool("ToWalk", true);
            yield return null;
            transform.Translate(Vector2.right * flipValue * moveSpeed * 2.25f * Time.deltaTime);
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
        attackCols[0].enabled = true;
        //(PolygonCollider2D)attackCols[0].
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.53f);
        attackCols[0].enabled = false;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Shaman_Idle"));
    }

    public IEnumerator EnemyAttack_2()
    {
        yield return StartCoroutine(EnemyTrace());
        FlipCheck();

        anim.SetTrigger("ToAttack_2");
        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.36f);
        attackCols[1].enabled = true;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.41f);
        attackCols[1].enabled = false;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.56f);
        attackCols[1].enabled = true;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.61f);
        attackCols[1].enabled = false;

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
        GameObject dartClone = HCH.Pool.PopObjectFromPool(skillEffects_PoisonDart);
        dartClone.transform.position = dartFirePos.position;
        dartClone.GetComponent<Rigidbody2D>().velocity = new Vector2(flipValue * dartSpeed, 0);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Shaman_Idle"));
        yield return new WaitForSeconds(dartDelayTime);
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

    public IEnumerator EnemySkill_3()
    {
        yield return null;
    }

    public IEnumerator Area_1()
    {
        for (int i = 0; i < poisonArea.Length; i++)
        {
            poisonArea[i].gameObject.SetActive(true);
            poisonArea[i].TogglePoisonArea();
        }
        yield return new WaitForSeconds(3);

        for (int i = 0; i < poisonArea.Length; i++)
        {
            poisonArea[i].TogglePoisonArea();
        }
        yield return new WaitForSeconds(2);

        for (int i = 0; i < poisonArea.Length; i++)
        {
            poisonArea[i].TogglePoisonArea();
        }
        yield return new WaitForSeconds(3);

        for (int i = 0; i < poisonArea.Length; i++)
        {
            poisonArea[i].TogglePoisonArea();
            poisonArea[i].gameObject.SetActive(false);
        }
    }

    public IEnumerator Area_2()
    {
        for (int i = 0; i < poisonArea.Length; i++)
        {
            poisonArea[i].gameObject.SetActive(!poisonArea[i].gameObject.activeInHierarchy);
            poisonArea[i].TogglePoisonArea();
        }
        yield return new WaitForSeconds(1);
    }

    #endregion
}
