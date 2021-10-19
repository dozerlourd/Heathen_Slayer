using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Tooltip("이동속도")]
    public float moveSpeed = 5.0f;
    [Tooltip("점프력")]
    public float jumpPower = 5.0f;
    [Tooltip("점프 가능 횟수")]
    public int jumpCount = 2;
    [Tooltip("무적 시간")]
    public float gracePeriod = 0.5f;
    [Tooltip("대쉬 가능 횟수")]
    public int dashCount = 2;
    [Tooltip("최대 대쉬 횟수")]
    public int dashMaxCount = 2;
    [Tooltip("대쉬 쿨타임")]
    public float dashCoolDown = 3f;
}
