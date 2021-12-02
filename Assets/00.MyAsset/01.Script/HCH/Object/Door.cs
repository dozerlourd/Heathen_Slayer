using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] DungeonData thisDungeon;
    [SerializeField] GameObject portal;
    Transform portalPos;

    bool isPortalInstantiate = false;

    private void Start()
    {
        portalPos = transform.GetChild(0);
    }

    private void Update()
    {
        if(IsClearThisRoom() && IsCurrRoom() && !isPortalInstantiate)
        {
            isPortalInstantiate = true;
            //�� ������ �� ȿ�������� ����~
            GameObject _portal = Instantiate(portal, portalPos.position, Quaternion.identity);
            _portal.GetComponent<Portal>().SetDungeonData(thisDungeon);
        }
    }

    bool IsClearThisRoom() => StageSystem.Instance.CurrStage.CurrDungeon.IsClearThisRoom;
    bool IsCurrRoom() => StageSystem.Instance.CurrStage.CurrDungeon == thisDungeon;
}
