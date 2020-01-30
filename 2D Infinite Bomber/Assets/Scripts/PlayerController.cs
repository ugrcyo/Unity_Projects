using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rB2D; //Karakterimizin 2D uzayda fiziksel haraketi için
    [SerializeField] float speed = 2; //Hız ayarlaması için 
    Animator anim; //Karakterimizin animasyon kontrolü özelliğin için
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
        rB2D = GetComponent<Rigidbody2D>(); //rB2D değişkeninin atamasını yapıyoruzs
        anim = GetComponent<Animator>();    //anim değişkeninin atamasını yapıyoruz
    }
    private void Update()
    {
        fireTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftControl))//GetMouseButton mouse click için inputumuz 0 sol tık 1 sağ tık 2 ise orta tuşa denk gelmektedir 
        {
            if (fireTimer>fireRate)
            {
                fireTimer = 0;
                if (isFacingRight)
                {
                    Instantiate(projectile, barrel.position, Quaternion.Euler(new Vector3(0, 0, -90)));//Instantiate verilen objeyi klonlar ve sahneye yerleştirir ilk parametre objeyi 2. parametre pozisyonunu 3. parametre ise açısını belirler Quaternion.Euler arayüzdeki gibi x,y,z değerleri olarak açı vermemizi sağlar
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
        // Debug.Log(rB2D.velocity.y);
        // Debug.LogWarning("<color=red>RAW: "+ Input.GetAxisRaw("Horizontal")+"</color>");//Debug yazdırılırken başına <color=renk> sonuna </color> eklenirse verilen renkte yazdırılır eğer Debug.Log sonuna Warning veya Error eklenirse yazı ikonu değiştirilir
        //  Debug.LogError("<color=blue>Normal: " + Input.GetAxis("Horizontal") + "</color>");
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rB2D.velocity.y); //input ile gidilecek birim yön vektörünü oluşturuyoruz.
        if (!takingDamage)
        {
            rB2D.velocity = dir; 
        }//Rigidbody2D nin hız değerini belirliyoruz
        anim.SetBool("isWalking", 0 != Mathf.Abs(dir.x)); //Animasyon parametresi olan isWalking değerine input alınıp alınmadığını yazıyoruz (giriş değerinin mutlak değeri 1 mi)
        anim.SetFloat("velocityY", rB2D.velocity.y);
        if (rB2D.velocity.x > 0 && !isFacingRight)//Karakterin gittiği yönde döndürmek için gittiği yönü ve şu anda dönük olduğu yönü kontrol ediyoruz
        {
            isFacingRight = !isFacingRight;//Şu anda dönük olduğu yönü tutuyoruz
            Reverse();//Karakterin scale ini - ile çarparak ters çeviriyoruz
        }
        else if (rB2D.velocity.x < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Reverse();
        }
        if (/*Input.GetAxis("Jump")==1||*/Input.GetKey(KeyCode.Space) && isGrounded)
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
        //   transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);
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
