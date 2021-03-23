using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralEnemyController : MonoBehaviour
{
    [SerializeField]LayerMask playerLayerMask;
    [SerializeField]BoxCollider2D enemyCollider;
    [SerializeField] Animator anim;
    [SerializeField] Transform playerPosition;
    [SerializeField] float flipAfter;
    [SerializeField] float moveSpeed=2f; 
    [SerializeField] float pause;

    public GameObject bulletPrefab;
    public GameObject exclamation;
    public Transform firePoint;

    bool isAlert = false;
    bool shouldFlip = true;
    bool isShooting;
    float stationaryTime=7f;
    float waitBeforeShooting = 2f;

    Rigidbody2D rigid;

    RaycastHit2D proximity;

    void Start()
    {
        flipAfter = stationaryTime;
        rigid = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        flipAfter -= Time.deltaTime;
        pause -= Time.deltaTime;
        if(flipAfter<=0&&shouldFlip)
        {
            Flip();
        }
        Alert();    
        if(pause<=0)
        {
            Shoot();
            
        }
    }
    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        flipAfter = stationaryTime;
    }

   

    private bool DetectionRange()
    {
        proximity = Physics2D.Raycast(enemyCollider.bounds.center, transform.right, 12.5f, playerLayerMask);
        return proximity.collider != null;
    }


    private void Alert()
    {
        if(DetectionRange())
        {
            Debug.Log("Player Detected");
            isAlert = true;
            shouldFlip = false;
        }
    }

    public void Shoot()
    {
        if(isAlert)
        {
            Instantiate(bulletPrefab, firePoint.position,firePoint.rotation);
            pause = waitBeforeShooting;
        }
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(enemyCollider.bounds.center, transform.right*12.5f);
    }
}
