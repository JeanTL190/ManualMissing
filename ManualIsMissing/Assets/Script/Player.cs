using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator anim;
    [SerializeField] private const float jump = 100f;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Voar");
            Jump();
        }
    }

    private void Jump()
    {
        rb2d.velocity = Vector2.up * jump;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Dead");
    }
}
