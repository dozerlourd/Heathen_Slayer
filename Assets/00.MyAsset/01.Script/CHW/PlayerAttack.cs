using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("공격 범위")]
    public BoxCollider2D atkCollider;

    [SerializeField] AudioClip[] attackSounds;

    Animator anim;
    PlayerMove playerMove;
    Rigidbody2D rb;

    Coroutine Co_attack;

    public bool isAttack = false;
    bool isSkill_1 = false;
    public int attackCount = 0;

    public GameObject skillEffect;
    public GameObject shuriken;

    float dashTime = 0.3f;
    float lerpSpeed = 4;

    void Start()
    {
        FindAttackCollider();
        atkCollider.enabled = false;

        anim = GetComponent<Animator>();
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
                    // GhostWarrior
                    if (transform.GetChild(0).gameObject.activeSelf)
                    {
                        if (!isSkill_1)
                        {
                            // 스킬 공격 1
                            StartCoroutine(GhostWarrior_SpecialAttack_1());
                        }

                    }
                    // Rogue
                    else if (transform.GetChild(1).gameObject.activeSelf)
                    {
                        if (!isSkill_1)
                        {
                            // 스킬 공격 1
                            Rogue_SkillAttack_1();
                        }
                    }

                    StartCoroutine(SpecialAttack_1_CoolDown());
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
        SoundManager.Instance.PlayVoiceOneShot(attackSounds[0]);
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
            anim.Rebind();
        }
    }

    IEnumerator Attack2()
    {
        attackCount = 0;
        isAttack = true;
        anim.SetTrigger("Attack2");

        yield return null;
        SoundManager.Instance.PlayVoiceOneShot(attackSounds[1]);
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
            anim.Rebind();
        }
    }

    IEnumerator Attack3()
    {
        attackCount = 0;
        isAttack = true;
        anim.SetTrigger("Attack3");

        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.55f);
        SoundManager.Instance.PlayVoiceOneShot(attackSounds[2]);
        atkCollider.enabled = true;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f);
        atkCollider.enabled = false;

        isAttack = false;
        anim.Rebind();
    }

    IEnumerator GhostWarrior_SpecialAttack_1()
    {
        float curTime = 0;
        anim.speed = 3.5f;
        playerMove.isMove = false;

        Instantiate(skillEffect, transform.position + (transform.right * 2) - transform.up, Quaternion.identity);

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

    IEnumerator SpecialAttack_1_CoolDown()
    {
        float time = 0;
        isSkill_1 = true;

        if (transform.GetChild(0).gameObject.activeSelf)
        {
            time = 10;
        }
        else if (transform.GetChild(1).gameObject.activeSelf)
        {
            time = 5;
        }

        yield return new WaitForSeconds(time);

        isSkill_1 = false;
    }

    void Rogue_SkillAttack_1()
    {
        Instantiate(shuriken, transform.position, Quaternion.identity);
    }

    public void FindAttackCollider()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;

            if (go.activeSelf)
            {
                atkCollider = go.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
            }
        }
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
