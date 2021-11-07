using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Variable

    [Header("Value for camera movement")]
    [SerializeField, Range(0.01f, 5f)] float followSpeed = 1.0f;
    [SerializeField] float downPadding = 3;
    [SerializeField, Tooltip("The radius of the extent to which Camera Follow does not work")] float paddingRadius = 0f;

    private bool isFollow = true;
    GameObject player;

    #endregion

    #region Property

    #endregion

    #region Unity Life Cycle

    void Start()
    {
        StartCoroutine(FindPlayerWithTag());
        StartCoroutine(Co_Follow());
    }

    private void LateUpdate()
    {
        if (isFollow && player != null && Vector2.Distance(transform.position, player.transform.position + Vector3.up * downPadding) >= paddingRadius)
        {
            Vector3 resultVec = Vector3.Lerp(transform.position, player.transform.position + Vector3.up * downPadding, followSpeed * Time.deltaTime);
            resultVec.z = -10;
            transform.position = resultVec;
        }
    }

    IEnumerator FindPlayerWithTag()
    {
        yield return new WaitUntil(() => player = GameObject.FindGameObjectWithTag("Player"));
        Vector3 initVec = player.transform.position;
        initVec.z = -10;
        initVec.y += downPadding;
        transform.position = initVec;
    }

    #endregion

    #region Implementation Place

    IEnumerator Co_Follow()
    {
        yield return new WaitUntil(() => player);
    }

    public void SetCameraFollow(bool _isFollow = true) => isFollow = _isFollow;

    #endregion
}
