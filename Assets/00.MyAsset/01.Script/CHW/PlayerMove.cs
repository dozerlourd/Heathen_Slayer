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

    // �̵� �Լ�
    void Move()
    {
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
        // Ground Layer�� �ε����� ���� ���� Ƚ�� �ʱ�ȭ
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            jumpCount = 2;
        }
    }

    // �뽬�� 2������ �ϰ�ʹ�.
    // �뽬�� ��Ÿ���� �ִ�
    // ������O ����X

    // �뽬�Ҷ��� �����̴�
    // �뽬�Ҷ��� �߷������� �ȵǰ� �̵����뵵 �ȵȴ�.

    IEnumerator DashCoolTime()
    {
        isDash = true;

        yield return new WaitForSeconds(dashCoolDown);
        dashCount = 2;
        isDash = false;
    }
}
