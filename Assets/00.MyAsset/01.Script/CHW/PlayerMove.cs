using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerStat
{
    Rigidbody2D rb;
    Animator anim;
    PlayerAttack playerAttack;
    bool isJumping = false;
    bool isDash = false;
    public bool isDamaged = false;
    public bool isMove = true;
    Vector3 moveDir;

    float dashTime = 0.3f;
    [Tooltip("�뽬 �ӵ� ����")]
    [SerializeField] float lerpSpeed = 4;
    [Tooltip("�뽬 ī��Ʈ ���� �ð�")]
    [SerializeField] float dashCountTime = 1;
    float dashCountCurTime = 0;
    float damagedTime = 0.5f;

    Coroutine Co_StatusEffect;

    public GameObject posionEffect;

    [SerializeField] AudioClip[] dashSounds;
    [SerializeField] AudioClip[] jumpSounds;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();

        StartCoroutine(SearchPlayerAndUILink());
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            return;

        if (isMove)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.C) && dashCount > 0 && !isDamaged)
        {
            playerAttack.StopAttack();
            StartCoroutine(Dash());
            dashCount--;

            // �뽬 ī��Ʈ�� 0 �����϶�
            if (dashCount <= 0 && !isDash)
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

        // �� ���¶��
        if (isPoison)
        {
            posionEffect.SetActive(true);
        }
        else
        {
            posionEffect.SetActive(false);
        }

        if (CurrentHP <= 0)
        {
            // ��� �ִϸ��̼� ���
            anim.SetTrigger("Die");
        }
    }

    private void FixedUpdate()
    {
        Jump();
    }

    // �̵� �Լ�
    void Move()
    {
        if (isDamaged)
            return;

        moveDir = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            // ��� �ִϸ��̼� ����
            anim.SetBool("Move", false);

            // �����.
            moveDir = Vector3.zero;

        }
        // ���� ����Ű�� �θ���
        else if (Input.GetKey(KeyCode.LeftArrow))
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

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            // �̵� �ִϸ��̼� ����
            anim.SetBool("Move", true);
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

        SoundManager.Instance.PlayEffectOneShot(jumpSounds, 0.2f);

        Vector2 jumpVelocity = new Vector2(0, jumpPower);

        //���� ���� Ƚ���� 0 �̻��̸�
        if (jumpCount > 0)
        {
            rb.velocity = Vector2.zero;

            anim.SetTrigger("Jump");

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
            float curTime = 0;
            anim.speed = 3.5f;
            isMove = false;
            SoundManager.Instance.PlayEffectOneShot(dashSounds, 0.45f);

            // dashCurTime ���� �뽬�� �����Ѵ�.
            while (curTime <= dashTime)
            {
                curTime += Time.deltaTime;

                float lerpspeed = Mathf.Lerp(lerpSpeed, 0, curTime / dashTime);
                transform.position += transform.right * moveSpeed * lerpspeed * Time.deltaTime;

                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;

                StartCoroutine(SetGracePeriod(dashTime));

                yield return null;
            }

            rb.gravityScale = 3;
            anim.speed = 1;
            isMove = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Ground Layer�� �ε����� ���� ���� Ƚ�� �ʱ�ȭ
        if (col.gameObject.layer == LayerMask.NameToLayer("L_Ground"))
        {
            jumpCount = 2;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // SpecialAttack �±׸� ���� ������Ʈ���� �ε�����
        if (col.gameObject.CompareTag("SpecialAttack_Enemy"))
        {
            anim.SetTrigger("GetHit");

            Vector2 damagedVelocity;

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

            playerAttack.atkCollider.enabled = false;
            transform.position += new Vector3(damagedDir.x, damagedDir.y, 0) * Time.deltaTime;

            yield return null;
        }

        isDamaged = false;
    }

    // �����̻�
    IEnumerator StatusEffect(float statusEffectDamage, float statusEffectTime, float DOT)
    {
        float curTime = 0;
        float time = 0;

        // �� �ð� ���
        while (time <= statusEffectTime)
        {
            time += Time.deltaTime;
            curTime += Time.deltaTime;

            // ��Ʈ �ð� ���
            if (curTime >= DOT)
            {
                // ƽ �� ������
                SetHP(statusEffectDamage, 0);
                curTime = 0;
            }

            yield return null;
        }
    }

    public void EffectDamaged(float tickDamage, float amountDuration, float tickDuration)
    {
        if (Co_StatusEffect != null) StopCoroutine(Co_StatusEffect);
        Co_StatusEffect = StartCoroutine(StatusEffect(tickDamage, amountDuration, tickDuration));
    }
}
