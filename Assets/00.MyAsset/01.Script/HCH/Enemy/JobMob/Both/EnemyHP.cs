using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : HPControllerToEnemy
{
    #region Variable

    [SerializeField] Image hpBar;

    [SerializeField] float hardnessDuration;

    [SerializeField] AudioClip[] damagedVoiceClips;

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
        isDead = false;
        gameObject.tag = "Enemy";
        hpBar.transform.parent.parent.gameObject.SetActive(true);
        if (Co_Dead != null) StopCoroutine(Co_Dead);
    }

    #endregion

    #region Implementation Place

    protected override IEnumerator EnemyDamaged()
    {
        if (!isAbsolute)
        {
            EnemyFSM.FlipCheck();
            Animator.SetTrigger("ToDamaged");
            //SoundManager.Instance.PlayVoiceOneShot(damagedVoiceClips);

            yield return hardnessTime;
        }
        else yield return null;
    }

    protected override void RefreshUI()
    {
        if (!hpBar) return;
        hpBar.fillAmount = currHP / maxHP;
    }

    protected override IEnumerator EnemyDead()
    {
        SoundManager.Instance.PlayVoiceOneShot(deadVoiceClips);
        hpBar.transform.parent.parent.gameObject.SetActive(false);
        GetComponent<EnemyItemDrop>().Looting();
        gameObject.tag = "Corpse";
        isDead = true;
        StartCoroutine(base.EnemyDead());

        yield return new WaitForSeconds(corpseTime);
        gameObject.SetActive(false);
    }

    public void SetAbsolute(bool value) => isAbsolute = value;

    #endregion
}
