using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFromEnemyFlip : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Start() => spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();

    private void Update() => transform.eulerAngles = new Vector3(0, spriteRenderer.flipX ? 180 : 0, 0);
}
