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
    [SerializeField] protected float detectRange = 10f;

    [SerializeField] protected float attackRange = 3;
    [SerializeField] protected float groundCheckRayDist = 0.03f;
    #endregion

    [Header(" - Gravity")]
    #region gravity
    [SerializeField] protected float gravityScale = 1.00f;
    #endregion

    [Header(" - Flip Objects")]
    #region flip objects
    [SerializeField] protected GameObject[] flipObjects;
    #endregion

    [Header(" - Flip Objects")]
    #region flip objects
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

    private void Start()
    {
        initAnimSpeed = anim.GetFloat("AnimSpeed");
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

    public void FlipCheck()
    {
        if (!spriteRenderer) return;

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

    protected float GetDistanceB2WPlayer() => Vector2.Distance(PlayerSystem.Instance.Player.transform.position, transform.position);
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

    #endregion
}
