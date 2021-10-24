using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponColCheck : MonoBehaviour
{
    [Tooltip("공격력")]
    public float attackDamage = 5f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        // 만약 충돌 대상의 태그가 Enemy면
        if (coll.gameObject.CompareTag("Enemy"))
        { 
            // Enemy 체력 감소

        }
    }
}
