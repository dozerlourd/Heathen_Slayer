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
    PlayerMove playerMove;
    Rigidbody2D rb;

    Coroutine Co_attack;

    public bool isAttack = false;
    bool isSkill_1 = false;
    public int attackCount = 0;

    float dashTime = 0.3f;
    float lerpSpeed = 4;

    void Start()
    {
        FindAttackCollider();
        atkCollider.enabled = false;

        anim = GetComponentInChildren<Animator>();
        playerMove = GetComponent<PlayerMove>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!playerMove.isDamaged)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                attackCount++;

                if (!isAttack)
                {
                    Co_attack = StartCoroutine(Attack1());
                }
            }

            if (!isAttack)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (!isSkill_1)
                    {
                        // 스킬 공격 1
                        StartCoroutine(SkillAttack1());
                    }

                    StartCoroutine(Skill1CoolDown());
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

    IEnumerator SkillAttack1()
    {
        float curTime = 0;
        anim.speed = 3.5f;
        playerMove.isMove = false;

        // dashCurTime 동안 대쉬를 실행한다.
        while (curTime <= dashTime)
        {
            curTime += Time.deltaTime;

            float lerpspeed = Mathf.Lerp(lerpSpeed, 0, curTime / dashTime);
            transform.position += transform.right * playerMove.moveSpeed * lerpspeed * Time.deltaTime;

            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;

            StartCoroutine(playerMove.SetGracePeriod(dashTime));

            yield return null;
        }

        rb.gravityScale = 3;
        anim.speed = 1;
        playerMove.isMove = true;
    }

    IEnumerator Skill1CoolDown()
    {
        isSkill_1 = true;

        yield return new WaitForSeconds(10);

        isSkill_1 = false;
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
        if (Co_attack != null)
        {
            StopCoroutine(Co_attack);
        }

        isAttack = false;
        atkCollider.enabled = false;
        attackCount = 0;
    }
}
