using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator anim;
    [SerializeField] private const float jump = 100f;

    private static Player instance;

    public static Player GetInstance()
    {
        return instance;
    }

    public event EventHandler Ondied;
    public event EventHandler OnstartedPlaying;

    private State state;

    private enum State
    {
        WaitingToStart,
        Playing,
        Dead
    }
    private void Awake()
    {
        instance = this;
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb2d.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }
    private void Update()
    {
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    state = State.Playing;
                    rb2d.bodyType = RigidbodyType2D.Dynamic;
                    anim.SetTrigger("Voar");
                    Jump();
                    if (OnstartedPlaying != null) OnstartedPlaying(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    anim.SetTrigger("Voar");
                    Jump();
                }
                break;
            case State.Dead:
                break;
        }

    }

    private void Jump()
    {
        rb2d.velocity = Vector2.up * jump;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        rb2d.bodyType = RigidbodyType2D.Static;
        if (Ondied != null) Ondied(this, EventArgs.Empty);
    }
}
