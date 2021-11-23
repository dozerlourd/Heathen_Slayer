using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : MonoBehaviour
{
    [SerializeField] Text Text_itemname;
    [SerializeField] Text Text_capacity;
    [SerializeField] Text Text_ItemInfo;
    [SerializeField] Text Text_ItemRank;

    public void SetInfoText(string itemName, string capacityInfo, string itemInfo, string itemRank)
    {
        if (itemName != null) Text_itemname.text = itemName;
        if (capacityInfo != null) Text_capacity.text = capacityInfo;
        if (itemInfo != null) Text_ItemInfo.text = itemInfo;
        if (itemRank != null) Text_ItemRank.text = itemRank;
    }

    public void SetInfoText(int idx)
    {
        SetInfoText(GlobalState.passiveItemList[idx].ItemName, GlobalState.passiveItemList[idx].CapacityInfo, GlobalState.passiveItemList[idx].SkillInfo, GlobalState.passiveItemList[idx].Rank);
    }
}
