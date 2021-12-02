using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestText : MonoBehaviour
{
    PlayerStat ps;
    PlayerStat PlayerStat => ps = ps ? ps : PlayerSystem.Instance.Player?.GetComponent<PlayerStat>();

    Text txt;
    Text text => txt = txt ? txt : GetComponent<Text>();

    private void Update()
    {
        //text.text = PlayerStat?.currentHP.ToString();
    }

    public void OnEnemy(GameObject go)
    {
        go.SetActive(!go.activeInHierarchy);
    }
}
