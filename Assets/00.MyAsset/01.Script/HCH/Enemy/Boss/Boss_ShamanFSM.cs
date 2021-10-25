using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_ShamanFSM : EnemyFSM
{
    #region Variable

    [Header(" - Elapsed Time For State Change"), Min(0.0f)]
    [SerializeField] float waitToPatrolTime;
    [SerializeField] float waitToTraceTime;
    [SerializeField] float waitToAttackTime;
    [SerializeField] float waitToSkillTime;

    [Header(" - Related to Shaman's Skill")]
    [SerializeField] GameObject Skill_PoisonDart;
    [SerializeField] GameObject Skill_PoisonFloor;
    [SerializeField] GameObject Skill_PoisonExplosion;

    [Tooltip("0 => Dart \n 1 => Floor \n 2 => Explosion")]
    [SerializeField] int[] maxSkillEffectPoolCounts;
    [SerializeField, Range(0f, 1f)] float skillEffectTiming;

    GameObject[] skillEffects_PoisonDart;
    GameObject[] skillEffects_PoisonFloor;
    GameObject[] skillEffects_PoisonExplosion;

    #endregion

    #region Property



    #endregion

    #region Unity Life Cycle

    private new void Start()
    {
        base.Start();

        skillEffects_PoisonDart = new GameObject[maxSkillEffectPoolCounts[0]];
        skillEffects_PoisonFloor = new GameObject[maxSkillEffectPoolCounts[1]];
        skillEffects_PoisonExplosion = new GameObject[maxSkillEffectPoolCounts[2]];
        for (int i = 0; i < maxSkillEffectPoolCounts[0]; i++)
        {
            skillEffects_PoisonDart[i] = Instantiate(Skill_PoisonDart);
            skillEffects_PoisonDart[i].name = Skill_PoisonDart.name;
            //skillEffects_PoisonDart[i].GetComponent<Shaman_SkillPool>().SetDamage(skillAttackDmg[0]);
            skillEffects_PoisonDart[i].transform.SetParent(FolderSystem.Instance.Shaman_SkillPool);
            skillEffects_PoisonDart[i].SetActive(false);
        }
        for (int i = 0; i < maxSkillEffectPoolCounts[1]; i++)
        {
            skillEffects_PoisonFloor[i] = Instantiate(Skill_PoisonFloor);
            skillEffects_PoisonFloor[i].name = Skill_PoisonFloor.name;
            //skillEffects_PoisonFloor[i].GetComponent<Shaman_SkillPool>().SetDamage(skillAttackDmg[0]);
            skillEffects_PoisonFloor[i].transform.SetParent(FolderSystem.Instance.Shaman_SkillPool);
            skillEffects_PoisonFloor[i].SetActive(false);
        }
        for (int i = 0; i < maxSkillEffectPoolCounts[2]; i++)
        {
            skillEffects_PoisonExplosion[i] = Instantiate(Skill_PoisonExplosion);
            skillEffects_PoisonExplosion[i].name = Skill_PoisonExplosion.name;
            //skillEffects_PoisonExplosion[i].GetComponent<Shaman_SkillPool>().SetDamage(skillAttackDmg[0]);
            skillEffects_PoisonExplosion[i].transform.SetParent(FolderSystem.Instance.Shaman_SkillPool);
            skillEffects_PoisonExplosion[i].SetActive(false);
        }
    }

    #endregion

    #region Implementation Place 

    protected override IEnumerator Co_Pattern()
    {
        yield return null;
    }

    #endregion
}
