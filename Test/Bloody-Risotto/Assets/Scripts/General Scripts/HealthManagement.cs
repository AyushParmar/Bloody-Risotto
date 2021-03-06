using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagement : MonoBehaviour
{
    public int health = 10;
    public GameObject deathEffect;
    public bool dieCalled=false;
    
    private Animator anim;
    private bool gotAnimator;

    private void Start()
    {
        gotAnimator = TryGetComponent<Animator>(out anim);
    }

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
        if (gotAnimator)
        {
            dieCalled = true;
        }
        else
        {
            GameObject newExplosion=Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(newExplosion, 0.725f);
        }
    }
}
