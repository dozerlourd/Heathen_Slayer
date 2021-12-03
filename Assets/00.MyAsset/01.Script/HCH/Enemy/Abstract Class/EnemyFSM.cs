using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyFSM : MonoBehaviour
{
    #region Variable

    #region SerializedField

    [Header(" - Attack")]
    #region Damage
    [SerializeField] protected float basicAttackDmg = 10;
    //[Header(" - Skill Variable (Max 3 Skills)")]
    [SerializeField] protected float[] skillAttackDmg = new float[3];
    #endregion

    [Header(" - Speed")]
    #region Speed
    [SerializeField] protected float attackSpeed = 1.0f;
    [SerializeField] protected float moveSpeed = 1.0f;
    #endregion

    [Header(" - Check Distance")]
    #region Distance Variables
    [SerializeField] protected float playerDetectRange = 10f;

    [SerializeField] protected float wallDetectRange = 0.5f;

    [SerializeField] protected float attackRange = 3;
    [SerializeField] protected float groundCheckRayDist = 0.03f;
    [SerializeField] protected float detectingHeight = 5;
    #endregion

    [Header(" - Gravity")]
    #region Gravity
    [SerializeField] protected float gravityScale = 1.00f;
    #endregion

    [Header(" - Flip")]
    #region Flip
    [SerializeField] protected GameObject[] flipObjects;

    [SerializeField] protected float waitStart = 0.7f;

    [Tooltip("니가 처음에 왼쪽을 보고있냐 아님 오른쪽을 보고있냐")]
    [SerializeField] protected bool originFlipIsRight;
    #endregion

    #endregion

    #region HideInInspector

    //[Header(" - Animation")]
    #region
    private float initAnimSpeed = 1;
    #endregion

    protected bool isGround = false;
    private bool isFrozen = false;
    protected BoxCollider2D boxCol2D;
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;

    protected Coroutine Co_Gravity;
    protected Coroutine Co_Freeze;
    protected Coroutine Co_FreezeColor;

    #endregion

    #endregion

    #region Property

    protected Vector2 playerPos => PlayerSystem.Instance.Player.transform.position;

    protected float flipValue => originFlipIsRight ? spriteRenderer.flipX ? -1 : 1 : spriteRenderer.flipX ? 1 : -1;

    public bool IsFrozen
    {
        get => isFrozen;
        protected set => isFrozen = value;
    }

    protected float InitAnimSpeed => initAnimSpeed;

    protected bool IsMove { get; set; }

    protected bool IsNotWall { get; set; }

    #endregion

    #region Unity Life Cycle

    protected void Awake()
    {
        boxCol2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void OnEnable()
    {
        Co_Gravity = StartCoroutine(Grivaty());
        StartCoroutine(Co_Pattern());
    }

    protected void Start()
    {
        initAnimSpeed = anim.GetFloat("AnimSpeed");
    }

    protected void Update()
    {
        StopToWall();
    }

    #endregion

    #region Implementation Place 

    protected abstract IEnumerator Co_Pattern();

    IEnumerator Grivaty()
    {
        yield return new WaitForSeconds(waitStart);
        while (true)
        {
            GroundCheck(groundCheckRayDist);
            if (!isGround)
            {
                transform.Translate(0, -(4.9f * gravityScale * Time.deltaTime), 0);
            }
            yield return null;
        }
    }

    void StopToWall()
    {
        Vector2 rayDir = originFlipIsRight ? (spriteRenderer.flipX ? Vector2.left : Vector2.right) : (spriteRenderer.flipX ? Vector2.right : Vector2.left);

        IsNotWall = !Physics2D.Raycast((Vector2)transform.position + boxCol2D.offset, rayDir, wallDetectRange, LayerMask.GetMask("L_Ground"));

        Debug.DrawRay((Vector2)transform.position + boxCol2D.offset, rayDir * wallDetectRange, Color.red);
    }

    public void FlipCheck()
    {
        if (!spriteRenderer || Mathf.Abs(playerPos.x - transform.position.x) < 0.5f) return;

        if (originFlipIsRight)
        {
            spriteRenderer.flipX = playerPos.x < transform.position.x ? true : false;
        }
        else
        {
            spriteRenderer.flipX = playerPos.x > transform.position.x ? true : false;
        }

        if (flipObjects.Length == 0) return;
        for (int i = 0; i < flipObjects.Length; i++)
        {
            flipObjects[i].transform.eulerAngles = new Vector3(0, spriteRenderer.flipX ? 180 : 0, 0);
        }
    }

    protected void GroundCheck(float dist)
    {
        isGround = Physics2D.Raycast(transform.position + Vector3.up * boxCol2D.offset.y, -transform.up, boxCol2D.size.y / 2 + dist, LayerMask.GetMask("L_Ground")) ? true : false;
    }

    protected float GetDistanceB2WPlayer()
    {
        if (PlayerSystem.Instance.Player == null) return 0;
        return Vector2.Distance(PlayerSystem.Instance.Player.transform.position, transform.position);
    }

    protected float GetDistanceB2WPlayerXValue() => Mathf.Abs(PlayerSystem.Instance.Player.transform.position.x - transform.position.x);
    protected float GetDistanceB2WPlayerYValue() => Mathf.Abs(PlayerSystem.Instance.Player.transform.position.y - transform.position.y);

    #region Unusual Condition Method

    #region Stun

    public IEnumerator Stun(float stunTime)
    {
        StopAllCoroutines();
        Co_Gravity = StartCoroutine(Grivaty());
        anim.SetBool("IsStunned", true);
        yield return new WaitForSeconds(stunTime);
        anim.SetBool("IsStunned", false);
        StartCoroutine(Co_Pattern());
    }

    #endregion

    #region Freeze

    /// <summary>
    /// The method for freezing enemy
    /// </summary>
    /// <param name="freezeDuration"> Freeze State Duration </param>
    /// <param name="intensity"> Speed Deceleration value (As the value increases, more slower) [Range: 0 ~ 1] </param>
    public void SetFreeze(float freezeDuration, float intensity)
    {
        intensity = Mathf.Clamp01(intensity);
        if (Co_Freeze != null)
        {
            StopCoroutine(Co_Freeze);
            StopCoroutine(Co_FreezeColor);
        }

        Co_Freeze = StartCoroutine(Freeze(freezeDuration, intensity));
    }

    protected IEnumerator Freeze(float freezeDuration, float intensity)
    {
        anim.SetFloat("AnimSpeed", InitAnimSpeed - intensity);
        Co_FreezeColor = StartCoroutine(SetFrozenColor(freezeDuration));
        IsFrozen = true;
        yield return new WaitForSeconds(freezeDuration);
        anim.SetFloat("AnimSpeed", InitAnimSpeed);
        IsFrozen = false;
        StopCoroutine(Co_Freeze);
    }

    private IEnumerator SetFrozenColor(float duration)
    {
        spriteRenderer.color = new Color(0.4117647f, 0.7333333f, 0.9176471f); // blue

        yield return new WaitForSeconds(duration);

        spriteRenderer.color = Color.white;
    }

    #endregion

    #endregion

    protected IEnumerator Move()
    {
        IsMove = GetDistanceB2WPlayerXValue() >= attackRange;
        anim.SetBool("IsWalk", IsMove);
        yield return null;
        if(IsMove && IsNotWall)
        {
            transform.Translate(Vector2.right * flipValue * moveSpeed * Time.deltaTime);
        }
    }

    #endregion
}
