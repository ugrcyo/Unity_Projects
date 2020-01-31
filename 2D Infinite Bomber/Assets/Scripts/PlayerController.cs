using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rB2D; 
    [SerializeField] float speed = 2;
    Animator anim; 
    [SerializeField]Animator bloodFx;
    bool isFacingRight = true;
    [SerializeField] float jumpForce = 100;
    [SerializeField] float pushForce = 100;
    bool isGrounded = true;
    bool takingDamage = false;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform barrel;
    [SerializeField] Image hpBar;
    [SerializeField] Text scoreTxt;
    int hp = 50;
    int score = 0;
    float fireRate = 0.7f, fireTimer;
    void Start()
    {
        UpdateHealthUI();
        rB2D = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>();    
    }
    private void Update()
    {
        fireTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (fireTimer>fireRate)
            {
                fireTimer = 0;
                if (isFacingRight)
                {
                    Instantiate(projectile, barrel.position, Quaternion.Euler(new Vector3(0, 0, -90)))
                }
                else
                {
                    Instantiate(projectile, barrel.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                } 
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            hp -= 25;
            UpdateHealthUI();
        }
    }
    void UpdateHealthUI()
    {
        hpBar.fillAmount = hp / 100f;
        scoreTxt.text = ""+score;
    }
    void FixedUpdate()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rB2D.velocity.y); 
        if (!takingDamage)
        {
            rB2D.velocity = dir; 
        }
        anim.SetBool("isWalking", 0 != Mathf.Abs(dir.x)); 
        anim.SetFloat("velocityY", rB2D.velocity.y);
        if (rB2D.velocity.x > 0 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Reverse();
        }
        else if (rB2D.velocity.x < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Reverse();
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rB2D.AddForce(new Vector2(0, jumpForce));
            isGrounded = false;
            anim.SetBool("isJumping", true);
        }
        else if (!isGrounded|| takingDamage)
        {
            anim.SetBool("isJumping", false);
            if (Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundMask))
            {
                takingDamage = false;
                isGrounded = true;
            }
        }
    }
    void Reverse()
    {
        Vector3 charScale = transform.localScale;
        charScale.x *= -1;
        transform.localScale = charScale;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name=="Heart")
        {
            hp = Mathf.Clamp(hp + 20, 0, 100);
        }
        else if (other.gameObject.name == "Star")
        {
            score += 5;
        }
        Destroy(other.gameObject);
        UpdateHealthUI();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Damager")
        {
            takingDamage = true;
            Vector3 pushDir = transform.position - collision.gameObject.transform.position;
            rB2D.AddForce(pushDir.normalized * pushForce,ForceMode2D.Impulse);
            TakeDamage(15);
            bloodFx.SetTrigger("isDamaged"); 
        }
    }
    void TakeDamage(int damage)
    {
        hp -= damage;
        UpdateHealthUI();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }

}
