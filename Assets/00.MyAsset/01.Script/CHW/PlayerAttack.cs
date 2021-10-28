using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("���ݼӵ�")]
    public float attackSpeed = 0.5f;
    [Tooltip("���� ����")]
    public BoxCollider2D atkCollider;
    [Tooltip("��ų ���")]
    public float skillFactor = 1.5f;

    Animator anim;
    PlayerMove pm;

    void Start()
    {
        FindAttackCollider();
        atkCollider.enabled = false;

        anim = GetComponentInChildren<Animator>();
        pm = GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (!pm.isDamaged)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(Attack());
                print("�÷��̾� ����");
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                // ��ų���� 1
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                // ��ų���� 2
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                // ��ų���� 3
            }
        }
    }

    IEnumerator Attack()
    {
        atkCollider.enabled = true;
        anim.SetTrigger("Attack1");

        attackSpeed = AnimationTime(AnimationName.Attack1);

        yield return new WaitForSeconds(attackSpeed);
        atkCollider.enabled = false;
    }

    public void FindAttackCollider()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = gameObject.transform.GetChild(i).gameObject;

            if (go.activeSelf)
            {
                atkCollider = go.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
            }
        }
    }

    public enum AnimationName
    { 
        Attack1
    }

    public float AnimationTime(AnimationName animationName)
    {
        string animName = string.Empty;

        switch (animationName)
        {
            case AnimationName.Attack1:
                animName = "Attack1";
                break;
            default:
                break;
        }

        float time = 0;

        RuntimeAnimatorController ac = anim.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == animName)
            {
                time = ac.animationClips[i].length;
            }
        }

        return time;
    }
}
