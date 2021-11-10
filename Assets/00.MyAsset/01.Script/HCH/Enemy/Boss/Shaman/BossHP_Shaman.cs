using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP_Shaman : HPControllerToEnemy
{
    [SerializeField] float stunTime = 5.0f;
    int paze = 1;

    SpriteRenderer spriteRenderer;

    Coroutine damageColor;

    SpriteRenderer SpriteRenderer => spriteRenderer = spriteRenderer ? spriteRenderer : GetComponent<SpriteRenderer>();

    // 배열로 특정 체력 비율을 뺴놓고 비교해서 피격 상태로 변경한다
    protected override IEnumerator EnemyDamaged()
    {
        if(damageColor != null) StopCoroutine(damageColor);
        damageColor = StartCoroutine(DamageColor());

        if (NormalizedCurrHP < 0.7f && paze == 1)
        {
            yield return StartCoroutine(EnemyFSM.Stun(stunTime));
            paze++;
        }
        else if (NormalizedCurrHP < 0.3f && paze == 2)
        {
            yield return StartCoroutine(EnemyFSM.Stun(stunTime));
            paze++;
        }
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
