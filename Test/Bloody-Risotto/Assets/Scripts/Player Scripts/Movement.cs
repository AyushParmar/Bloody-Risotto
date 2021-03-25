using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] LayerMask platformLayerMask;
    [SerializeField] LayerMask ladderLayerMask;
    [SerializeField] LayerMask waterLayerMask;
    [SerializeField] Transform wallGrabPoint;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 28f;
    [SerializeField] float wallJumpSpeed = 8f;
    [SerializeField] float ladderSpeed = 2f;
    [SerializeField] Vector2 wallCheckSize;

    Rigidbody2D rigid;
    Animator anim;
    CapsuleCollider2D playerCollider;

    public static bool isAlive;

    bool canGrab;
    bool isGrabbing;
    bool lookingRight = true;
    bool canJump = true;
    bool canDoubleJump = false;
    bool launchedFromWall = false;
    bool climbingLadders = false;
    public float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    float gravityScaleAtStart;
    float maxFallSpeed = 35f;

    RaycastHit2D inWaterRaycast;
    RaycastHit2D onGroundRaycast;

    Vector2 minDistance;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = rigid.gravityScale;
        minDistance = new Vector2(playerCollider.bounds.size.x - 0.1f, playerCollider.bounds.size.y);
    }

    void Update()
    {
        if (WrestleControl())
        {
            Move();
            Jump();
            StartCoroutine(DblJump());
            ClimbLadders();
            WallJump();
        }
        Flip();
        Debug.Log(isAlive);
    }

    private void FixedUpdate()
    {
        if(rigid.velocity.magnitude>maxFallSpeed)
        {
            rigid.velocity = Vector2.ClampMagnitude(rigid.velocity, maxFallSpeed);
        }
    }



    private void Move()
    {
        float controlThrow;
        if(OnGroundOrWater())
        {
            controlThrow = Input.GetAxis("Horizontal");
        }
        else
        {
            controlThrow = Input.GetAxisRaw("Horizontal");
        }
        Vector2 playerVelocity = new Vector2(controlThrow * moveSpeed, rigid.velocity.y);
        rigid.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rigid.velocity.x) > Mathf.Epsilon;

        if (OnGroundOrWater())
        {
            anim.SetBool("isRunning", playerHasHorizontalSpeed);
            anim.SetFloat("runningSpeed", Mathf.Abs(rigid.velocity.x/7));
        }


    }

    void Flip()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigid.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            if (rigid.velocity.x >= 0 && !lookingRight)
            {
                lookingRight = !lookingRight;
                transform.Rotate(0, 180, 0);
            }
            if (rigid.velocity.x < 0 && lookingRight)
            {
                lookingRight = !lookingRight;
                transform.Rotate(0, 180, 0);
            }
        }
    }

    private bool OnGroundOrWater()
    {
        onGroundRaycast = Physics2D.BoxCast(playerCollider.bounds.center,
        minDistance, 0f, Vector2.down, 0.1f, platformLayerMask);

        inWaterRaycast = Physics2D.BoxCast(playerCollider.bounds.center,
        minDistance, 0f, Vector2.down, 0.1f, waterLayerMask);

        if ((onGroundRaycast.collider != null)|| (inWaterRaycast.collider != null))
        {
            canJump = true;
        }

        return (onGroundRaycast.collider != null)||(inWaterRaycast.collider != null);

    }

    private bool MinDistanceForWallJump()
    {
        RaycastHit2D minDistanceCheck = Physics2D.BoxCast(playerCollider.bounds.center,
            minDistance, 0f, Vector2.down, 2f, platformLayerMask);

        return minDistanceCheck.collider == null;

    }

    private bool WrestleControl()
    {
        wallJumpCounter -= Time.deltaTime;
        if (wallJumpCounter <= 0)
            return true;
        else
        {
            return false;
        }
    }


    private IEnumerator DblJump()
    {
        if (Input.GetButtonDown("Jump") && !OnGroundOrWater() && canDoubleJump)
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isDoubleJumping", true);
            Vector2 jumpVelocity = new Vector2(rigid.velocity.x, jumpSpeed);
            rigid.velocity = jumpVelocity;
            canDoubleJump = false;
            yield return new WaitForSeconds(0.417f);
            anim.SetBool("isDoubleJumping", false);
            anim.SetBool("isRunning", true);
        }
    }

    private void ClimbLadders()
    {
        if (!playerCollider.IsTouchingLayers(ladderLayerMask))
        {
            rigid.gravityScale = gravityScaleAtStart;
            climbingLadders = false;

            anim.SetBool("isClimbing", false);
            return;
        }

        float onLadderAxis = Input.GetAxisRaw("Vertical");
        Vector2 onLadderPlayerVelocity = new Vector2(rigid.velocity.x, onLadderAxis * ladderSpeed);
        rigid.velocity = onLadderPlayerVelocity;

        rigid.gravityScale = 0f;
        climbingLadders = true;
        bool playerHasVerticalSpeedOnLadder = Mathf.Abs(rigid.velocity.y) > Mathf.Epsilon;

        anim.SetBool("isClimbing", playerHasVerticalSpeedOnLadder);
        anim.SetBool("isRunning", false);
        canJump = true;
        canDoubleJump = true;



    }

    private void Jump()
    {

        if ((Input.GetButtonDown("Jump") && OnGroundOrWater() && canJump) || (Input.GetButton("Jump") && climbingLadders))
        {
            FindObjectOfType<AudioManager>().Play("Jump");
            Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
            rigid.velocity = jumpVelocity;
            canDoubleJump = true;
        }
    }

    private void WallJump()
    {
        isGrabbing = false;
        canGrab = Physics2D.OverlapBox(wallGrabPoint.position, wallCheckSize, 0f, platformLayerMask);

        if (canGrab && !OnGroundOrWater()&&MinDistanceForWallJump())
        {
            if ((lookingRight ? Input.GetAxisRaw("Horizontal") > 0 : Input.GetAxisRaw("Horizontal") < 0) || launchedFromWall)
            {
                rigid.gravityScale = 0f;
                rigid.velocity = Vector2.zero;
                isGrabbing = true;

                if (lookingRight ? Input.GetAxisRaw("Horizontal") < 0 : Input.GetAxisRaw("Horizontal") > 0)
                {
                    isGrabbing = false;
                    launchedFromWall = false;
                    anim.SetBool("isGrabbing", false);
                    return;
                }

                else if (Input.GetButtonDown("Jump"))
                {
                    wallJumpCounter = wallJumpTime;
                    rigid.AddForce(new Vector2(lookingRight ? -wallJumpSpeed : wallJumpSpeed, jumpSpeed / 2),ForceMode2D.Impulse);
                    isGrabbing = false;
                    launchedFromWall = true;
                    canDoubleJump = true;
                }

            }
            anim.SetBool("isDoubleJumping", false);
            anim.SetBool("isRunning", !isGrabbing);
            anim.SetBool("isGrabbing", isGrabbing);
            if (OnGroundOrWater())
            {
                launchedFromWall = false;
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(wallGrabPoint.position, wallCheckSize);
    }
}

