using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaman_BlowDart : MonoBehaviour
{
    #region Variable

    public float Duration;
    public Rigidbody2D Body;

    #endregion

    #region Unity Life Cycle

    void OnEnable() => StartCoroutine(VanishAfterSetTime());

    #endregion

    #region Implementation Place

    IEnumerator VanishAfterSetTime()
    {
        yield return new WaitForSeconds(Duration);
        Vanish();
    }

    void Vanish() => gameObject.SetActive(false);

    #region Callback Method

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerStat>()?.SetHP(8, 0.7f);
            Vanish();
        }
    }

    #endregion

    #endregion
}