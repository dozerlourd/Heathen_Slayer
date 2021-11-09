using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaman_Explosion : MonoBehaviour
{
    #region Variable

    [SerializeField] float attackDamage, gracePeriod;

    Collider2D col2D;

    Animator animator;

    #endregion

    #region Property
    Animator Animator => animator = animator ? animator : GetComponent<Animator>();
    Collider2D Col2D => col2D = col2D ? col2D : GetComponent<Collider2D>();

    #endregion

    private void OnEnable()
    {
        StartCoroutine(Explosion());
    }

    private IEnumerator Explosion()
    {
        yield return new WaitUntil(() => GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f);
        Col2D.enabled = true;
        yield return new WaitUntil(() => GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f);
        Col2D.enabled = false;
        yield return new WaitUntil(() => GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerStat>()?.SetHP(attackDamage, gracePeriod);
        }
    }
}
