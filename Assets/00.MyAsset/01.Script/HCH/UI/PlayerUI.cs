using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private void Start()
    {
        SearchingPlayer();
    }

    IEnumerator SearchingPlayer()
    {
        yield return new WaitUntil(() => PlayerSystem.Instance.Player);
    }
}
