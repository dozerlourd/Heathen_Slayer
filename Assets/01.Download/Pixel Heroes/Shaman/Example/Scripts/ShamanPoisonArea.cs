using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanPoisonArea : MonoBehaviour
{
    [SerializeField] float tickDamage, tickTime;
    WaitForSeconds waitForTick;
    bool isPlayerDamaged = false;
    Collider2D col2D;

    public List<Animator> PoisonAnimators;
    public bool IsActive = false;
    public bool Busy = false;

    private void Start()
    {
        waitForTick = new WaitForSeconds(tickTime);
        col2D = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        if (!col2D) return;
        StartCoroutine(PoisonArea());
    }

    private void Update()
    {
        Busy = PoisonAnimators[0].GetBool("Activating") || PoisonAnimators[0].GetBool("Deactivating");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.TryGetComponent(out PlayerStat playerStat);
            if (playerStat != null)
            {
                playerStat.SetHP(tickDamage, 0);
                isPlayerDamaged = true;
                col2D.enabled = false;
            }
        }
    }

    IEnumerator PoisonArea()
    {
        while(true)
        {
            col2D.enabled = true;
            yield return new WaitUntil(() => isPlayerDamaged);
            isPlayerDamaged = false;
            yield return waitForTick;
        }
    }
}