using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestWitchController : EnemyController
{
    public AttackSO appleAttack;
    public AttackSO thornAttack;
    public AttackSO dropAttack;

    private Animator animator;
    private Coroutine attackCoroutine;
    private Coroutine patternCoroutine;
    public GameObject player; // 플레이어 오브젝트를 저장할 변수
    
    private bool isPlayerInRange = false;
    public bool isMove = false;
    public bool isHit = false;
    public bool isDead = false;

    protected override void Start()
    {
        //SoundManager.Instance.PlayBGM("Mountain Town");

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // BossStatHandler 사용
        gameObject.AddComponent<BossStatHandler>();

        // 플레이어 오브젝트 찾기
        player = GameObject.FindGameObjectWithTag("Player");

        // 플레이어 감지 시작
        StartCoroutine(CheckForPlayer());

        // 공격 코루틴 시작
        StartCoroutine(AttackCycleRoutine());
    }

    private void FixedUpdate()
    {
        // 플레이어가 범위 밖에 있을 때 플레이어 추적
        if (!isAttack && !isPlayerInRange)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Vector2 targetPosition = rb.position + new Vector2(direction.x, direction.y) * enemyStats.moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);

            // 이동 애니메이션 재생
            isMove = true;
            animator.SetBool("isMove", isMove);
        }
        else
        {
            // 이동 애니메이션 중지
            isMove = false;
            animator.SetBool("isMove", isMove);
        }
    }

    private IEnumerator CheckForPlayer()
    {
        while (enemyStats.maxHP > 0)
        {
            if (player != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
                if (distanceToPlayer <= detectionRange)
                {
                    isPlayerInRange = true;
                }
                else
                {
                    isPlayerInRange = false;
                }
            }

            yield return new WaitForSeconds(0.5f); // 플레이어 감지 주기
        }
    }

    private IEnumerator AttackCycleRoutine()
    {
        while (enemyStats.maxHP > 0)
        {
            // appleAttack 3번 반복 실행 (1.5초마다)
            for (int i = 0; i < 3; i++)
            {
                ExecuteAttack(appleAttack);
                yield return new WaitForSeconds(3f);
            }

            // thornAttack 또는 dropAttack을 랜덤으로 실행
            AttackSO selectedAttack = Random.value > 0.5f ? thornAttack : dropAttack;
            ExecuteAttack(selectedAttack);

            // 다음 사이클 전 대기 시간
            yield return new WaitForSeconds(4f); // 4초 대기 후 다음 사이클
        }
    }


    private void ExecuteAttack(AttackSO attack)
    {
        if (attack != null && enemyStats.maxHP > 0 && !isAttack)
        {
            // 공격 애니메이션 트리거 설정 및 공격 실행
            isAttack = true;
            switch (attack.name)
            {
                case "AppleAttack":
                    animator.SetTrigger("AppleAttack");
                    break;
                case "DropAttack":
                    animator.SetTrigger("DropAttack");
                    break;
                case "ThornAttack":
                    animator.SetTrigger("ThornAttack");
                    break;
            }
        }
    }

    public void OnHit()
    {
        animator.SetBool("isHit", isHit);
    }

    public override void OnDeath()
    {
        isDead = true;

        // 모든 애니메이션 트리거와 bool 값을 초기화하여 애니메이션을 멈춤
        animator.ResetTrigger("AppleAttack");
        animator.ResetTrigger("DropAttack");
        animator.ResetTrigger("ThornAttack");
        StopAllCoroutines();

        animator.SetBool("isMove", false);
        animator.SetBool("isHit", false);
        animator.SetBool("isDead", isDead);
    }

    public void OnDeathAnimationEnd()
    {
        Invoke("EndScene", 2f);
        Destroy(this.gameObject, 2f);
    }

    private void EndScene()
    {
        SceneManager.LoadScene("EndScene");
    }

    public void SpawnThornTraps()
    {
        if (thornAttack != null)
        {
            thornAttack.Attack(this.gameObject);
        }
        isAttack = false; // 공격이 끝난 후 이동 재개
    }

    public void ThrowApple()
    {
        if (appleAttack != null)
        {
            appleAttack.Attack(this.gameObject);
        }
        isAttack = false;
    }

    public void DropTree()
    {
        if (dropAttack != null)
        {
            dropAttack.Attack(this.gameObject);
        }
        isAttack = false; // 공격이 끝난 후 이동 재개
    }

    public void OnAnimationEnd()
    {
        isAttack = false;
    }

    private void OnDisable()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        if (patternCoroutine != null)
        {
            StopCoroutine(patternCoroutine);
            patternCoroutine = null;
        }
    }

    // 감지 범위를 씬에 그리기 위한 Gizmo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}