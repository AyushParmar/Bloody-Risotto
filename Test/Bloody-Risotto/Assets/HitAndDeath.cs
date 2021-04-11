using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAndDeath : MonoBehaviour
{  
    [SerializeField] MonoBehaviour scriptToDisable;
    [SerializeField] int hp;

    Animator anim;
    public bool dead=false;
    HealthManagement health;
    void Start()
    {
        health = GetComponent<HealthManagement>();
        hp = health.health;
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health.health!=hp)
        {
            hp = health.health;
            anim.SetBool("isHurt", true);
            if(hp<=0&&!dead)
            {
                anim.SetTrigger("isDead");
                dead = true;
            }
            anim.SetBool("isHurt", false);
        }
        
    }
}
