using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed=2f; 
    [SerializeField]LayerMask playerLayerMask;
    [SerializeField]BoxCollider2D enemyCollider;
    Rigidbody2D rigid;

    RaycastHit2D proximity;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
    }

    private bool DetectionRange()
    {
        proximity = Physics2D.Raycast(enemyCollider.bounds.center, transform.right, 15f, playerLayerMask);
        return proximity.collider != null;
    }

    private void Move()
    {
        rigid.velocity = new Vector2(moveSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(enemyCollider.bounds.center, transform.right*15f);
    }
}
