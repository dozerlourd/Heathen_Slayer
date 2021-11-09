using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponColCheck : MonoBehaviour
{
    [Tooltip("공격력")]
    public float attackDamage = 5f;
    [Tooltip("스킬 계수")]
    public float skillFactor = 1.5f;

    PlayerAttack playerAttack;

    // playerAttack = playerAttack ? playerAttack : GetComponent<PlayerAttack>();
    // playerAttack이 있으면 playerAttack을 가져오고 없으면 GetComponent를 해서 가져오는 삼항연산자
    PlayerAttack PlayerAttack => playerAttack = playerAttack ? playerAttack : transform.parent.GetComponent<PlayerAttack>();

    private void OnTriggerEnter2D(Collider2D col)
    {
        // 만약 충돌 대상의 태그가 Enemy면
        if (col.gameObject.CompareTag("Enemy"))
        {
            // Enemy 체력 감소
            col.GetComponent<HPControllerToEnemy>()?.TakeDamage(attackDamage);

            PlayerAttack.atkCollider.enabled = false;
        }
    }
}
