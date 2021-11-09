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
    [Tooltip("대쉬 가능 횟수")]
    public int dashCount = 2;
    //[Tooltip("최대 대쉬 횟수")]
    //public int dashMaxCount = 2;
    [Tooltip("대쉬 쿨타임")]
    public float dashCoolDown = 3f;
    [Tooltip("최대 체력")]
    public float maxHP = 100;
    [Tooltip("현재 체력")]
    public float currentHP = 0;

    public bool isPoison = false;

    private void Awake()
    {
        currentHP = maxHP;
    }

    // HP 변동사항
    public void SetHP(float value, float time)
    {
        currentHP -= value;

        StartCoroutine(SetGracePeriod(time));
    }

    // 해당 시간만큼 게임 오브젝트를 무적 상태로 설정
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
