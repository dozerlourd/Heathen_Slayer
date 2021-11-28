using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    void Start()
    {
        Instantiate(player);
    }

    void Update()
    {
        
    }
}
