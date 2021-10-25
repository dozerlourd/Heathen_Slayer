using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanPoisonAnimListener : MonoBehaviour
{
    private Animator _anim;
    
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void Activate()
    {
        _anim.SetBool("Active", true);
        _anim.SetBool("Activating", false);
    }
    
    public void Deactivate()
    {
        _anim.SetBool("Active", false);
        _anim.SetBool("Deactivating", false);
    }
}