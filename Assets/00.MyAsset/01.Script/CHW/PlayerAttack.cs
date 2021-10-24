using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("공격속도")]
    public float attackSpeed = 0.5f;
    [Tooltip("공격 범위")]
    public BoxCollider2D atkCollider;

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
    }

    IEnumerator Attack()
    {
        atkCollider.enabled = true;

        yield return new WaitForSeconds(0.5f);
        atkCollider.enabled = false;
    }
}
