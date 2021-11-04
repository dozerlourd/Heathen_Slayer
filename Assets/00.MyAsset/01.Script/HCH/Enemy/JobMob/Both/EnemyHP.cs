using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : HPControllerToEnemy
{
    #region Variable

    [SerializeField] Image hpBar;

    [SerializeField] float hardnessDuration;

    WaitForSeconds hardnessTime;

    #endregion

    #region Unity Life Cycle

    private void Start()
    {
        hardnessTime = new WaitForSeconds(hardnessDuration);
    }

    #endregion

    #region Implementation Place

    protected override IEnumerator EnemyDamaged()
    {
        EnemyFSM.FlipCheck();
        Animator.SetTrigger("ToDamaged");

        yield return hardnessTime;
    }

    protected override void RefreshUI(float _val)
    {
        if (!hpBar) return;
        hpBar.fillAmount = currHP / maxHP;
    }

    protected override IEnumerator EnemyDead()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }

    #endregion
}
