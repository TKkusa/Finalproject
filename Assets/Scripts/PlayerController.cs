using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 contactNormal = Vector2.zero;

    public Rigidbody2D rb;
    public Animator animator;
    private new Collider2D collider;
    [SerializeField] public LayerMask ground;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();       
        collider = GetComponent<Collider2D>();       
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-5, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
            if(collider.IsTouchingLayers(ground))
            animator.SetBool("running", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(5, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
            if(collider.IsTouchingLayers(ground))
            animator.SetBool("running", true);
        }
        else
        {          
            animator.SetBool("running", false);
        }



        // jump animation
        if(rb.velocity.y > 0.1f)
        {
            animator.SetBool("jumping",true);
        }
        else if(rb.velocity.y < 0.1f && !collider.IsTouchingLayers(ground))
        {
            animator.SetBool("falling", true);
        }
        else
        {
            animator.SetBool("falling",false);
            animator.SetBool("jumping",false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        contactNormal = collision.GetContact(0).normal;
        if(collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            if (contactNormal.x == -1 && contactNormal.y < 1) // attach right
            {
                if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D))
                {
                    rb.velocity = new Vector2(-5, 10f);
                    transform.localScale = new Vector2(-1, 1);
                    animator.SetBool("running", false);
                }
            }
            else if(contactNormal.x == 1 && contactNormal.y < 1) // attach left
            {
                if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A))
                {
                    rb.velocity = new Vector2(5, 10f);
                    transform.localScale = new Vector2(1, 1);
                    animator.SetBool("running", false);
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
                {
                    rb.velocity = new Vector2(rb.velocity.x, 10f);
                    animator.SetBool("running", false);
                }
            }
        }
    }
}
