using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralBullet : MonoBehaviour
{
    public Rigidbody2D rigid;
    public GameObject hitEffect;
    public int damage = 25;

    [SerializeField] float fireSpeed = 15f;
    //[SerializeField] float fireVariation;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(transform.right.x, 0) * fireSpeed;
        Destroy(gameObject, 10f);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision)
        {
            HealthManagement enemy = collision.GetComponent<HealthManagement>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
        GameObject newHitEffect = Instantiate(hitEffect, rigid.position, Quaternion.identity);
        Destroy(newHitEffect, 0.4f);
    }
}
