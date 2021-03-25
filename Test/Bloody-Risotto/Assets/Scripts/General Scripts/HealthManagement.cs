using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagement : MonoBehaviour
{
    public int health = 10;
    public GameObject deathEffect;
    public Animator anim;

    private bool gotAnimator;

    private void Start()
    {
        gotAnimator = TryGetComponent<Animator>(out anim);
        Debug.Log(gotAnimator + "yes");
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
            anim.SetBool("isDead", true);
        }
        else
        {
            GameObject newExplosion=Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(newExplosion, 0.725f);
        }
    }
}
