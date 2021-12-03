using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] protected float maxHP = 100;
    [Tooltip("현재 체력")]
    protected float currentHP = 0;

    public bool isPoison = false;

    [SerializeField] AudioClip[] DamagedSounds;

    private void Awake()
    {
        CurrentHP = MaxHP;
    }

    #region HCH

    protected Image PlayerHPBar;

    public float CurrentHP
    {
        get => currentHP;
        set
        {
            if(currentHP > value)
            {
                SceneEffectSystem.Instance.BloodFrame();
                CameraManager.Instance.ShakeCamera(0.075f, 0.05f);
            }
            currentHP = value;
            if(currentHP <= 0)
            {
                StartCoroutine(Dead());
            }
            RefreshUI(value);
        }
    }

    public float MaxHP => maxHP;

    // HP 변동사항
    public void SetHP(float value, float time)
    {
        if(CurrentHP != 0)
        {
            SoundManager.Instance.PlayVoiceOneShot(DamagedSounds, 0.85f);
        }

        CurrentHP -= value;

        StartCoroutine(SetGracePeriod(time));
    }

    protected void RefreshUI(float currHP)
    {
        if (PlayerHPBar == null) return;
        PlayerHPBar.fillAmount = currHP / MaxHP;
    }

    protected IEnumerator SearchPlayerAndUILink()
    {
        yield return new WaitUntil(() => PlayerSystem.Instance.Player != null);
        PlayerHPBar = GameObject.Find("PlayerUI_Curr HP Bar").GetComponent<Image>();
    }

#endregion

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

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Dead Scene");
    }
}
