using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponColCheck : MonoBehaviour
{
    [Tooltip("���ݷ�")]
    public float attackDamage = 5f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        // ���� �浹 ����� �±װ� Enemy��
        if (coll.gameObject.CompareTag("Enemy"))
        { 
            // Enemy ü�� ����

        }
    }
}
