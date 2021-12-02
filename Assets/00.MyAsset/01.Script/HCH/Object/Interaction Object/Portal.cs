using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, InteractionObject
{
    DungeonData thisDungeon;

    private void Awake()
    {
        InteractionManager.Instance.AddInterList(this, gameObject);
        GetComponent<Animator>().SetTrigger("ToOpen");
    }

    public void SetDungeonData(DungeonData value) => thisDungeon = value;

    public void EntryNextDungeon()
    {
        thisDungeon.IsNext = true;
    }

    public void Interaction()
    {
        EntryNextDungeon();
    }
}
