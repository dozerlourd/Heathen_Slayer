using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : HPControllerToEnemy
{
    #region Variable

    [SerializeField] Image hpBar;

    #endregion

    protected override void EnemyDamaged()
    {
        EnemyFSM.FlipCheck();
        Animator.SetTrigger("ToDamaged");
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
}
