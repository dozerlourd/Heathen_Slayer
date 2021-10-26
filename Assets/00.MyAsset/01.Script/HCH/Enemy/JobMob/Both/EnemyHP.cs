using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : HPControllerToEnemy
{
    EnemyFSM enemyFSM;
    Animator animator;
    EnemyFSM EnemyFSM => enemyFSM = enemyFSM ? enemyFSM : GetComponent<EnemyFSM>();
    Animator Animator => animator = animator ? animator : GetComponent<Animator>();



    protected override void EnemyDamaged()
    {
        EnemyFSM.FlipCheck();
        Animator.SetTrigger("ToDamaged");
    }

    protected override void RefreshUI(float _val)
    {
        
    }
}
