using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortSword : Item
{
    protected override void OnStart()
    {
        itemName = "���ҵ�";

        capacity1_Name = "���ݷ�";

        capacity1_Coef = 0.1f;

        capacity2_Name = null;
        capacity2_Coef = 0;

        capacityInfo = "���ݷ��� 10% �����մϴ�.";
        skillInfo = "";

        itemRank = "�Ϲ�";
    }

    protected override void Execute()
    {
        
    }
}
