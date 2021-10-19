using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject target;

    public float currentTime = 0;
    public float delayTime = 3;
    void Start()
    {
    }

    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, target.transform.position, 0.001f);
    }
}
