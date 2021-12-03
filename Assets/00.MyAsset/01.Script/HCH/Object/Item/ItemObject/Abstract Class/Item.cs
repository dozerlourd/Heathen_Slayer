using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, InteractionObject
{
    #region Variable

    #region Only Parent Variable

    [SerializeField] Vector3 rayOriginLocalPos;

    [SerializeField] float rayDist = 0.03f;

    [SerializeField] float gravity = 0.08f;

    WaitForSeconds floatingWaitTime;

    Vector3 moveVec = Vector3.zero;

    bool isSpawnComplete = false;

    #endregion

    #region Item Informations

    ItemInfoUI itemInfoUI;
    GameObject itemInfoPanel;

    #region Number Information
    [Header("Spread Sheet Item Index")]
    [SerializeField] protected int index;

    protected string capacity1_Name;
    protected float capacity1_Coef;

    protected string capacity2_Name;
    protected float capacity2_Coef;
    #endregion

    #endregion

    #endregion

    #region Unity Life Cycle

    protected void Start()
    {
        floatingWaitTime = new WaitForSeconds(1 / ItemManager.Instance.FloatingSpeed);
        itemInfoUI = GetComponentInChildren<ItemInfoUI>();
        itemInfoPanel = itemInfoUI.gameObject;
        itemInfoPanel.SetActive(false);

        OnStart();

        itemInfoUI.SetInfoText(index); 

        StartCoroutine(RandomFly());
    }

    private void Update()
    {
        if (!isSpawnComplete) return;
        ShowItemInfo(Vector2.Distance(PlayerSystem.Instance.Player.transform.position, transform.position) <= ItemManager.Instance.ShowInfoPanelDist);
        itemInfoPanel.transform.position = transform.position + Vector3.up * 4; /*PlayerSystem.Instance.Player.transform.position + new Vector3(ItemManager.Instance.ShowUIPaddingX, ItemManager.Instance.ShowUIPaddingY, 0)*/;
    }

    #endregion

    #region Implementation Place

    #region Spawning Method
    private IEnumerator RandomFly()
    {
        Vector2 ItemFlyingVec = ItemManager.Instance.ItemFlyingVec;
        Vector3 randomVec = Vector3.Lerp(new Vector3(-ItemFlyingVec.x, ItemFlyingVec.y), new Vector3(ItemFlyingVec.x, ItemFlyingVec.y), Random.Range(0f, 1f));
        float power = 0.45f;
        float time = 0;

        while (!Physics2D.Raycast(transform.position + rayOriginLocalPos, Vector3.down, rayDist, LayerMask.GetMask("L_Ground")) || time < 0.3f)
        {
            //print(time);
            moveVec += randomVec * (Mathf.Max(power -= Time.deltaTime * 8, 0) * Time.deltaTime);
            moveVec += Vector3.down * gravity * Time.deltaTime;
            //print(randomVec * (Mathf.Max(power -= Time.deltaTime, 0) * Time.deltaTime));
            time += Time.deltaTime;
            transform.Translate(moveVec);
            yield return null;
        }
        isSpawnComplete = true;
        StartCoroutine(ItemFloating());
    }
    #endregion

    #region Floating Method
    private IEnumerator ItemFloating()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return FloatingOnce();
        }
    }

    private IEnumerator FloatingOnce()
    { 
        iTween.MoveTo(gameObject, iTween.Hash("y", transform.position.y + ItemManager.Instance.FloatingIntencity,
                                              "time", 1 / ItemManager.Instance.FloatingSpeed, "easetype",
                                              iTween.EaseType.easeInOutSine));
        yield return floatingWaitTime;
        iTween.MoveTo(gameObject, iTween.Hash("y", transform.position.y - ItemManager.Instance.FloatingIntencity,
                                              "time", 1 / ItemManager.Instance.FloatingSpeed, "easetype",
                                              iTween.EaseType.easeInOutSine));
        yield return floatingWaitTime;
    }
    #endregion

    #region Method to Show Item Info

    private void ShowItemInfo(bool isActive)
    {
        itemInfoPanel.SetActive(isActive);
    }

    #endregion

    #region Override Method

    protected abstract void Execute();

    protected virtual void OnStart() { }

    public void Interaction()
    {
        Destroy(gameObject);
    }

    #endregion

    #endregion
}
