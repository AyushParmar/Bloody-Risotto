using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projection : MonoBehaviour
{
    public float fireSpeed = 15f;
    public Rigidbody2D rigid;
    public GameObject hitEffect;
    public int damage = 5;

    [SerializeField] float fireVariation;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(transform.right.x, Random.Range(-fireVariation, fireVariation))*fireSpeed;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision)
        {
            HealthManagement enemy = collision.GetComponent<HealthManagement>();
            if(enemy!=null)
            {
                enemy.TakeDamage(5);
            }
        }
        Destroy(gameObject);
        GameObject newHitEffect = Instantiate(hitEffect, rigid.position, Quaternion.identity);
        Destroy(newHitEffect, 0.4f);
    }
}
