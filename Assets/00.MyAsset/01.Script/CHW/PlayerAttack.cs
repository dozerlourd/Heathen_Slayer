using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("공격속도")]
    public float attackSpeed = 0.5f;
    [Tooltip("공격 범위")]
    public BoxCollider2D atkCollider;
    [Tooltip("스킬 계수")]
    public float skillFactor = 1.5f;

    Animator anim;
    PlayerMove pm;

    Coroutine Co_attack;

    bool isAttack = false;
    public int attackCount = 0;

    void Start()
    {
        FindAttackCollider();
        atkCollider.enabled = false;

        anim = GetComponentInChildren<Animator>();
        pm = GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (!pm.isDamaged)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                attackCount++;

                if (!isAttack)
                {
                    Co_attack = StartCoroutine(Attack1());
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                // 스킬공격 1
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                // 스킬공격 2
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                // 스킬공격 3
            }
        }
    }

    IEnumerator Attack1()
    {
        attackCount = 0;
        isAttack = true;
        anim.SetTrigger("Attack1");

        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f);
        atkCollider.enabled = true;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        atkCollider.enabled = false;

        if (attackCount > 0)
        {
            Co_attack = StartCoroutine(Attack2());
        }
        else
        {
            isAttack = false;
        }
    }

    IEnumerator Attack2()
    {
        attackCount = 0;
        isAttack = true;
        anim.SetTrigger("Attack2");

        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f);
        atkCollider.enabled = true;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        atkCollider.enabled = false;

        if (attackCount > 0)
        {
            Co_attack = StartCoroutine(Attack3());
        }
        else
        {
            isAttack = false;
        }
    }

    IEnumerator Attack3()
    {
        attackCount = 0;
        isAttack = true;
        anim.SetTrigger("Attack3");

        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.55f);
        atkCollider.enabled = true;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f);
        atkCollider.enabled = false;

        isAttack = false;
    }

    public void FindAttackCollider()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = gameObject.transform.GetChild(i).gameObject;

            if (go.activeSelf)
            {
                atkCollider = go.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
            }
        }
    }

    public enum AnimationName
    { 
        Attack1,
        Attack2,
        Attack3
    }

    public float AnimationTime(AnimationName animationName)
    {
        string animName = string.Empty;

        switch (animationName)
        {
            case AnimationName.Attack1:
                animName = "Attack1";
                break;
            case AnimationName.Attack2:
                animName = "Attack2";
                break;
            case AnimationName.Attack3:
                animName = "Attack3";
                break;
            default:
                break;
        }

        float time = 0;

        RuntimeAnimatorController ac = anim.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == animName)
            {
                time = ac.animationClips[i].length;
            }
        }

        return time;
    }


    // 코루틴 종료와 초기화
    public void StopAttack()
    {
        StopCoroutine(Co_attack);
        isAttack = false;
        attackCount = 0;
    }
}
