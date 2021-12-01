using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject portal;
    Transform portalPos;

    private void Start()
    {
        portalPos = transform.GetChild(0);
    }

    private void Update()
    {
        if(EnemyCountEqualZero() && StageSystem.Instance.CurrStage.CurrDungeon.IsJoin)
        {
            StageSystem.Instance.CurrStage.CurrDungeon.IsJoin = false;
            Instantiate(portal, portalPos.position, Quaternion.identity);
            //�� ������ �� ȿ�������� ����~
        }
    }

    bool EnemyCountEqualZero() => StageSystem.Instance.CurrStage.CurrDungeon.GetEnemyCount() != 0;
}
