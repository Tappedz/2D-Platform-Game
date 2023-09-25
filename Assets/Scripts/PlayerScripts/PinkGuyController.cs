using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PinkGuyController : MonoBehaviour
{

    public float movement = 2;
    public float jump = 3;
    public float doubleJump = 2.5f;
    private bool canDoubleJump;
    Rigidbody2D rb2d;

    public bool betterJump = false;
    public float fall = 0.5f;
    public float lowJump = 1f;

    public SpriteRenderer spriteRenderer;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey("space"))
        {
            if (CheckGround.isGrounded)
            {
                canDoubleJump = true;
                rb2d.velocity = new Vector2(rb2d.velocity.x, jump);
            }
            else if (Input.GetKeyDown("space"))
            {
                if (canDoubleJump)
                {
                    animator.SetBool("DoubleJump", true);
                    rb2d.velocity = new Vector2(rb2d.velocity.x, doubleJump);
                    canDoubleJump = false;
                }
            }
        }

        if (CheckGround.isGrounded == false)
        {
            animator.SetBool("Jump", true);
            animator.SetBool("Run", false);
        }
        if (CheckGround.isGrounded == true)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("DoubleJump", false);
            animator.SetBool("Falling", false);
        }
        if (rb2d.velocity.y < 0)
        {
            animator.SetBool("Falling", true);
        }
        else if (rb2d.velocity.y > 0)
        {
            animator.SetBool("Falling", false);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("a"))
        {
            rb2d.velocity = new Vector2(-movement, rb2d.velocity.y);
            spriteRenderer.flipX = true;
            animator.SetBool("Run", true);
        }
        else if (Input.GetKey("d"))
        {
            rb2d.velocity = new Vector2(movement, rb2d.velocity.y);
            spriteRenderer.flipX = false;
            animator.SetBool("Run", true);
        }
        else
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            animator.SetBool("Run", false);
        }
    }

    public void PlayerDamaged()
    {
        animator.Play("Hit");
        SceneManager.LoadScene("Level1");
    }
}
