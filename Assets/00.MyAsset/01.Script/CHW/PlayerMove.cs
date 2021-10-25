using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerStat
{
    Rigidbody2D rb;
    bool isJumping = false;
    bool isDash = false;
    bool isDamaged = false;
    Vector3 moveDir;

    float dashTime = 0.3f;
    float curTime = 0;
    float lerpSpeed = 4;
    float dashCountTime = 1;
    float dashCountCurTime = 0;
    float damagedTime = 0.5f;

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

        if (Input.GetKeyDown(KeyCode.C) && dashCount > 0 && !isDamaged)
        {
            StartCoroutine(Dash());
            dashCount--;

            // �뽬 ī��Ʈ�� 0 �����϶�
            if (dashCount <= 0 && isDash == false)
            {
                // �뽬 ��Ÿ�� ����
                StartCoroutine(DashCoolTime());
            }
        }

        // �뽬 ī��Ʈ�� 1�϶�
        if (dashCount == 1)
        {
            dashCountCurTime += Time.deltaTime;

            // ���� �ð��� ������ �뽬 ī��Ʈ ���� 
            if (dashCountCurTime >= dashCountTime)
            {
                dashCount--;

                // �뽬 ��Ÿ�� ����
                StartCoroutine(DashCoolTime());
            }
        }

        if (currentHP == 0)
        { 
            // ��� �ִϸ��̼� ���
        }
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    // �̵� �Լ�
    void Move()
    {
        if (isDamaged)
            return;

        moveDir = Vector3.zero;

        // ���� ����Ű�� �θ���
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // �������� �����δ�.
            moveDir = Vector3.left;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        // ������ ����Ű�� ������
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // ���������� �����δ�.
            moveDir = Vector3.right;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // ���� ����Ű�� ������ ����Ű�� �� �� ������
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            // �����.
            moveDir = Vector3.zero;
        }

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    // ���� �Լ�
    void Jump()
    {
        if (isDamaged)
            return;

        if (!isJumping)
            return;

        rb.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, jumpPower);

        //���� ���� Ƚ���� 0 �̻��̸�
        if (jumpCount > 0)
        {
            rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
            jumpCount--;
        }

        isJumping = false;
    }

    // �뽬 �Լ�
    IEnumerator Dash()
    {
        // �뽬 ī��Ʈ�� 0 �̻��̸�
        if (dashCount >= 0)
        {
            // dashCurTime ���� �뽬�� �����Ѵ�.
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
            rb.gravityScale = 1;
        }
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        // Ground Layer�� �ε����� ���� ���� Ƚ�� �ʱ�ȭ
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            jumpCount = 2;
        }


    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // SpecialAttack �±׸� ���� ������Ʈ���� �ε�����
        if (col.gameObject.CompareTag("SpecialAttack"))
        {
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

    // �뽬 ��Ÿ�� ����
    IEnumerator DashCoolTime()
    {
        isDash = true;

        yield return new WaitForSeconds(dashCoolDown);
        // �ʱ�ȭ
        dashCount = 2;
        dashCountCurTime = 0;
        isDash = false;
    }

    // �ǰ� �� �˹� �� ����
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