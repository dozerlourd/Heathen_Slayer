using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP_Shaman : HPControllerToEnemy
{
    // �迭�� Ư�� ü�� ������ ������ ���ؼ� �ǰ� ���·� �����Ѵ�
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
