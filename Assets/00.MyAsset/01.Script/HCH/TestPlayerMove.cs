using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMove : MonoBehaviour
{

    [SerializeField] float speed = 5f;

    Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(h * Time.deltaTime * speed, 0);

        if(Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector2.up * 12, ForceMode2D.Impulse);
        }
    }
}
