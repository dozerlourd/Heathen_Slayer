using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Tooltip("�̵��ӵ�")]
    public float moveSpeed = 5.0f;
    [Tooltip("������")]
    public float jumpPower = 5.0f;
    [Tooltip("���� ���� Ƚ��")]
    public int jumpCount = 2;
    [Tooltip("�뽬 ���� Ƚ��")]
    public int dashCount = 2;
    //[Tooltip("�ִ� �뽬 Ƚ��")]
    //public int dashMaxCount = 2;
    [Tooltip("�뽬 ��Ÿ��")]
    public float dashCoolDown = 3f;
    [Tooltip("�ִ� ü��")]
    public float maxHP = 100;
    [Tooltip("���� ü��")]
    public float currentHP = 0;

    public bool isPoison = false;

    private void Awake()
    {
        currentHP = maxHP;
    }

    // HP ��������
    public void SetHP(float value, float time)
    {
        currentHP -= value;

        StartCoroutine(SetGracePeriod(time));
    }

    // �ش� �ð���ŭ ���� ������Ʈ�� ���� ���·� ����
    public IEnumerator SetGracePeriod(float gracePeriod)
    {
        Physics2D.IgnoreLayerCollision(7, 10, true);

        yield return new WaitForSeconds(gracePeriod);
        Physics2D.IgnoreLayerCollision(7, 10, false);
    }

    public void PoisonStatus(bool isCheck)
    {
        isPoison = isCheck;
    }
}
