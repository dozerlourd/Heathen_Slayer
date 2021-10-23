using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP : HPControllerToEnemy
{
    // 배열로 특정 체력 비율을 뺴놓고 비교해서 피격 상태로 변경한다
    protected override void EnemyDamaged()
    {
        
    }

    protected override void RefreshUI(float _val)
    {
        
    }
}
