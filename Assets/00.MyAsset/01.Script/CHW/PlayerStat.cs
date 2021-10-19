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
    [Tooltip("���� �ð�")]
    public float gracePeriod = 0.5f;
    [Tooltip("�뽬 ���� Ƚ��")]
    public int dashCount = 2;
    [Tooltip("�ִ� �뽬 Ƚ��")]
    public int dashMaxCount = 2;
    [Tooltip("�뽬 ��Ÿ��")]
    public float dashCoolDown = 3f;
}
