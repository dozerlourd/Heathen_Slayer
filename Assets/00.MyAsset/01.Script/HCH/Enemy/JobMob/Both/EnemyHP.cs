using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : HPControllerToEnemy
{
    EnemyFSM EnemyFSM => EnemyFSM ? EnemyFSM : GetComponent<EnemyFSM>();
    Animator Animator => Animator ? Animator : GetComponent<Animator>();



    protected override void EnemyDamaged()
    {
        EnemyFSM.FlipCheck();
        Animator.SetTrigger("ToDamaged");
    }

    protected override void RefreshUI(float _val)
    {
        
    }
}
