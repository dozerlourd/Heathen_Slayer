using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponColCheck : MonoBehaviour
{
    [Tooltip("공격력")]
    public float attackDamage = 5f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        // 만약 충돌 대상의 태그가 Enemy면
        if (col.gameObject.CompareTag("Enemy"))
        {
            // Enemy 체력 감소
            col.GetComponent<HPControllerToEnemy>()?.TakeDamage(attackDamage);
        }
    }
}
