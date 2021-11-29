using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    void Start()
    {
        GameObject _player = Instantiate(player);
        _player.TryGetComponent(out SpriteRenderer sp);
        if (sp != null) sp.sortingLayerName = "S_Player";
    }

    void Update()
    {
        
    }
}
