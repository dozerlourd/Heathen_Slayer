using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : HPControllerToEnemy
{
    protected override void EnemyDamaged()
    {
        EnemyFSM.FlipCheck();
        Animator.SetTrigger("ToDamaged");
    }

    protected override void RefreshUI(float _val)
    {
        
    }

    protected override IEnumerator EnemyDead()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }
}
