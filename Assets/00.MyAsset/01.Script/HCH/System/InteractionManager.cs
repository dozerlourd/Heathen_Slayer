using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    #region Singleton

    static InteractionManager instance;
    public static InteractionManager Instance => instance ? instance : new GameObject("InteractionManager").AddComponent<InteractionManager>();

    void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
    }

    #endregion

    #region Variable

    [SerializeField] List<GameObject> interObjList = new List<GameObject>();
    GameObject nearestObj;
    float nearestValue;

    #endregion

    #region Property

    public GameObject NearestObj
    {
        get
        {
            CheckNearestObj();
            return nearestObj;
        }
    }

    public float NearestValue
    {
        get
        {
            CheckNearestObj();
            return nearestValue;
        }
    }

    #endregion

    #region Unity Life Cycle

    private void Start()
    {
        transform.SetParent(FolderSystem.Instance.SystemFolder);
    }

    #endregion

    #region Implamentaion Place

    public void AddInterList(object _interObjType, GameObject _interactionObject)
    {
        InteractionObject interObj = _interObjType as InteractionObject;
        print(interObj != null);
        if (interObj != null)
            interObjList.Add(_interactionObject);
    }

    void CheckNearestObj()
    {
        int nearestIndex = 0;
        float nearDist = 9999;
        for (int i = 0; i < interObjList.Count; i++)
        {
            float temp = Vector2.Distance(PlayerSystem.Instance.Player.transform.position, interObjList[i].transform.position);
            if (temp < nearDist)
            {
                nearDist = temp;
                nearestIndex = i;
            }
        }
        nearestObj = interObjList[nearestIndex];
        nearestValue = nearDist;
    }

    #endregion
}
