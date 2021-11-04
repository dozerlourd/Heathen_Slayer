using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaman_PoisonArea : MonoBehaviour
{
    [SerializeField] float tickDamage, tickTime;
    WaitForSeconds waitForTick;
    bool isPlayerDamaged = false;
    Collider2D col2D;

    Coroutine Co_PoisonArea;

    public List<Animator> PoisonAnimators;
    public bool IsActive = false;
    public bool Busy = false;

    private void Awake()
    {
        waitForTick = new WaitForSeconds(tickTime);
        col2D = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        col2D.enabled = true;
        isPlayerDamaged = false;
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

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.TryGetComponent(out PlayerStat playerStat);
            if (playerStat != null && !isPlayerDamaged)
            {
                playerStat.SetHP(tickDamage, 0);
                col2D.enabled = false;
                isPlayerDamaged = true;
                Co_PoisonArea = StartCoroutine(PoisonArea());
            }
        }
    }

    IEnumerator PoisonArea()
    {
        while (true)
        {
            yield return waitForTick;
            col2D.enabled = true;
            isPlayerDamaged = false;
            StopCoroutine(Co_PoisonArea);
        }
    }
}