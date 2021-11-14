using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortSword : Item
{
    protected override void OnStart()
    {
        itemName = "숏소드";

        capacity1_Name = "공격력";

        capacity1_Coef = 0.1f;

        capacity2_Name = null;
        capacity2_Coef = 0;

        capacityInfo = "공격력이 10% 증가합니다.";
        skillInfo = "";

        itemRank = "일반";
    }

    protected override void Execute()
    {
        
    }
}
