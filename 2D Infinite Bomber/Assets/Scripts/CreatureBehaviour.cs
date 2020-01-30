using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class CreatureBehaviour : MonoBehaviour
{
    [SerializeField] Transform player;
    Rigidbody2D rb;
    [SerializeField] float viewDist;
    [SerializeField] float speed;
    bool isFacingRight = false;
    Animator anim;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        anim.SetBool("isRunning", 0 != rb.velocity.x);
        if (Vector3.Distance(player.position, transform.position) < viewDist)
        {
            if ((player.position.x - transform.position.x) < 0)
            {
                rb.velocity = new Vector3(-1, rb.velocity.y, 0) * speed;
                if (isFacingRight) Reverse();
            }
            else
            {
                rb.velocity = new Vector3(1, rb.velocity.y, 0) * speed;
                if (!isFacingRight) Reverse();

            }
        }

    }
    void Reverse()
    {
        isFacingRight = !isFacingRight;
        Vector3 charScale = transform.localScale;
        charScale.x *= -1;
        transform.localScale = charScale;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, viewDist);
    }
}
