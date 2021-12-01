using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Singleton

    static CameraManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion
    public static CameraManager Instance => instance;

    #region Property

    public CameraFollow CameraFollow => GetComponent<CameraFollow>();
    public CameraShakeController CameraShakeController => GetComponent<CameraShakeController>();

    #endregion

    #region Method

    public void ShakeCamera(float duration, float intensity = 1, bool isX = true, bool isY = true) => StartCoroutine(CameraShakeController.CameraShake(duration, intensity, isX, isY));

    #endregion
}
