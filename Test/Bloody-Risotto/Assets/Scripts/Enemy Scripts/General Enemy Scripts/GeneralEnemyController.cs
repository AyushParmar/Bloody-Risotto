using System.Collections;
using UnityEngine;

public class GeneralEnemyController : MonoBehaviour
{
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] BoxCollider2D enemyCollider;
    [SerializeField] Animator anim;

    [SerializeField] int hp;
    [SerializeField] float fireRate = 3f;
    [SerializeField] float fireOffset = 0.4f;
    [SerializeField] float stationaryTime = 7f;
    [SerializeField] bool lookingRight;

    public static bool staticCheck;

    public GameObject bulletPrefab;
    public GameObject exclamation;
    public Transform firePoint;
    //public Renderer rend;

    public static bool groupAlert;

    bool isAlive;
    bool isAlert = false;
    bool showAlert = true;
    bool canShoot = true;

    HealthManagement health;
    Transform playerTransform;
    RaycastHit2D proximity;

    void Start()
    {
        //rend = GetComponent<Renderer>();
        lookingRight = gameObject.transform.rotation.y == 0 ? true : false;
        InvokeRepeating("Flip", 0f, stationaryTime);

        health = GetComponent<HealthManagement>();
        hp = health.health; 

        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        isAlive = !health.dieCalled;
        if (isAlive)
        {
            Alert();
            CheckHit();
            LookAtAfterAlert();
            if(canShoot)
            {
                StartCoroutine(Shoot());
            }
            Debug.Log(isAlive);
            Debug.Log(gameObject.name + staticCheck);
        }
    }

    private void CheckHit()
    {
        anim.SetBool("isHurt", false);
        if(health.health<hp)
        {
            hp = health.health;
            anim.SetBool("isHurt",true);
        }
    }

    private void Flip()
    {
        if (!isAlert)
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
            else if (playerTransform.position.x < gameObject.transform.position.x && lookingRight)
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
        if (DetectionRange() && !isAlert)
        {
            isAlert = true;
            if (showAlert)
            {
                Instantiate(exclamation, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.5f),
                    Quaternion.identity);
                showAlert = false;
            }
        }

    }

    IEnumerator Shoot()
    {
        if(!isAlert)
        {
            yield return null;
        }
        else if(isAlert)
        {
            canShoot = false;
            yield return new WaitForSeconds(fireRate);
            anim.SetTrigger("isShooting");
            yield return new WaitForSeconds(fireOffset);
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            canShoot = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(enemyCollider.bounds.center, transform.right * 12.5f);
    }
}