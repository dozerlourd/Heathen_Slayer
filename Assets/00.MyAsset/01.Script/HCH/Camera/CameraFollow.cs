using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Variable

    [Header("Value for camera movement")]
    [SerializeField, Range(0.1f, 10f)] float followSpeed = 1.0f;
    [SerializeField] float downPadding = 3;
    [SerializeField, Tooltip("The radius of the extent to which Camera Follow does not work")] float paddingRadius = 0f;


    GameObject player;

    #endregion

    #region Property

    #endregion

    #region Unity Life Cycle

    void Start() => StartCoroutine(FindPlayerWithTag());

    IEnumerator FindPlayerWithTag()
    {

        yield return new WaitUntil(() => player = GameObject.FindGameObjectWithTag("Player"));
        Vector3 initVec = player.transform.position;
        initVec.z = -10;
        initVec.y += downPadding;
        transform.position = initVec;
    }

    private void LateUpdate()
    {
        if (player == null || Vector2.Distance(transform.position, player.transform.position + Vector3.up * downPadding) <= paddingRadius) {
            return;
        }
        StartCoroutine(Co_Follow());
    }

    #endregion

    #region Implementation Place

    IEnumerator Co_Follow()
    {
        while(Vector2.Distance(transform.position, player.transform.position + Vector3.up * downPadding) > 0.05f)
        {
            Vector3 resultVec = Vector3.Lerp(transform.position, player.transform.position + Vector3.up * downPadding, followSpeed * Time.deltaTime);
            resultVec.z = -10;
            transform.position = resultVec;
            yield return null;
        }
    }

    #endregion
}
