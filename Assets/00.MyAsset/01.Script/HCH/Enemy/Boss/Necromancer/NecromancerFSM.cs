using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Necromancer_Variable
{
    [Header(" - Related to Necromancer's attack")]
    [SerializeField] internal Collider2D[] attackCols;
    [SerializeField] internal Transform[] _FirePos;

    [Header(" - Related to Necromancer's Skill")]
    [SerializeField] internal GameObject Skill_DeathBolt;

    [Tooltip("0 => Dart's Count \n 1 => Explosion's Count"), Space(5)]
    [SerializeField] internal int[] maxSkillEffectPoolCounts;

    [Tooltip("The timing of the animation where each skill will be activated"), Space(5)]
    [SerializeField, Range(0f, 1f)] internal float attackEffectTiming = 0.45f;
    [SerializeField, Range(0f, 1f)] internal float[] skillEffectTiming;

    [Header(" - Elapsed Time For State Change"), Min(0.0f)]
    [SerializeField] internal float IdleDelayTime;

    [Header(" - Sound")]
    [SerializeField] internal AudioClip[] voiceClips;
    [SerializeField] internal AudioClip[] attackVoiceClips;

    internal BossHP_Necromancer bossHP;

    internal Coroutine Co_Patterns;
}

public class NecromancerFSM : EnemyFSM, IIdle, ITrace, IAttack_1, ISkill_1, ISkill_2, ISkill_3
{
    #region Variable

    [Space(30)]
    [SerializeField] Necromancer_Variable necromancer_Variable;

    GameObject[] skillEffects_DeathBolt;

    #endregion

    #region Property

    BossHP_Necromancer BossHP => necromancer_Variable.bossHP = necromancer_Variable.bossHP ? necromancer_Variable.bossHP : GetComponent<BossHP_Necromancer>();

    #endregion

    #region Unity Life Cycle

    private new void Start()
    {

    }

    private new void Awake()
    {
        base.Awake();
        for (int i = 0; i < necromancer_Variable.attackCols.Length; i++)
        {
            necromancer_Variable.attackCols[i].enabled = false;
        }

        #region Generate ObjectPool
        skillEffects_DeathBolt = new GameObject[necromancer_Variable.maxSkillEffectPoolCounts[0]];

        skillEffects_DeathBolt = HCH.GameObjectPool.GeneratePool(necromancer_Variable.Skill_DeathBolt, skillEffects_DeathBolt.Length, FolderSystem.Instance.Necromancer_SkillPool);
        #endregion
    }

    #endregion

    #region Implementation Place

    protected override IEnumerator Co_Pattern()
    {
        while(true)
        {
            if (BossHP.NormalizedCurrHP >= 0.7f)
                yield return StartCoroutine(Pattern_1());

            else if (BossHP.NormalizedCurrHP >= 0.3f)
                yield return StartCoroutine(Pattern_2());

            else
                yield return StartCoroutine(Pattern_3());
        }
    }

    #region Patterns

    IEnumerator Pattern_1()
    {
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
        yield return new WaitForSeconds(necromancer_Variable.IdleDelayTime);
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
        FlipCheck();

        anim.SetTrigger("ToAttack");
        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.25f);
        SoundManager.Instance.PlayVoiceOneShot(necromancer_Variable.attackVoiceClips);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= necromancer_Variable.attackEffectTiming);
        necromancer_Variable.attackCols[0].enabled = true;

        Vector3[] attackPos = new Vector3[1];
        for (int i = 0; i < necromancer_Variable._FirePos.Length; i++) attackPos[i] = necromancer_Variable._FirePos[i].position;

        GameObject[] deathBolts = HCH.GameObjectPool.PopObjectsFromPool(skillEffects_DeathBolt, 1, attackPos);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.67f);
        necromancer_Variable.attackCols[0].enabled = false;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Necromancer_Idle"));
    }

    public IEnumerator EnemySkill_1()
    {
        FlipCheck();

        anim.SetTrigger("ToAttack");
        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.25f);
        SoundManager.Instance.PlayVoiceOneShot(necromancer_Variable.attackVoiceClips);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= necromancer_Variable.attackEffectTiming);
        necromancer_Variable.attackCols[0].enabled = true;

        Vector3[] attackPos = new Vector3[5];
        for (int i = 0; i < necromancer_Variable._FirePos.Length; i++) attackPos[i] = necromancer_Variable._FirePos[i].position;

        GameObject[] deathBolts = HCH.GameObjectPool.PopObjectsFromPool(skillEffects_DeathBolt, 5, attackPos);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.67f);
        necromancer_Variable.attackCols[0].enabled = false;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Necromancer_Idle"));
    }

    public IEnumerator EnemySkill_2()
    {

        yield return null;
    }

    public IEnumerator EnemySkill_3()
    {
        yield return null;
    }

    #endregion
}
