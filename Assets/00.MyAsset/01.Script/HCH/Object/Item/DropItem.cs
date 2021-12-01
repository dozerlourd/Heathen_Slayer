using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropItem
{
    [SerializeField] Item item;
    [SerializeField] float weight;

    public Item GetItem() => item;

    public float GetWeight() => weight;
}
