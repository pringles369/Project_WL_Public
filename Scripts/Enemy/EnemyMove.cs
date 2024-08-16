using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private EnemyController enemyController;
    private SpriteRenderer spriteRenderer;

    private float moveSpeed;
    private Vector2 randomDirection;
    private Vector2 direction;
    private float randomMoveDuration = 1.5f;
    private float randomMoveTimer;
    private float idleDuration = 1.5f;
    private float idleTimer;
    private bool isIdle = false;
    private bool isStopped = false;
    private float customTimeScale = 1f;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        rb = GetComponentInChildren<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        moveSpeed = enemyController.enemyStats.moveSpeed;
        randomMoveTimer = randomMoveDuration;
    }

    private void FixedUpdate()
    {
        if(isStopped) return;

        float deltaTime = Time.fixedDeltaTime * customTimeScale;

        if (enemyController.enemyStats.maxHP > 0)
        {
            if (enemyController.isAttack == true || enemyController.enemyAnimationController.isHit == true)
            {
                StopMove();
            }
            else if (isIdle)
            {
                Idle(deltaTime);
            }
            else
            {
                MoveOrIdle(deltaTime);
            }

        }
        else
        {
            StopMove();
        }

    }

    private void MoveOrIdle(float deltaTime)
    {
        if (enemyController.ClosestTarget != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, enemyController.ClosestTarget.position);
            if (distanceToTarget <= enemyController.detectionRange)
            {
                if (distanceToTarget > GetAttackRange())
                {
                    direction = (enemyController.ClosestTarget.position - transform.position).normalized;
                    direction.y = 0; // 좌우로만 움직이도록 y축 방향 제거
                }
                else
                {
                    direction = Vector2.zero;
                }
            }
            else
            {
                enemyController.SetClosestTarget(null); // 타겟을 잃음
            }
        }

        if (enemyController.ClosestTarget == null)
        {
            if (randomMoveTimer <= 0)
            {
                float randomValue = Random.value;
                if (randomValue < 0.3f) // 30% 확률로 왼쪽으로 이동
                {
                    direction = Vector2.left;
                }
                else if (randomValue < 0.7f) // 40% 확률로 가만히 있기
                {
                    direction = Vector2.zero;
                    isIdle = true;
                }
                else // 30% 확률로 오른쪽으로 이동
                {
                    direction = Vector2.right;
                }
                randomMoveTimer = randomMoveDuration;
            }
            else
            {
                randomMoveTimer -= deltaTime;
            }
        }

        if (direction != Vector2.zero)
        {
            Move(direction);
        }
    }

    private float GetAttackRange()
    {
        float attackRange = 0;
        foreach (var attackSO in enemyController.enemyStats.enemyAttackSO)
        {
            if (attackSO.attackRange > attackRange)
            {
                attackRange = attackSO.attackRange;
            }
        }
        return attackRange;
    }

    private void Move(Vector2 direction)
    {
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        // 좌우 방향에 따라 스프라이트의 flipX 설정 >> Animation 설정으로 flipX 대신 Quaternion으로 변경
        if (direction.x < 0)
        {
            transform.rotation = new Quaternion(0, 1, 0, 0);
        }
        else if (direction.x > 0)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        // 이동 애니메이션 트리거
        enemyController.OnMove(direction);
    }

    private void Idle(float deltaTime)
    {
        StopMove();

        if (idleTimer <= 0)
        {
            isIdle = false;
            idleTimer = idleDuration;
            randomMoveTimer = randomMoveDuration;
        }
        else
        {
            idleTimer -= Time.fixedDeltaTime;
        }
    }

    private void StopMove()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = 0;
        enemyController.OnMove(velocity);
    }

    public void StopEnemyMovement() //적 멈춤
    {
        isStopped= true;
        customTimeScale = 0f;
        StopMove();
    }

    public void ResumeEnemyMovement() //적 움직임
    {
        isStopped = false;
        customTimeScale = 1f;
    }
}