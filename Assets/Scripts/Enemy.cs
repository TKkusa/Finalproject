using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rb;
    protected Collider2D coll;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }
    public void JumpedOn()
    {
        rb.velocity = Vector2.zero;
        coll.enabled = false;
        animator.SetTrigger("Death");
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
