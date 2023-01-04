using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private float rightCap;
    [SerializeField] private float leftCap;

    [SerializeField] private float jumpLength;
    [SerializeField] private LayerMask ground;

    private bool facingLeft = true;


    protected override void Start()
    {
        base.Start();
        jumpLength = 2f;
    }

    private void Update()
    {


    }

    private void Move()
    {
        animator.SetBool("Move", true);
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

                rb.velocity = new Vector2(-jumpLength, 1f);

            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

                rb.velocity = new Vector2(jumpLength, 1f);

            }
            else
            {
                facingLeft = true;
            }
        }
    }

    private void Idle() {
        animator.SetBool("Move", false);
    }

    private void Next()
    {
        animator.SetBool("Move", true);
    }

    //public void JumpedOn() {
    //    animator.SetTrigger("Death");
    //}

    //private void Death()
    //{
    //    Destroy(gameObject);
    //}
}
