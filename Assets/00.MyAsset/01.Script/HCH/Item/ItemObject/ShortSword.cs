using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortSword : Item
{
    protected override void OnStart()
    {
        index = 0;

        capacity1_Name = GlobalState.passiveItemList[index].Capacity_Name_1;
        capacity1_Coef = GlobalState.passiveItemList[index].Capacity_Coef_1;
        capacity2_Name = GlobalState.passiveItemList[index].Capacity_Name_2;
        capacity2_Coef = GlobalState.passiveItemList[index].Capacity_Coef_2;
    }

    protected override void Execute()
    {
        
    }
}
