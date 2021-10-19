using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerStat
{
    Rigidbody2D rb;
    bool isJumping = false;
    bool isDash;
    Vector3 moveDir;

    float dashTime = 0.3f;
    float dashCurTime = 0;
    float lerpSpeed = 4;

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

            if (dashCount < dashMaxCount && isDash == false)
            {
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
            while (dashCurTime <= dashTime)
            {
                dashCurTime += Time.deltaTime;

                float lerpspeed = Mathf.Lerp(lerpSpeed, 0 , dashCurTime / dashTime);
                transform.position += transform.right * moveSpeed * lerpspeed * Time.deltaTime;
                
                yield return null;
            }

            dashCurTime = 0;
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

    IEnumerator DashCoolTime()
    {
        isDash = true;

        yield return new WaitForSeconds(dashCoolDown);
        dashCount = 2;
        isDash = false;
    }
}
