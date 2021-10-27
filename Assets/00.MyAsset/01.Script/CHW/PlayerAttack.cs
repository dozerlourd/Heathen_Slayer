using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("���ݼӵ�")]
    public float attackSpeed = 0.5f;
    [Tooltip("���� ����")]
    public BoxCollider2D atkCollider;
    [Tooltip("��ų ���")]
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
            print("�÷��̾� ����");
        }

        if (Input.GetKeyDown(KeyCode.A))
        { 
            // ��ų���� 1
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // ��ų���� 2
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            // ��ų���� 3
        }
    }

    IEnumerator Attack()
    {
        atkCollider.enabled = true;

        yield return new WaitForSeconds(attackSpeed);
        atkCollider.enabled = false;
    }
}
