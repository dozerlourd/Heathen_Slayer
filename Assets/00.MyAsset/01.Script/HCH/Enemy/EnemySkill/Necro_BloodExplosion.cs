using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necro_BloodExplosion : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(KillThePlayer());
    }

    IEnumerator KillThePlayer()
    {
        yield return new WaitForSeconds(2f);

        PlayerSystem.Instance.Player.GetComponent<PlayerStat>().SetHP(10000, 0f);
    }
}
