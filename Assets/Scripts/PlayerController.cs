using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    

    private int gems = 0;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();       
        collider = GetComponent<Collider2D>();
        health = 3;
        healthbar.SetHealth(health);
        StartCoroutine(TurnOffInstruction());
   
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
        animator.SetBool("hurt", false);
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
                if (Input.GetKey(KeyCode.W))
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

            if (animator.GetBool("falling"))
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

}
