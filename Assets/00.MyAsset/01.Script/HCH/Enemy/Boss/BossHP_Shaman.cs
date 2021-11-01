using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP_Shaman : HPControllerToEnemy
{
    // 배열로 특정 체력 비율을 뺴놓고 비교해서 피격 상태로 변경한다
    protected override void EnemyDamaged()
    {


        
    }

    protected override IEnumerator EnemyDead()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);

    }

    protected override void RefreshUI(float _val)
    {
        
    }
}
