using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]GameObject explosionEffect;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject,5);
    }

    void Update()
    {
        rb.AddForce(transform.up*0.5f,ForceMode2D.Impulse);
    }
    void OnTriggerEnter2D(Collider2D collision) 
    {
        EnemyHealth eH= collision.gameObject.GetComponent<EnemyHealth>();
        if (eH)
        {
            eH.TakeDamage(15);
        }
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
   
}
