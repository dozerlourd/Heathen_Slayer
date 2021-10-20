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

    #endregion

    #region HideInInspector

    protected bool isGround = false;
    protected BoxCollider2D boxCol2D;
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;
    protected float pastPosX = 0;

    #endregion

    #endregion

    #region Property



    #endregion

    #region Unity Life Cycle

    protected void Awake()
    {
        boxCol2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Update()
    {
        FlipCheck();
        GroundCheck(groundCheckRayDist);
        if (!isGround)
        {
            transform.Translate(0, -(0.5f * gravityScale * Time.deltaTime), 0);
        }
    }

    #endregion

    #region Implementation Place 

    protected abstract IEnumerator Co_Pattern();

    protected void FlipCheck()
    {
        if(pastPosX < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if(pastPosX > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        pastPosX = transform.position.x;
    }

    protected void GroundCheck(float dist)
    {
        isGround = Physics2D.Raycast(transform.position, -transform.up, boxCol2D.size.y / 2 + dist, LayerMask.GetMask("Ground")) ? true : false;
    }

    protected float GetDistanceB2WPlayer() => Vector2.Distance(PlayerSystem.Instance.Player.transform.position, transform.position);

    #endregion
}
