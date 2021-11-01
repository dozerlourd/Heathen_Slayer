using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerStat
{
    Rigidbody2D rb;
    Animator anim;
    bool isJumping = false;
    bool isDash = false;
    public bool isDamaged = false;
    Vector3 moveDir;

    float dashTime = 0.3f;
    float curTime = 0;
    [Tooltip("대쉬 속도 조절")]
    [SerializeField]float lerpSpeed = 4;
    [Tooltip("대쉬 카운트 감소 시간")]
    [SerializeField] float dashCountTime = 1;
    float dashCountCurTime = 0;
    float damagedTime = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            return;

        Move();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isJumping = true;   
        }

        if (Input.GetKeyDown(KeyCode.C) && dashCount > 0 && !isDamaged)
        {
            StartCoroutine(Dash());
            dashCount--;

            // 대쉬 카운트가 0 이하일때
            if (dashCount <= 0 && isDash == false)
            {
                // 대쉬 쿨타임 적용
                StartCoroutine(DashCoolTime());
            }
        }

        // 대쉬 카운트가 1일때
        if (dashCount == 1)
        {
            dashCountCurTime += Time.deltaTime;

            // 일정 시간이 지나면 대쉬 카운트 감소 
            if (dashCountCurTime >= dashCountTime)
            {
                dashCount--;

                // 대쉬 쿨타임 적용
                StartCoroutine(DashCoolTime());
            }
        }

        if (currentHP <= 0)
        {
            // 사망 애니메이션 출력
            anim.SetTrigger("Die");
        }
    }

    private void FixedUpdate()
    {
        Jump();
    }

    // 이동 함수
    void Move()
    {
        if (isDamaged)
            return;

        moveDir = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            // 대기 애니메이션 실행
            anim.SetBool("Move", false);

            // 멈춘다.
            moveDir = Vector3.zero;
            
        }
        // 왼쪽 방향키를 부르면
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            // 왼쪽으로 움직인다.
            moveDir = Vector3.left;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        // 오른쪽 방향키를 누르면
        else if (Input.GetKey(KeyCode.RightArrow))
        {

            // 오른쪽으로 움직인다.
            moveDir = Vector3.right;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            // 이동 애니메이션 실행
            anim.SetBool("Move", true);
        }

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    // 점프 함수
    void Jump()
    {
        if (isDamaged)
            return;

        if (!isJumping)
            return;

        rb.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, jumpPower);

        //점프 가능 횟수가 0 이상이면
        if (jumpCount > 0)
        {
            rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
            jumpCount--;
        }

        isJumping = false;
    }

    // 대쉬 함수
    IEnumerator Dash()
    {
        // 대쉬 카운트가 0 이상이면
        if (dashCount >= 0)
        {
            anim.speed = 3.5f;
            // dashCurTime 동안 대쉬를 실행한다.
            while (curTime <= dashTime)
            {
                curTime += Time.deltaTime;

                float lerpspeed = Mathf.Lerp(lerpSpeed, 0 , curTime / dashTime);
                transform.position += transform.right * moveSpeed * lerpspeed * Time.deltaTime;

                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;

                StartCoroutine(SetGracePeriod(dashTime));

                yield return null;
            }

            curTime = 0;
            rb.gravityScale = 3;
            anim.speed = 1;
        }
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        // Ground Layer에 부딪히면 점프 가능 횟수 초기화
        if (col.gameObject.layer == LayerMask.NameToLayer("L_Ground"))
        {
            jumpCount = 2;
        }


    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // SpecialAttack 태그를 가진 오브젝트에게 부딪히면
        if (col.gameObject.CompareTag("SpecialAttack_Enemy"))
        {
            anim.SetTrigger("GetHit");

            Vector2 damagedVelocity = Vector2.zero;

            if (col.gameObject.transform.position.x > transform.position.x)
            {
                damagedVelocity = new Vector2(-2f, 2f);
            }
            else
            { 
                damagedVelocity = new Vector2(2f, 2f);
            }

            StartCoroutine(DamagedMove(damagedTime, damagedVelocity));
            StartCoroutine(SetGracePeriod(damagedTime));
        }
    }

    // 대쉬 쿨타임 적용
    IEnumerator DashCoolTime()
    {
        isDash = true;

        yield return new WaitForSeconds(dashCoolDown);
        // 초기화
        dashCount = 2;
        dashCountCurTime = 0;
        isDash = false;
    }

    // 피격 시 넉백 및 경직
    IEnumerator DamagedMove(float damagedTime, Vector2 damagedDir)
    {
        float currentTime = 0;
        isDamaged = true;

        while (currentTime <= damagedTime)
        {
            currentTime += Time.deltaTime;

            transform.position += new Vector3(damagedDir.x, damagedDir.y, 0) * Time.deltaTime;

            yield return null;
        }

        isDamaged = false;
    }
}
