using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shaman_Variable
{
    [Header(" - Related to Shaman's attack")]
    [SerializeField] internal Collider2D[] attackCols;
    [SerializeField] internal Transform dartFirePos;

    [Header(" - Related to Shaman's Skill")]
    [SerializeField] internal GameObject Skill_PoisonDart;
    [SerializeField] internal GameObject Skill_PoisonExplosion;
    [SerializeField] internal GameObject[] Skill_PoisonArea;

    [Tooltip("0 => Dart's Count \n 1 => Explosion's Count"), Space(5)]
    [SerializeField] internal int[] maxSkillEffectPoolCounts;

    [Tooltip("The timing of the animation where each skill will be activated"), Space(5)]
    [SerializeField, Range(0f, 1f)] internal float[] skillEffectTiming;

    [Tooltip("The speed of moving darts"), Space(5)]
    [SerializeField] internal float dartSpeed;

    [Tooltip("Related Explosion"), Space(5)]
    [SerializeField, Min(0.0f)] internal float explosionInterval;

    [Header(" - Elapsed Time For State Change"), Min(0.0f)]
    [SerializeField] internal float IdleDelayTime;
    [SerializeField] internal float dartDelayTime;
    [SerializeField] internal float areaSkillTime;

    [Header(" - Check Distance")]
    [SerializeField] internal float dartDist;

    internal BossHP_Shaman bossHP;

    internal Coroutine Co_Patterns;
}

public class ShamanFSM : EnemyFSM, IIdle, ITrace, IAttack_1, IAttack_2, ISkill_1, ISkill_2, ISkill_3
{
    #region Variable

    [Space(30)]
    [SerializeField] Shaman_Variable shaman_Variable;

    WaitForSeconds expInterval;

    Shaman_PoisonArea[] poisonArea;

    GameObject[] skillEffects_PoisonDart;
    GameObject[] skillEffects_PoisonExplosion;

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
        skillEffects_PoisonDart = new GameObject[shaman_Variable.maxSkillEffectPoolCounts[0]];
        skillEffects_PoisonExplosion = new GameObject[shaman_Variable.maxSkillEffectPoolCounts[1]];
        //poisonArea = new Shaman_PoisonArea[shaman_Variable.Skill_PoisonArea.Length];

        skillEffects_PoisonDart = HCH.GameObjectPool.GeneratePool(shaman_Variable.Skill_PoisonDart, shaman_Variable.maxSkillEffectPoolCounts[0], FolderSystem.Instance.Shaman_SkillPool);
        skillEffects_PoisonExplosion = HCH.GameObjectPool.GeneratePool(shaman_Variable.Skill_PoisonExplosion, shaman_Variable.maxSkillEffectPoolCounts[1], FolderSystem.Instance.Shaman_SkillPool);

        // Poison Area

        //for (int i = 0; i < shaman_Variable.Skill_PoisonArea.Length; i++)
        //{
        //    shaman_Variable.Skill_PoisonArea[i].transform.SetParent(FolderSystem.Instance.Shaman_SkillPool);
        //    shaman_Variable.Skill_PoisonArea[i].SetActive(false);

        //    poisonArea[i] = shaman_Variable.Skill_PoisonArea[i].GetComponent<Shaman_PoisonArea>();
        //}
        #endregion
    }

    private new void Start()
    {
        expInterval = new WaitForSeconds(shaman_Variable.explosionInterval);
    }

    #endregion

    #region Implementation Place 

    protected override IEnumerator Co_Pattern()
    {
        yield return new WaitForSeconds(waitStart);

        while (true)
        {
            if (BossHP.NormalizedCurrHP >= 0.5f)
                yield return StartCoroutine(Pattern_1());

            else
            {
                yield return EnemySkill_2(ActivateArea(false));
                yield return StartCoroutine(Pattern_2());
            }
        }
    }

    #region Patterns

    IEnumerator Pattern_1()
    {
        yield return EnemyIdle();
        yield return EnemyAttack_2();
        yield return EnemyAttack_1();
        yield return EnemyAttack_2();
        //yield return EnemySkill_2(FiniteActivateArea(5));
    }

    IEnumerator Pattern_2()
    {
        //yield return EnemySkill_2(ActivateArea(true));

        yield return EnemyAttack_1();
        yield return EnemyAttack_1();
        //yield return EnemySkill_2(ActivateArea(false));

        yield return EnemyAttack_2();
        yield return EnemyAttack_1();

    }

    IEnumerator Pattern_3()
    {
        yield return EnemySkill_3(4, 6);
        yield return EnemySkill_3(3, 4);
    }

    #endregion

    public IEnumerator EnemyIdle()
    {
        yield return new WaitForSeconds(shaman_Variable.IdleDelayTime);
    }

    public IEnumerator EnemyTrace()
    {

        while (GetDistanceB2WPlayer() > attackRange)
        {
            if (GetDistanceB2WPlayer() > shaman_Variable.dartDist)
            {
                //transform.position = playerPos + Vector2.up * 2f;
                //yield return StartCoroutine(EnemyAttack_1());
                //break;
                FlipCheck();
                yield return EnemySkill_1();
            }
            FlipCheck();
            yield return StartCoroutine(Move());
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
        yield return EnemyIdle();
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
        yield return EnemyIdle();
    }

    public IEnumerator EnemySkill_1()
    {
        FlipCheck();
        yield return null;

        //print("Skill_Dart");
        anim.SetTrigger("ToSkill_Dart");
        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= shaman_Variable.skillEffectTiming[0]);
        GameObject dartClone = HCH.GameObjectPool.PopObjectFromPool(skillEffects_PoisonDart);
        dartClone.transform.position = shaman_Variable.dartFirePos.position;
        dartClone.GetComponent<Rigidbody2D>().velocity = new Vector2(flipValue * shaman_Variable.dartSpeed, 0);
        dartClone.TryGetComponent(out SpriteRenderer spRenderer);
        if (spRenderer) spRenderer.flipX = spriteRenderer.flipX;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Shaman_Idle"));
        yield return new WaitForSeconds(shaman_Variable.dartDelayTime);
    }

    public IEnumerator EnemySkill_2()
    {
        yield return EnemySkill_2(ActivateArea());
    }

    public IEnumerator EnemySkill_2(IEnumerator Co)
    {
        //print("Skill_Area");
        anim.SetBool("ToSkill_Area", true);
        anim.SetTrigger("SkillStart");
        yield return null;

        yield return Co;

        anim.SetBool("ToSkill_Area", false);
    }

    public IEnumerator FiniteActivateArea(float _duration)
    {
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Shaman_Skill_Area"));

        for (int i = 0; i < poisonArea.Length; i++)
        {
            poisonArea[i].gameObject.SetActive(true);
            poisonArea[i].TogglePoisonArea();
        }
        yield return new WaitForSeconds(_duration);

        for (int i = 0; i < poisonArea.Length; i++)
        {
            poisonArea[i].TogglePoisonArea();
            poisonArea[i].gameObject.SetActive(false);
        }
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
    }

    public IEnumerator ActivateArea(bool _isActive = true)
    {
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Shaman_Skill_Area"));

        for (int i = 0; i < poisonArea.Length; i++)
        {
            poisonArea[i].gameObject.SetActive(_isActive);
            poisonArea[i].TogglePoisonArea();
        }
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
    }

    public IEnumerator EnemySkill_3()
    {
        yield return EnemySkill_3(4);
    }

    public IEnumerator EnemySkill_3(int _patternNum, int skillCount = 5)
    {
        //print("Skill_Explosion");
        anim.SetBool("ToSkill_Explosion", true);
        anim.SetTrigger("SkillStart");

        switch (_patternNum)
        {
            case 1:
                yield return ExplosionPattern_1();
                break;
            case 2:
                yield return ExplosionPattern_2();
                break;
            case 3:
                yield return ExplosionPattern_3(skillCount);
                break;
            case 4:
                yield return ExplosionPattern_4(skillCount);
                break;
        }
    }

    public IEnumerator ExplosionPattern_1()
    {
        yield return null;
    }

    public IEnumerator ExplosionPattern_2()
    {
        yield return null;
    }

    public IEnumerator ExplosionPattern_3(int _count)
    {
        yield return new WaitForSeconds(1f);
        while (_count > 0)
        {
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                Vector3 effectPos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, 1921), Random.Range(0, 1081)));
                effectPos.z = 0;
                HCH.GameObjectPool.PopObjectFromPool(skillEffects_PoisonExplosion, effectPos);
                yield return new WaitForSeconds(Random.Range(0.05f, 0.25f));
            }
            _count--;
            yield return expInterval;
        }
    }

    public IEnumerator ExplosionPattern_4(int _count)
    {
        yield return new WaitForSeconds(2f);
        while (_count > 0)
        {
            Vector3 effectPos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, 1921), Random.Range(0, 1081)));
            effectPos.z = 0;
            HCH.GameObjectPool.PopObjectFromPool(skillEffects_PoisonExplosion, effectPos);
            _count--;
            yield return expInterval;
        }
    }

    #endregion
}
