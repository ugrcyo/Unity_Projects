using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehaviour : MonoBehaviour
{
    [SerializeField] GameObject spore;
    [SerializeField] [Range(0, 100)] float throwForce = 5;
    void Start()
    {
        StartCoroutine(Shoot());
    }
    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            GameObject sporeIns = Instantiate(spore, transform.position+Vector3.up, Quaternion.identity);
            Rigidbody2D sporerb = sporeIns.GetComponent<Rigidbody2D>();
            Vector3 dir= new Vector3(Random.Range(-1f,1f), 1, 0);
            sporerb.AddForce(dir.normalized * throwForce, ForceMode2D.Impulse); 
        }
    }

}
