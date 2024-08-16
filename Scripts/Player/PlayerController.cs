using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : GenericSingleton<PlayerController>
{
    public event System.Action<Vector2> OnMoveEvent;
    public event System.Action OnJumpEvent;
    public event System.Action OnFallEvent;
    public event System.Action OnDashEvent;
    public event System.Action OnShootEvent;
    public event System.Action<int> OnAttackEvent;
    public event System.Action<bool> OnWallSlideEvent;
    public System.Action<PlayerController> OnHitEvent;
    public System.Action<PlayerController> OnDeathEvent;

    private System.Action<Vector2> originalOnMoveEvent;
    private System.Action originalOnJumpEvent;
    private System.Action originalOnFallEvent;
    private System.Action originalOnDashEvent;
    private System.Action originalOnShootEvent;
    private System.Action<int> originalOnAttackEvent;
    private System.Action<bool> originalOnWallSlideEvent;
    private System.Action<PlayerController> originalOnHitEvent;
    private System.Action<PlayerController> originalOnDeathEvent;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private ArrowSpawner arrowSpawner;

    public PlayerStats playerStats;
    public PlayerAnimation playerAnimation;
    public Animator animator;

    public bool isFacingRight { get; private set; } = true;

    [Header("Jump")]
    public LayerMask groundLayer;
    [SerializeField] private float jumpForce = 10f;
    public bool isGrounded;
    private bool isJumping;

    [Header("Dash")]
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private float dashSpeed = 60f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCoolTime = 1f;
    private float normalGravity;
    private float waitTime;
    private bool canDash = true;
    private bool isDashing;

    [Header("Wall")]
    public Transform wallChk;
    public float wallchkDistance;
    public LayerMask w_Layer;
    public bool isWall;
    public float slidingSpeed;
    public float wallJumpPower;
    public bool isWallJump;
    private bool canAttachToWall = true;

    [Header("Ground")]
    public Transform groundChkFront;
    public Transform groundChkBack;
    public float chkDistance;

    [Header("Attack")]
    [SerializeField] private Animator attackAnim;
    [SerializeField] private Vector2 attackOffset;
    [SerializeField] private Vector2 attackSize;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 0.5f;

    [Header("Health")]
    public float damageAmount = 1f;

    private bool canAttack = true;
    private int attackStep = 0;
    private float lastAttackTime;

    public InteractiveObject interactiveObject;

    [Header("Shoot")]
    public int arrowCount = 0;
    public int maxArrows = 10;
    public GameObject arrow;
    public Transform pos;
    private float isRight = 1;

    [Header("Deceleration")]
    [SerializeField] private float decelerationRate = 0.1f;

    private PlayerStatHandler playerStatHandler;

    private float originalSpeed;
    private bool isStopped = false;
    private bool canFlip = true;

    protected new void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        attackAnim = GetComponent<Animator>();
        arrowSpawner = GetComponent<ArrowSpawner>();
        normalGravity = rb.gravityScale;
        playerStatHandler = GetComponent<PlayerStatHandler>();

        if (playerStatHandler == null)
        {
            //Debug.LogError("PlayerStatHandler is not assigned in PlayerController.");
        }

        playerAnimation = GetComponent<PlayerAnimation>();

        originalOnMoveEvent = OnMoveEvent;
        originalOnJumpEvent = OnJumpEvent;
        originalOnFallEvent = OnFallEvent;
        originalOnDashEvent = OnDashEvent;
        originalOnShootEvent = OnShootEvent;
        originalOnAttackEvent = OnAttackEvent;
        originalOnWallSlideEvent = OnWallSlideEvent;
        originalOnHitEvent = OnHitEvent;
        originalOnDeathEvent = OnDeathEvent;
    }

    private void Start()
    {
        originalSpeed = playerStats.playerSpeed;
    }

    private void Update()
    {
        waitTime += Time.deltaTime;

        if (isDashing) return;

        CheckGround();
        CheckWall();
        HandleWallSlide();
        CheckDirection();

        ResetAttackStep();
        OnMoveEvent?.Invoke(moveInput);

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                isJumping = true;
                OnJumpEvent?.Invoke();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isStopped)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (isDashing) return;

        HandleMovement(Time.fixedDeltaTime);
        HandleJump();

        if (isWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * slidingSpeed);

            if (Input.GetButtonDown("Jump"))
            {
                WallJump();
            }
        }

        if (rb.velocity.y <= 0)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isGrounded", false);

            //Debug.DrawRay(rb.position, Vector2.down, new Color(1, 0, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector2.down, 1f, LayerMask.GetMask("groundLayer", "JumpPad"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.56f)
                {
                    animator.SetBool("isGrounded", true);
                    animator.SetBool("isFalling", false);
                }
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        isRight = moveInput.x > 0 ? 1 : (moveInput.x < 0 ? -1 : isRight);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                isJumping = true;
                OnJumpEvent?.Invoke();
                animator.SetBool("isJumping", true);
                animator.SetBool("isGrounded", false);
            }
            else if (isWall && canAttachToWall)
            {
                WallJump();
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && interactiveObject != null)
        {
            interactiveObject.Interaction();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            if (waitTime >= dashCoolTime)
            {
                waitTime = 0;
                Dash();
                OnDashEvent?.Invoke();
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack)
        {
            Attack();
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && arrowSpawner != null)
        {
            OnShootEvent?.Invoke();
            animator.SetTrigger("Shoot");
            Instantiate(arrow, pos.position, transform.rotation);
        }
    }

    private void CheckGround()
    {
        bool ground_front = Physics2D.Raycast(groundChkFront.position, Vector2.down, chkDistance, groundLayer);
        bool ground_back = Physics2D.Raycast(groundChkBack.position, Vector2.down, chkDistance, groundLayer);

        if (!isGrounded && (ground_front || ground_back))
            rb.velocity = new Vector2(rb.velocity.x, 0);

        isGrounded = ground_front || ground_back;
    }

    private void CheckWall()
    {
        if (!canAttachToWall) return;

        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallchkDistance, w_Layer);

        if (isWall)
        {
            FlipToOppositeWall();
        }
    }

    private void HandleMovement(float deltaTime)
    {
        if (!isWallJump)
            rb.velocity = new Vector2((moveInput.x) * playerStats.playerSpeed, rb.velocity.y);
    }

    private void HandleJump()
    {
        if (isJumping && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = false;
        }
    }

    private void HandleWallSlide()
    {
        if (isWall && !isGrounded && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -slidingSpeed);
            animator.SetBool("isFalling", false);
            animator.SetBool("isWallSliding", true);
        }
        else
            animator.SetBool("isWallSliding", false);
    }

    private void WallJump()
    {
        isWallJump = true;
        rb.velocity = new Vector2(-isRight * wallJumpPower, 0.9f * wallJumpPower);

        if ((isFacingRight && isRight == 1) || (!isFacingRight && isRight == -1))
        {
            Flip();
        }

        isWall = false;
        canAttachToWall = false;
        Invoke(nameof(ResetWallAttach), 0.3f);
        Invoke(nameof(ResetWallJump), 0.3f);
    }

    private void ResetWallAttach()
    {
        canAttachToWall = true;
    }

    private void ResetWallJump()
    {
        isWallJump = false;
    }

    private void Dash()
    {
        StartCoroutine(PerformDash());
    }

    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;
        tr.emitting = true;
        rb.gravityScale = 0;
        rb.velocity = new Vector2((isFacingRight ? 1 : -1) * dashSpeed, 0);
        animator.SetBool("isDash", true);

        yield return new WaitForSeconds(dashTime);

        tr.emitting = false;
        rb.gravityScale = normalGravity;
        isDashing = false;
        animator.SetBool("isDash", false);

        yield return new WaitForSeconds(dashCoolTime);
        canDash = true;
    }

    private void Attack()
    {
        OnAttackEvent?.Invoke(attackStep);
        Vector2 attackPosition = (Vector2)transform.position + attackOffset * (isFacingRight ? 1 : -1);
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            IDamagable damagable = enemy.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(attackDamage);
            }
        }

        attackStep = (attackStep + 1) % 3;
        lastAttackTime = Time.time;
    }

    private void CheckDirection()
    {
        if (moveInput.x > 0 && !isFacingRight)
            Flip();
        else if (moveInput.x < 0 && isFacingRight)
            Flip();
    }

    private void Flip()
    {
        if (!canFlip) return;

        isFacingRight = !isFacingRight;
        transform.Rotate(new Vector3(0, 180, 0));
    }

    private void FlipToOppositeWall()
    {
        if ((isFacingRight && isRight == 1) || (!isFacingRight && isRight == -1))
        {
            Flip();
        }
    }

    private void ResetAttackStep()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            attackStep = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 attackPosition = (Vector2)transform.position + attackOffset * (isFacingRight ? 1 : -1);
        Gizmos.DrawWireCube(attackPosition, attackSize);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(wallChk.position, Vector2.right * isRight * wallchkDistance);
    }

    public void AddArrows(int count)
    {
        arrowCount = Mathf.Min(arrowCount + count, maxArrows);
        //Debug.Log("Arrow count: " + arrowCount);
    }

    public bool UseArrow()
    {
        if (arrowCount > 0)
        {
            arrowCount--;
            return true;
        }
        else
        {
            //Debug.Log("No arrows left.");
            return false;
        }
    }

    public void OnHit()
    {
        OnHitEvent?.Invoke(this);
        if (playerStatHandler != null)
        {
            playerStatHandler.TakeDamage(damageAmount);
        }
        else
        {
            //Debug.LogError("PlayerStatHandler is not assigned in OnHit.");
        }
    }

    public void OnDeath()
    {
        OnDeathEvent?.Invoke(this);
        GameManager.Instance.GameOver();
    }

    private void OnDestroy()
    {
        OnMoveEvent = null;
        OnJumpEvent = null;
        OnFallEvent = null;
        OnDashEvent = null;
        OnShootEvent = null;
        OnAttackEvent = null;
        OnWallSlideEvent = null;
    }

    public void StopPlayer()
    {
        isStopped = true;
        canFlip = false;
        OnMoveEvent = null;
        OnFallEvent = null;
        OnJumpEvent = null;
        OnDashEvent = null;
        OnShootEvent = null;
        OnAttackEvent = null;
        OnWallSlideEvent = null;
        if (playerAnimation != null)
        {
            playerAnimation.PlayIdle();
        }
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }

    public void ResumePlayer()
    {
        isStopped = false;
        canFlip = true;
        OnMoveEvent = originalOnMoveEvent;
        OnJumpEvent = originalOnJumpEvent;
        OnFallEvent = originalOnFallEvent;
        OnDashEvent = originalOnDashEvent;
        OnShootEvent = originalOnShootEvent;
        OnAttackEvent = originalOnAttackEvent;
        OnWallSlideEvent = originalOnWallSlideEvent;
        OnShootEvent = originalOnShootEvent;
        rb.isKinematic = false;
    }

    public void DeceleratePlayer()
    {
        StartCoroutine(Decelerate());
    }

    private IEnumerator Decelerate()
    {
        while (rb.velocity.magnitude > 0.01f)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, decelerationRate * Time.deltaTime);
            yield return null;
        }
        rb.velocity = Vector2.zero;
    }

    public void Stop()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Resume()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public bool IsGround()
    {
        return isGrounded;
    }
}