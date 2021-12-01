using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyItemDrop : MonoBehaviour
{
    [Space(15)]
    [Tooltip("Item drop rate - Normal")]
    [SerializeField, Range(0,100)] float lootingRate_Normal;
    [SerializeField] DropItem[] lootItems_Normal;

    [Space(15)]
    [Tooltip("Item drop rate - Rare (This rate is less than Normal Rate)")]
    [SerializeField, Range(0, 100)] float lootingRate_Rare;
    [SerializeField] DropItem[] lootItems_Rare;

    [Space(15)]
    [Tooltip("Item drop rate - Epic (This rate is less than Rare Rate)")]
    [SerializeField, Range(0, 100)] float lootingRate_Epic;
    [SerializeField] DropItem[] lootItems_Epic;

    [Space(15)]
    [Tooltip("Item drop rate - Legendery (This rate is less than Epic Rate)")]
    [SerializeField, Range(0, 100)] float lootingRate_Legendery;
    [SerializeField] DropItem[] lootItems_Legendery;

    public void Looting()
    {
        float rand = new HCH_Random.MersenneTwister().Genrand_Int32(2);
        //print(rand);

        if (rand <= lootingRate_Legendery) {
            ChoiceDropItem(lootItems_Legendery);
        }
        else if(rand <= lootingRate_Epic) {
            ChoiceDropItem(lootItems_Epic);
        }
        else if(rand <= lootingRate_Rare) {
            ChoiceDropItem(lootItems_Rare);
        }
        else if(rand <= lootingRate_Normal) {
            ChoiceDropItem(lootItems_Normal);
        }
    }

    float WeightAmount(DropItem[] _items)
    {
        float sum = 0;

        foreach(DropItem _item in _items) {
            sum += _item.GetWeight();
        }
        return sum;
    }

    void ChoiceDropItem(DropItem[] _items)
    {
        float random = HCH_Random.Random.Genrand_Int32(3);

        CalcWeight(_items, random);
    }

    void CalcWeight(DropItem[] _items, float num)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (num <  _items[i].GetWeight())
            {
                print("¾ÆÀÌÅÛ ³ª¿È" + _items[i].GetItem().name + i);
                Instantiate(_items[i].GetItem(), transform.position, Quaternion.identity);
                break;
            }
            else
            {
                num -= _items[i].GetWeight();
            }
        }

        if (num > WeightAmount(_items))
            CalcWeight(_items, num);
    }
}
