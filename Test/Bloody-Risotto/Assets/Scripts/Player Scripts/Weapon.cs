using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon: MonoBehaviour
{
    public Transform firePoint;
    public int Damage=5;
    private float fireTime=0.2f;
    public GameObject bulletPrefab;
    public GameObject hitEffect;

    [SerializeField] float fireRate;
   
    void Update()
    {
        fireRate -= Time.deltaTime;
        Shoot();
    }

    void Shoot()
    {
        if(fireRate<=0)
        {
            if (Input.GetButton("Fire1"))
            {
                FindObjectOfType<AudioManager>().Play("Shooting");
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                fireRate = fireTime;
            }
        }

    }
}
