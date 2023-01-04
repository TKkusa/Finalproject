using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Vector2 contactNormal = Vector2.zero;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    private new Collider2D collider;
    [SerializeField] public LayerMask ground;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource jump_sound_effect;
    [SerializeField] private AudioSource hitted_sound_effect;
    [SerializeField] private AudioSource gem_sound;
    [SerializeField] private TextMeshProUGUI GemText;
    [SerializeField] private TextMeshProUGUI instruction;
    [SerializeField] private HealthBar healthbar;
    [SerializeField] private float climbSpeed;

    [HideInInspector] public bool canClimb = false;
    [HideInInspector] public Ladder ladder;
    

    private int gems = 0;
    private int health;
    private float naturalGravity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();       
        collider = GetComponent<Collider2D>();
        health = 3;
        healthbar.SetHealth(health);
        StartCoroutine(TurnOffInstruction());
        naturalGravity = rb.gravityScale;
        climbSpeed = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }

        if (!animator.GetBool("hurt"))
        {
            if (canClimb)
            {
                animator.SetBool("climb", true);
                Climb();
                rb.gravityScale = 0f;
            }
            else if (!canClimb) {
                rb.gravityScale = naturalGravity;
                animator.SetBool("climb", false);
                animator.speed = 1f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-5, rb.velocity.y);
                transform.localScale = new Vector2(-1, 1);
                if (collider.IsTouchingLayers(ground))
                    animator.SetBool("running", true);
                animator.SetBool("crouching", false);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(5, rb.velocity.y);
                transform.localScale = new Vector2(1, 1);
                if (collider.IsTouchingLayers(ground))
                    animator.SetBool("running", true);
                animator.SetBool("crouching", false);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animator.SetBool("crouching", true);
                animator.SetBool("running", false);
            }
            else
            {
                if(contactNormal.x<1 && contactNormal.y == 1)
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetBool("running", false);
                animator.SetBool("crouching", false);
            }

        }

        


        // jump animation
        if (rb.velocity.y > 0.1f && !canClimb)
        {
            animator.SetBool("jumping",true);
        }
        else if(rb.velocity.y < 0.1f && !collider.IsTouchingLayers(ground) && !canClimb)
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
        if(collision.gameObject.layer == LayerMask.NameToLayer("ground") && !canClimb)
        {
            if (contactNormal.x == -1 && contactNormal.y < 1) // attach right
            {
                if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D))
                {
                    rb.velocity = new Vector2(-5, 10f);
                    transform.localScale = new Vector2(-1, 1);
                    animator.SetBool("running", false);
                    jump_sound_effect.Play();
                }
            }
            else if(contactNormal.x == 1 && contactNormal.y < 1) // attach left
            {
                if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A))
                {
                    rb.velocity = new Vector2(5, 10f);
                    transform.localScale = new Vector2(1, 1);
                    animator.SetBool("running", false);
                    jump_sound_effect.Play();
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.W) && !animator.GetBool("hurt"))
                {
                    animator.SetBool("running", false);
                    animator.SetBool("crouching", false);
                    rb.velocity = new Vector2(rb.velocity.x, 10f);
                    jump_sound_effect.Play();
                }
            }
            // github test
        }

        if (collision.gameObject.tag == "Enemy") {

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (animator.GetBool("falling") && transform.position.y >= collision.collider.bounds.max.y)
            {
                enemy.JumpedOn();
                rb.AddForce(new Vector2(0, 700f));
            }
            else
            {
                healthbar.SetHealth(--health);
                if (health <= 0) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                if (collision.transform.position.x > transform.position.x)
                {
                    rb.AddForce(new Vector2(-200f, 100f));
                }
                else {
                    rb.AddForce(new Vector2(200f, 100f));
                }
                animator.SetBool("hurt", true);
                StartCoroutine(notHurt());
                hitted_sound_effect.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.tag == "Cherry") {
            if (health < 5)
            {
                healthbar.SetHealth(++health);
            }
            cherry.Play();
            Destroy(collision.gameObject);
        }
        if (collision.tag == "Gem")
        {
            gems++;
            GemText.text = gems.ToString();
            gem_sound.Play();
            Destroy(collision.gameObject);
        }
    }

    private void FootStep() { 
        footstep.Play();
    }

    private IEnumerator TurnOffInstruction() {
        yield return new WaitForSeconds(3);
        instruction.enabled = false;
    }

    private void Climb() {
        float vDirection = Input.GetAxis("Vertical");

        if (vDirection > .1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, vDirection * climbSpeed);
            animator.speed = 1f;
        }
        else if (vDirection < -.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, vDirection * climbSpeed);
            animator.speed = 1f;
        }
        else {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            animator.speed = 0f;
        }
    }

    private IEnumerator notHurt() {
        yield return new WaitForSeconds(0.7f);
        animator.SetBool("hurt", false);
    }

}
