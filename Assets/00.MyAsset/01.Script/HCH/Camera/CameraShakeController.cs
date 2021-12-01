using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    #region Variable



    #endregion

    #region Property



    #endregion

    #region Unity Life Cycle



    #endregion

    #region Implementation Place

    public IEnumerator CameraShake(float duration, float intensity = 0.5f, bool isX = true, bool isY = true)
    {
        Vector3 originPos = transform.localPosition;
        float elapsed = 0;

        while (elapsed < duration)
        {
            float x = isX ? Random.Range(-1f, 1f) * intensity : 0;
            float y = isY ? Random.Range(-1f, 1f) * intensity : 0;

            transform.localPosition += new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originPos;
    }

    #endregion
}
