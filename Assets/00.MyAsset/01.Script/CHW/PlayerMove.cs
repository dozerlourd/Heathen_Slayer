using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerStat
{
    Rigidbody2D rb;
    bool isJumping = false;
    bool isDash = false;
    Vector3 moveDir;

    float dashTime = 0.3f;
    float curTime = 0;
    float lerpSpeed = 4;
    float dashCountTime = 1;
    float dashCountCurTime = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isJumping = true;   
        }

        if (Input.GetKeyDown(KeyCode.C) && dashCount > 0)
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
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    // 이동 함수
    void Move()
    {
        moveDir = Vector3.zero;

        // 왼쪽 방향키를 부르면
        if (Input.GetKey(KeyCode.LeftArrow))
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

        // 왼쪽 방향키와 오른쪽 방향키를 둘 다 누르면
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            // 멈춘다.
            moveDir = Vector3.zero;
        }

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    // 점프 함수
    void Jump()
    {
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
            // dashCurTime 동안 대쉬를 실행한다.
            while (curTime <= dashTime)
            {
                curTime += Time.deltaTime;

                float lerpspeed = Mathf.Lerp(lerpSpeed, 0 , curTime / dashTime);
                transform.position += transform.right * moveSpeed * lerpspeed * Time.deltaTime;

                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;

                yield return null;
            }

            curTime = 0;
            rb.gravityScale = 1;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Ground Layer에 부딪히면 점프 가능 횟수 초기화
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            jumpCount = 2;
        }
    }

    // 대쉬를 2번까지 하고싶다.
    // 대쉬에 쿨타임이 있다
    // 땅땅쿨O 땅쿨X

    // 대쉬할때는 무적이다
    // 대쉬할때는 중력적용이 안되고 이동적용도 안된다.

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
}
