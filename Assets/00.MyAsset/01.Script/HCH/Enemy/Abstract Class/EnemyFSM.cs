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
    [SerializeField] protected float groundCheckRayDist = 5;
    #endregion

    [Header(" - Gravity")]
    #region gravity
    [SerializeField] protected float gravityScale = 9.81f;
    #endregion

    [Header(" - Flip Objects")]
    #region flip objects
    [SerializeField] GameObject[] flipObjects;
    #endregion

    #endregion

    #region HideInInspector

    protected bool isGround = false;
    protected BoxCollider2D boxCol2D;
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;

    Coroutine Co_Gravity;

    #endregion

    #endregion

    #region Property

    [Tooltip("니가 처음에 왼쪽을 보고있냐 아님 오른쪽을 보고있냐")]
    [SerializeField] protected bool originFlipIsRight;
    protected Vector2 playerPos => PlayerSystem.Instance.Player.transform.position;

    protected float flipValue => originFlipIsRight ? spriteRenderer.flipX ? -1 : 1 : spriteRenderer.flipX ? 1 : -1;

    #endregion

    #region Unity Life Cycle

    protected void Awake()
    {
        boxCol2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
        Co_Gravity = StartCoroutine(Grivaty());
    }

    #endregion

    #region Implementation Place 

    protected abstract IEnumerator Co_Pattern();

    IEnumerator Grivaty()
    {
        yield return new WaitForSeconds(0.25f);
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

    #endregion
}
