using System;
using System.Collections;
using UnityEngine;

// Enemy는 기본적으로 해당 스크립트를 상속받는다.

public class EnemyController : MonoBehaviour
{
    public Action<Vector2> OnMoveEvent;
    public Action<AttackSO> OnAttackEvent;
    public Action<EnemyController> OnDeathEvent;
    public Action<EnemyController> OnHitEvent;

    public Transform ClosestTarget { get; private set; }

    public EnemyAnimationController enemyAnimationController;
    public EnemyStats enemyStats; // EnemyStats를 통해 공격 배열을 가져옴

    public LayerMask playerLayer;
    public float detectionRange = 10f; // 기본 플레이어 감지 범위
    public float detectingTime = 0.5f; // 기본 플레이어 감지 주기
    public Coroutine checkForPlayerCoroutine; // 플레이어 체크 코루틴

    private EnemyMeleeAttack enemyMeleeAttack;
    private EnemyRangeAttack enemyRangeAttack;

    public bool attacked = false; // 공격한 뒤 딜레이 계산을 위한 bool값
    public bool isAttack = false; // 공격 중임을 판단하는 bool값

    public Rigidbody2D rb;

    protected virtual void Start()
    {
        checkForPlayerCoroutine = StartCoroutine(CheckForPlayer());

        enemyAnimationController = GetComponent<EnemyAnimationController>();
        enemyMeleeAttack = GetComponent<EnemyMeleeAttack>();
        enemyRangeAttack = GetComponent<EnemyRangeAttack>();
        rb = GetComponent<Rigidbody2D>();
    }

    private IEnumerator CheckForPlayer()
    {
        while (enemyStats.maxHP > 0)
        {
            // OverlapCircleAll : 원형 범위 내에 있는 모든 Collider2D 검색하고 배열로 반환
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange, playerLayer);

            if (hits.Length > 0) // playerLayer가 하나라도 hit되면
            {
                ClosestTarget = hits[0].transform;

                // 플레이어가 감지되면 공격 타입에 따라 공격 실행
                if (enemyMeleeAttack != null)
                {
                    enemyMeleeAttack.MeleeAttack();
                    attacked = true;
                }

                if (enemyRangeAttack != null)
                {
                    enemyRangeAttack.RangeAttack();
                    attacked = true;
                }
            }
            else
            {
                SetClosestTarget(null);
            }

            if (attacked)
            {
                // 공격 타입별로 가장 긴 attackDelay를 기다림
                float maxDelay = 0f;
                foreach (var attackSO in enemyStats.enemyAttackSO)
                {
                    if (attackSO.attackDelay > maxDelay)
                    {
                        maxDelay = attackSO.attackDelay;
                    }
                }
                yield return new WaitForSeconds(maxDelay);
                isAttack = false;
                attacked = false;
            }
            else
            {
                yield return new WaitForSeconds(detectingTime);
            }
        }
    }

    // ClosestTarget이 private set이므로 외부에서 값을 바꾸려면 해당 함수 이용
    public void SetClosestTarget(Transform target)
    {
        ClosestTarget = target;
    }

    public void OnMove(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void OnAttack(AttackSO attack)
    {
        OnAttackEvent?.Invoke(attack);
    }

    public void OnHit()
    {
        OnHitEvent?.Invoke(this);
    }

    public virtual void OnDeath()
    {
        if (checkForPlayerCoroutine != null)
        {
            StopCoroutine(checkForPlayerCoroutine);
        }

        OnDeathEvent?.Invoke(this);
    }
}