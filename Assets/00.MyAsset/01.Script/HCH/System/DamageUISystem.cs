using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageUISystem : MonoBehaviour
{
    #region Singleton

    static DamageUISystem instance;
    public static DamageUISystem Instance => instance ? instance : new GameObject("DamageUISystem").AddComponent<DamageUISystem>();

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
    }

    #endregion

    #region Variable

    [SerializeField] int damageUICount = 50;

    #endregion

    public GameObject[] damageTextArray;

    private void Start()
    {
        damageTextArray = new GameObject[50];

        transform.SetParent(FolderSystem.Instance.SystemFolder);
        damageTextArray = HCH.GameObjectPool.GeneratePool(Resources.Load("DamageText") as GameObject, damageUICount, FolderSystem.Instance.DamageText_UIPool, false);
    }

    public void DisplayDamageText(float dmg, Transform enemyTr)
    {
        //print(damageTextArray.Length);
        GameObject text = HCH.GameObjectPool.PopObjectFromPool(damageTextArray, enemyTr.position + Vector3.up, true);
        //print(text.name);
        text.TryGetComponent(out DamagedText damagedText);
        if (damagedText) damagedText.SetDamagedText(dmg);
        else print("DamageText 스크립트 참조 불가");
    }
}
