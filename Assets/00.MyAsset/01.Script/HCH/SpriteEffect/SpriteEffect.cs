using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffect : MonoBehaviour
{
    #region Variable

    [SerializeField] float Duration;

    #endregion

    #region Unity Life Cycle

    void OnEnable() => StartCoroutine(VanishAfterSetTime());

    #endregion

    #region Implementation Place

    IEnumerator VanishAfterSetTime()
    {
        yield return new WaitUntil(() => GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f);
        Vanish();
    }

    void Vanish() => gameObject.SetActive(false);

    #endregion
}
