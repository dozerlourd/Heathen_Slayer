using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP_Necromancer : HPControllerToEnemy
{
    [SerializeField] float stunTime = 5.0f;
    int paze = 1;

    SpriteRenderer spriteRenderer;

    Coroutine damageColor;

    SpriteRenderer SpriteRenderer => spriteRenderer = spriteRenderer ? spriteRenderer : GetComponent<SpriteRenderer>();

    protected override IEnumerator EnemyDamaged()
    {
        if (damageColor != null) StopCoroutine(damageColor);
        damageColor = StartCoroutine(DamageColor());

        yield return null;
    }

    protected override IEnumerator EnemyDead()
    {
        isDead = true;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(corpseTime);
        gameObject.SetActive(false);
    }

    protected override void RefreshUI(float _val)
    {
        
    }

    IEnumerator DamageColor()
    {
        print("ChangeColor");
        SpriteRenderer.color = new Color(0.9372549f, 0.4980392f, 0.4980392f);

        yield return new WaitForSeconds(0.05f);

        SpriteRenderer.color = Color.white;
    }
}
