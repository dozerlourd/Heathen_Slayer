using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Assassin_Variable
{
    [Header(" - Related to Assassin's Skill")]
    [SerializeField] internal GameObject Skill_PoisonDart;
    [SerializeField] internal GameObject Skill_PoisonExplosion;
    [SerializeField] internal GameObject[] Skill_PoisonArea;

    [Header(" - Related to Assassin's attack")]
    [SerializeField] internal Collider2D[] attackCols;
    [SerializeField] internal Transform dartFirePos;

    [SerializeField] internal int[] maxSkillEffectPoolCounts;
    [SerializeField, Range(0f, 1f)] internal float effectTiming;

    [Header(" - Check Distance")]
    [SerializeField] internal float blinkDist;

    internal GameObject[] skillEffects_1;
    internal GameObject[] skillEffects_2;

    internal EnemyHP enemyHP;

    internal Coroutine Co_Patterns;
}

public class AssassinFSM : EnemyFSM
{
    #region Variable

    Assassin_Variable assassin_Variable;

    #endregion

    #region Unity Life Cycle

    private new void Awake()
    {
        
    }

    private new void Start()
    {
        
    }

    #endregion

    #region Implementation Place

    protected override IEnumerator Co_Pattern()
    {
        yield return null;
    }

    #endregion
}
