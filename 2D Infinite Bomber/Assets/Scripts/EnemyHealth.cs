using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]float maxHp;
    float currentHp;
    [SerializeField] Image hpBar;
    void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (hpBar!=null)
        {
            UpdateHpUI(); 
        }
        if (currentHp<=0)
        {
            Die();
        }
    }
    void UpdateHpUI()
    {
        hpBar.fillAmount = currentHp / maxHp;
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
