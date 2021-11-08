using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : HPControllerToEnemy
{
    #region Variable

    [SerializeField] Image hpBar;

    [SerializeField] float hardnessDuration;

    [SerializeField] AudioClip[] deadVoiceClips;

    bool isAbsolute = false;

    WaitForSeconds hardnessTime;


    #endregion

    #region Unity Life Cycle

    private void Start()
    {
        hardnessTime = new WaitForSeconds(hardnessDuration);
    }

    new void OnEnable()
    {
        base.OnEnable();
        if(Co_Dead != null) StopCoroutine(Co_Dead);
    }

    #endregion

    #region Implementation Place

    protected override IEnumerator EnemyDamaged()
    {
        if (!isAbsolute)
        {
            EnemyFSM.FlipCheck();
            Animator.SetTrigger("ToDamaged");

            yield return hardnessTime;
        }
        else yield return null;
    }

    protected override void RefreshUI(float _val)
    {
        if (!hpBar) return;
        hpBar.fillAmount = currHP / maxHP;
    }

    protected override IEnumerator EnemyDead()
    {
        SoundManager.Instance.PlayVoiceOneShot(deadVoiceClips);
        isDead = true;
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }

    public void Absolute(bool value) => isAbsolute = value;

    #endregion
}
