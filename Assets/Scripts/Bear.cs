using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Enemy
{
    [SerializeField] private float rightCap;
    [SerializeField] private float leftCap;

    [SerializeField] private float jumpLength;
    [SerializeField] private LayerMask ground;

    private bool facingLeft = false;


    protected override void Start()
    {
        base.Start();
        jumpLength = 5f;
    }

    private void Update()
    {


    }

    private void Move()
    {

        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

                rb.velocity = new Vector2(-jumpLength, 0);
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
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

                rb.velocity = new Vector2(jumpLength, 0);

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
