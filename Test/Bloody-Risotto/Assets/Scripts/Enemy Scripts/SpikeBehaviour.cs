using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehaviour : MonoBehaviour
{
    HealthManagement damageHealth;
    [SerializeField]Vector2 repulse;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision)
        {
            damageHealth = collision.GetComponent<HealthManagement>();
            if(damageHealth!=null)
            {
                damageHealth.TakeDamage(35);
                collision.GetComponent<Rigidbody2D>().AddForce(repulse,ForceMode2D.Impulse);
            }    
        }
    }

    // Update is called once per frame
    void Update()
    { 
        
    }
}
