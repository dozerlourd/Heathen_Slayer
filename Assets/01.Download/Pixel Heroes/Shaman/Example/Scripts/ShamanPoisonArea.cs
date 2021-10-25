using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanPoisonArea : MonoBehaviour
{
    public List<Animator> PoisonAnimators;
    public bool IsActive = false;
    public bool Busy = false;

    private void Update()
    {
        Busy = PoisonAnimators[0].GetBool("Activating") || PoisonAnimators[0].GetBool("Deactivating");
    }

    public void TogglePoisonArea()
    {
        if (Busy)
            return;

        if (IsActive)
        {
            IsActive = false;
            SetAnimBools("Deactivating", true);
        }
        else
        {
            IsActive = true;
            SetAnimBools("Activating", true);
        }
    }

    public void Activate()
    {
        SetAnimBools("Active", true);
        SetAnimBools("Activating", false);
    }

    public void Deactivate()
    {
        SetAnimBools("Active", false);
        SetAnimBools("Deactivating", false);
    }

    private void SetAnimBools(string boolName, bool value)
    {
        foreach (var anim in PoisonAnimators)
        {
            anim.SetBool(boolName, value);
        }
    }
}