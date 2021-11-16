using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    [SerializeField] GameObject boss;
    [SerializeField] GameObject ClearCanvas;
    [SerializeField] GameObject[] enemies;
    int enemyCount;

    private void Start()
    {
        boss.SetActive(false);
        StartCoroutine(EnemyCheck());
    }

    private IEnumerator EnemyCheck()
    {
        while (!boss.activeInHierarchy)
        {
            enemyCount = 0;
            yield return new WaitForSeconds(2);
            for (int i = 0; i < enemies.Length; i++)
            {
                if (!enemies[i].GetComponent<EnemyHP>().IsDead) enemyCount++;
            }
            //print(enemyCount);
            if(enemyCount == 0)
            {
                boss.SetActive(true);
                StartCoroutine(BossFight());
            }
        }
    }

    IEnumerator BossFight()
    {
            yield return new WaitUntil(() => boss.GetComponent<BossHP_Shaman>().IsDead);
            ClearCanvas.SetActive(true);
    }
}
