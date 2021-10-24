using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("���ݼӵ�")]
    public float attackSpeed = 0.5f;
    [Tooltip("���� ����")]
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
            print("�÷��̾� ����");
        }
    }

    IEnumerator Attack()
    {
        atkCollider.enabled = true;

        yield return new WaitForSeconds(0.5f);
        atkCollider.enabled = false;
    }
}
