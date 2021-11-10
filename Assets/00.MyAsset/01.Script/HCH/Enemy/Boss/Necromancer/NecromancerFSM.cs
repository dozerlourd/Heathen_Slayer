using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Necromancer_Variable
{
    [Header(" - Related to Shaman's attack")]
    [SerializeField] internal Collider2D[] attackCols;
    [SerializeField] internal Transform _FirePos;

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

    internal Shaman_PoisonArea[] poisonArea;

    internal GameObject[] skillEffects_PoisonDart;
    internal GameObject[] skillEffects_PoisonExplosion;

    internal BossHP_Shaman bossHP;

    internal Coroutine Co_Patterns;
}

public class NecromancerFSM : EnemyFSM, IIdle, ITrace, IAttack_1, ISkill_1, ISkill_2, ISkill_3
{
    #region Variable

    [Space(30)]
    [SerializeField] Necromancer_Variable necromancer_Variable;

    #endregion

    protected override IEnumerator Co_Pattern()
    {
        while(true)
        {
            yield return EnemyTrace();
        }
    }

    public IEnumerator EnemyIdle()
    {
        yield return null;
    }

    public IEnumerator EnemyTrace()
    {
        while (GetDistanceB2WPlayer() > attackRange)
        {
            FlipCheck();
            //anim.SetBool("IsWalk", true);
            yield return null;
            transform.Translate(Vector2.right * flipValue * moveSpeed * Time.deltaTime);
            yield return null;
        }
        //anim.SetBool("IsWalk", false);
    }

    public IEnumerator EnemyAttack_1()
    {
        yield return null;
    }

    public IEnumerator EnemySkill_1()
    {
        yield return null;
    }

    public IEnumerator EnemySkill_2()
    {
        yield return null;
    }

    public IEnumerator EnemySkill_3()
    {
        yield return null;
    }
}
