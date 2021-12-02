using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] protected float maxHP = 100;
    [Tooltip("���� ü��")]
    protected float currentHP = 0;

    public bool isPoison = false;

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
            RefreshUI(value);
        }
    }

    public float MaxHP => maxHP;

    // HP ��������
    public void SetHP(float value, float time)
    {
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
