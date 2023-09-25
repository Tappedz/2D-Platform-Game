using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float force;
    public Animator animator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            animator.SetBool("Jump", true);
            rb2d.AddForce(transform.up * force, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        animator.SetBool("Jump", false);
    }
}
