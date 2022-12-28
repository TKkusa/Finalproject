using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private float rightCap;
    [SerializeField] private float leftCap;

    [SerializeField] private float jumpLength;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask ground;

    private bool facingLeft = true;


    protected override void Start()
    {
        base.Start();
        jumpLength = 5f;
        jumpHeight = 4f;
    }

    private void Update()
    {
        if (animator.GetBool("Jumping"))
        {
            if (rb.velocity.y < .1)
            {
                animator.SetBool("Falling", true);
                animator.SetBool("Jumping", false);
            }
        }
        else if (coll.IsTouchingLayers(ground) && animator.GetBool("Falling")) {
            animator.SetBool("Falling", false);
        } 


    }

    private void Move() {

        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    animator.SetBool("Jumping", true);
                }
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
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    animator.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }

    //public void JumpedOn() {
    //    animator.SetTrigger("Death");
    //}

    //private void Death()
    //{
    //    Destroy(gameObject);
    //}
}
