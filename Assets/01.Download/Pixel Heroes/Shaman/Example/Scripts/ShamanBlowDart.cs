using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanBlowDart : MonoBehaviour
{
    public float Duration;
    public Rigidbody2D Body;
    
    void Start()
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
}