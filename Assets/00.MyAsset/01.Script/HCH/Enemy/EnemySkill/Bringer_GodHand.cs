using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringer_GodHand : MonoBehaviour
{
    #region Variable

    [SerializeField, Range(0f, 1f)] float attackTiming;
    [SerializeField, Range(0f, 0.5f)] float attackDuration;

    float damage;

    Animator animator;
    BoxCollider2D boxCol2D;
    Coroutine Co_SkillShot;

    #endregion

    #region Property

    Animator Animator => animator = animator ? animator : GetComponent<Animator>();

    BoxCollider2D BoxCol2D => boxCol2D = boxCol2D ? boxCol2D : GetComponent<BoxCollider2D>();

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
        BoxCol2D.enabled = false;
        yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= attackTiming);
        BoxCol2D.enabled = true;    
        yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= attackTiming + attackDuration);
        BoxCol2D.enabled = false;
        yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);
        gameObject.SetActive(false);
        StopCoroutine(Co_SkillShot);
    }

    public void SetDamage(float _dmg) => damage = _dmg;

    #endregion

    #region Callback Method

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerStat>().SetHP(6, 0.1f);
            BoxCol2D.enabled = false;
        }
    }

    #endregion
}
