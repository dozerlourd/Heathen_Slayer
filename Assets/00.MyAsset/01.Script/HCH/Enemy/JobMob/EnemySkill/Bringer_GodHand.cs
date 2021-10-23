using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringer_GodHand : MonoBehaviour
{
    #region Variable

    [SerializeField, Range(0f, 1f)] float attackTiming;

    Animator animator;
    Animator Animator => animator = animator ? animator : GetComponent<Animator>();

    Coroutine Co_SkillShot;

    #endregion 

    #region Unity Life Cycle

    private void OnEnable()
    {
        Co_SkillShot = StartCoroutine(SkillShot());
    }

    #endregion 

    #region Implementation Place

    IEnumerator SkillShot()
    {
        yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= attackTiming);
        print("공격!!!!");
        // 중간에 공격
        yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);
        gameObject.SetActive(false);
        StopCoroutine(Co_SkillShot);
    }

    #endregion 
}
