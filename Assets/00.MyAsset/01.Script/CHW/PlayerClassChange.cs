using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassChange : MonoBehaviour
{
    public RuntimeAnimatorController[] animController;

    void Start()
    {
        ClassChangeAnimator();
    }

    void Update()
    {
        // Space 키를 누르면 클래스가 변경
        if (Input.GetKey(KeyCode.Space))
        {

        }
    }

    void ClassChangeAnimator()
    {
        for (int i = 0; i < animController.Length; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.activeSelf)
            {
                GetComponent<Animator>().runtimeAnimatorController = animController[i];
            }
        }
    }
}
