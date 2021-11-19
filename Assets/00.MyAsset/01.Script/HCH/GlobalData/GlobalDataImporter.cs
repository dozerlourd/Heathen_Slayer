using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class GlobalDataImporter : MonoBehaviour
{
    [SerializeField]
    GoogleSheetData googleSheetData;

    [System.Serializable]
    public class GoogleSheetData
    {
        [Header("�нú� ������")]
        public TextAsset passiveItem;
    }

    void Awake()
    {
        GlobalState.passiveItemList = JsonConvert.DeserializeObject<List<Excel_PassiveItem>>(googleSheetData.passiveItem.ToString());
    }
}
