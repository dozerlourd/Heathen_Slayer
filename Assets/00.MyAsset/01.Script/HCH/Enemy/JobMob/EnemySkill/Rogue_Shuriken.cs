using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue_Shuriken : MonoBehaviour
{
    public float Duration;
    public Rigidbody2D Body;

    void OnEnable()
    {
        StartCoroutine(VanishAfterSetTime());
    }

    void Vanish()
    {
        gameObject.SetActive(false);
    }

    IEnumerator VanishAfterSetTime()
    {
        yield return new WaitForSeconds(Duration);
        Vanish();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerStat>()?.SetHP(8, 0.7f);
            Vanish();
        }
    }
}
