using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponColCheck : MonoBehaviour
{
    [Tooltip("���ݷ�")]
    public float attackDamage = 5f;
    [Tooltip("��ų ���")]
    public float skillFactor = 1.5f;

    [SerializeField] AudioClip attackSound;

    PlayerAttack playerAttack;

    // playerAttack = playerAttack ? playerAttack : GetComponent<PlayerAttack>();
    // playerAttack�� ������ playerAttack�� �������� ������ GetComponent�� �ؼ� �������� ���׿�����
    PlayerAttack PlayerAttack => playerAttack = playerAttack ? playerAttack : transform.parent.parent.GetComponent<PlayerAttack>();

    private void OnTriggerEnter2D(Collider2D col)
    {
        // ���� �浹 ����� �±װ� Enemy��
        if (col.gameObject.CompareTag("Enemy"))
        {
            CameraManager.Instance.ShakeCamera(0.07f, 0.06f);
            StartCoroutine(TimeStopu(0.05f));
            SoundManager.Instance.PlayEffectOneShot(attackSound, 0.875f);

            // Enemy ü�� ����
            col.GetComponent<HPControllerToEnemy>()?.TakeDamage(attackDamage);

            PlayerAttack.atkCollider.enabled = false;
        }
    }

    IEnumerator TimeStopu(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
    }
}
