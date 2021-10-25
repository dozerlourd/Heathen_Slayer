using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("공격속도")]
    public float attackSpeed = 0.5f;
    [Tooltip("공격 범위")]
    public BoxCollider2D atkCollider;
    [Tooltip("스킬 계수")]
    public float skillFactor = 1.5f;

    void Start()
    {
        atkCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(Attack());
            print("플레이어 공격");
        }

        if (Input.GetKeyDown(KeyCode.A))
        { 
            // 스킬공격 1
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // 스킬공격 2
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            // 스킬공격 3
        }
    }

    IEnumerator Attack()
    {
        atkCollider.enabled = true;

        yield return new WaitForSeconds(attackSpeed);
        atkCollider.enabled = false;
    }
}
