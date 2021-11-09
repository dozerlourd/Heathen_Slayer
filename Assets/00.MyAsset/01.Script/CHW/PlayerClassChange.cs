using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassChange : MonoBehaviour
{
    public RuntimeAnimatorController[] animController;

    PlayerAttack playerAttack;
    BoxCollider2D bc;

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        playerAttack = GetComponent<PlayerAttack>();
        ClassChangeAnimator();
    }

    void Update()
    {

    }

    void ClassChangeAnimator()
    {
        for (int i = 0; i < animController.Length; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.activeSelf)
            {
                GetComponentInChildren<Animator>().runtimeAnimatorController = animController[i];
            }
        }

        playerAttack.FindAttackCollider();

        transform.position = transform.position + (transform.up * 3);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Corpse"))
        {
            // Space 키를 누르면 클래스가 변경
            if (Input.GetKey(KeyCode.Space))
            {
                if (col.gameObject.name == "GhostWarrior")
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(false);

                    bc.offset = new Vector2(0, 0.25f);
                    bc.size = new Vector2(1, 2.25f);

                    ClassChangeAnimator();

                    Destroy(col.gameObject);
                }
                else if (col.gameObject.name == "Rogue")
                {
                    transform.GetChild(1).gameObject.SetActive(true);
                    transform.GetChild(0).gameObject.SetActive(false);

                    bc.offset = new Vector2(-0.1f, 0.7f);
                    bc.size = new Vector2(1, 1.25f);

                    ClassChangeAnimator();

                    Destroy(col.gameObject);
                }
            }
        }
    }
}
