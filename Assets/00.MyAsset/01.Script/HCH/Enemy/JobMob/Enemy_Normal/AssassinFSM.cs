using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Assassin_Variable
{
    [Header(" - Related to Assassin's Skill")]
    //[SerializeField] internal GameObject Skill_;

    [Header(" - Related to Assassin's attack")]
    [SerializeField] internal Collider2D[] attackCols;

    //[SerializeField] internal int[] maxSkillEffectPoolCounts;
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

    [SerializeField]
    Assassin_Variable assassin_Variable;

    #endregion

    #region Unity Life Cycle

    private new void Awake()
    {
        base.Awake();
        anim.SetTrigger("ToArise");
    }

    #endregion

    #region Implementation Place

    protected override IEnumerator Co_Pattern()
    {
        yield return new WaitForSeconds(waitStart);
    }

    #endregion
}
