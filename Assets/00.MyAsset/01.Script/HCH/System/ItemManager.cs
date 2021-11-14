using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    #region Singleton

    static ItemManager instance;
    public static ItemManager Instance => instance ? instance : new GameObject("ItemManager").AddComponent<ItemManager>();

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
    }

    #endregion

    #region SerializedField

    [SerializeField, Range(0.01f, 10)] float floatingSpeed = 0.67f;
    [SerializeField] float floatingIntencity = 0.45f;
    [SerializeField] float itemFlyingVecX = 2, itemFlyingVecY = 4;

    [SerializeField] float showInfoPanelDist = 5;

    [SerializeField] float showUIPaddingX = 1;
    [SerializeField] float showUIPaddingY = 2.5f;

    #region NonSerializedField

    #endregion

    #region Private



    #endregion

    #endregion

    #region Property

    public float FloatingSpeed => floatingSpeed;
    public float FloatingIntencity => floatingIntencity;

    public Vector2 ItemFlyingVec => new Vector2(itemFlyingVecX, itemFlyingVecY);

    public float ShowInfoPanelDist => showInfoPanelDist;
    public float ShowUIPaddingX => showUIPaddingX;
    public float ShowUIPaddingY => showUIPaddingY;

    #endregion

    #region Unity Life Cycle

    private void Start()
    {
        transform.SetParent(FolderSystem.Instance.SystemFolder);
    }

    #endregion
}
