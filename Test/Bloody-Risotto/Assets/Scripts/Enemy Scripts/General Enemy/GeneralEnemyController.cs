using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralEnemyController : MonoBehaviour
{
    [SerializeField]LayerMask playerLayerMask;
    [SerializeField]BoxCollider2D enemyCollider;
    [SerializeField] Animator anim;
    [SerializeField] Transform playerTransform;
    
    [SerializeField] float flipAfter;
    [SerializeField] float pause;

    [SerializeField] float fireRate = 3f;
    [SerializeField] bool lookingRight;

    public GameObject bulletPrefab;
    public GameObject exclamation;
    public Transform firePoint;
    public Renderer rend;

    public static bool isAlive;
    public static bool groupAlert;

    bool isAlert = false;
    bool isShooting;
    bool showAlert=true;
    float stationaryTime=7f;
    float shootingTime;

    Rigidbody2D rigid;

    RaycastHit2D proximity;

    void Start()
    {
        rend = GetComponent<Renderer>();
        lookingRight = gameObject.transform.rotation.y == 0 ? true : false;
        InvokeRepeating("Flip", 0f, stationaryTime);
    }
    void Update()
    {
        Alert();
        LookAtAfterAlert();
        //isVisible();
        Shoot();
        Debug.Log(isAlive);
    }

    private void isVisible()
    {
        if (rend.isVisible)
        {
            Debug.Log("hey visible");
        }
        else
            Debug.Log("not visible");
    }

    private void Flip()
    {
        if(!isAlert)
        {
            transform.Rotate(0, 180f, 0);
            lookingRight = !lookingRight;
        }
    }

    private void LookAtAfterAlert()
    {
        if (isAlert)
        {
            if (playerTransform.position.x > gameObject.transform.position.x && !lookingRight)
            {
                transform.Rotate(0, 180f, 0);
                lookingRight = !lookingRight;
            }
            else if (playerTransform.position.x < gameObject.transform.position.x  && lookingRight)
            {
                transform.Rotate(0, 180f, 0);
                lookingRight = !lookingRight;
            }
        }
    }

    private bool DetectionRange()
    {
        proximity = Physics2D.Raycast(enemyCollider.bounds.center, transform.right, 12.5f, playerLayerMask);
        return proximity.collider != null;
    }



    private void Alert()
    {
        if(DetectionRange()&&!isAlert)
        {
            isAlert = true;
            if(showAlert)
            {
                Instantiate(exclamation, new Vector2(gameObject.transform.position.x,gameObject.transform.position.y+1.5f),
                    Quaternion.identity);
                showAlert = false;
            }
        }
       
    }

    public void Shoot()
    {
        if(!isAlert)
        {

        }
        if(isAlert)
        {
            anim.SetBool("isShooting", false);
            anim.SetBool("isIdling", true);
            if (Time.time>shootingTime)
            {
                shootingTime = Time.time+fireRate;
                anim.SetBool("isIdling", false);
                anim.SetBool("isShooting", true);
                Instantiate(bulletPrefab, firePoint.position,firePoint.rotation);
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(enemyCollider.bounds.center, transform.right*12.5f);
    }
}
