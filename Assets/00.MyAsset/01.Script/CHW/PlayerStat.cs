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
    public int maxHP = 100;
    [Tooltip("���� ü��")]
    public int currentHP = 0;

    private void Awake()
    {
        currentHP = maxHP;
    }

    // HP ��������
    public void SetHP(int value)
    {
        currentHP += value;
    }

    // �ش� �ð���ŭ ���� ������Ʈ�� ���� ���·� ����
    public IEnumerator SetGracePeriod(float gracePeriod)
    {
        float currentTime = 0;

        Physics2D.IgnoreLayerCollision(6, 7, true);

        while (currentTime <= gracePeriod)
        {
            currentTime += Time.deltaTime;

            yield return null;
        }

        Physics2D.IgnoreLayerCollision(6, 7, false);
    }
}
