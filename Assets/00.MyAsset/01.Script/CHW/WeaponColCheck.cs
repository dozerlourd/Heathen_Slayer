using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponColCheck : MonoBehaviour
{
    [Tooltip("���ݷ�")]
    public float attackDamage = 5f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        // ���� �浹 ����� �±װ� Enemy��
        if (col.gameObject.CompareTag("Enemy"))
        {
            // Enemy ü�� ����
            col.GetComponent<HPControllerToEnemy>()?.TakeDamage(attackDamage);
        }
    }
}
