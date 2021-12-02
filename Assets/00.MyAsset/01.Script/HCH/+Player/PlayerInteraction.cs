using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interactionDist = 3;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Interaction();
        }
    }

    void Interaction()
    {
        if (InteractionManager.Instance.NearestValue <= interactionDist)
        {
            InteractionManager.Instance.NearestObj.GetComponent<InteractionObject>().Interaction();
        }
    }
}
