using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyFSM : MonoBehaviour
{
    #region Variable

    protected bool isGround = false;

    [SerializeField] protected float basicAttackDmg = 10;
    [Header("Max 3 Skills")]
    [SerializeField] protected float[] skillAttackDmg = new float[3];
    [SerializeField] protected float atkSpeed = 1.0f, moveSpeed = 1.0f;
    [SerializeField] float detectRange = 10f, attackRange = 3;



    [SerializeField] protected float groundCheckRayDist = 5;
    protected BoxCollider2D boxCol2D;

    #endregion

    #region Property



    #endregion

    #region Unity Life Cycle

    protected void Awake()
    {
        boxCol2D = GetComponent<BoxCollider2D>();
    }

    protected void Update()
    {
        GroundCheck(groundCheckRayDist);
        if(!isGround)
        {
            print("중력 받아라");
            transform.Translate(0, -0.05f, 0);
        }
        print("와하");
    }

    #endregion

    #region Implementation Place 

    protected abstract IEnumerator Co_Pattern();

    protected void GroundCheck(float dist)
    {
        isGround = Physics2D.Raycast(transform.position, -transform.up, boxCol2D.size.y / 2 + dist, LayerMask.GetMask("Ground")) ? true : false;
    }

    #endregion
}
