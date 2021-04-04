using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagement : MonoBehaviour
{
    public int health = 10;
    public GameObject deathEffect;
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);

        if(health<=0)
        {
            Die();
        }
    }
    void Die()
    {
        GameObject newExplosion=Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(newExplosion, 0.725f);
    }
}
