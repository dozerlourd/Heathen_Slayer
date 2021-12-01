using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, InteractionObject
{
    private void Awake()
    {
        InteractionManager.Instance.AddInterList(gameObject);
        GetComponent<Animator>().SetTrigger("ToOpen");
    }

    public void EntryNextDungeon()
    {
        StageSystem.Instance.CurrStage.CurrDungeon.IsNext = true;
    }

    public void Interaction()
    {
        EntryNextDungeon();
    }
}
