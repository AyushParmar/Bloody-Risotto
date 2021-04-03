using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] BoxCollider2D enemyCollider;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] float BulletFireRate= 0.3f;
    private RaycastHit2D proximity;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public int playerDamage;
    private bool isAlert;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if(isAlert)
        {
            StartCoroutine(EnemyShoot());
        }
    }

    private bool DetectionRange()
    {
        proximity = Physics2D.Raycast(enemyCollider.bounds.center, transform.right, 7.5f, playerLayerMask);
        if(proximity.collider != null)
        Debug.Log("IS ALERTED");
        return proximity.collider != null;
    }
    public IEnumerator EnemyShoot()
    {
        if(DetectionRange())
        {
            isAlert = true;
        }
        if(isAlert)
        {
            for(int i=0;i<=2; i++)
            {
                Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
                Debug.Log("BULLET CREATED");
                yield return new WaitForSeconds(BulletFireRate);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(firePoint.transform.position, firePoint.transform.right*5f);
    }
}
