using UnityEngine;

public class EnemyStatHandler : MonoBehaviour, IDamagable
{
    private EnemyController enemyController;
    private EnemyStats enemyStats;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyStats = enemyController.enemyStats;
    }

    public void TakeDamage(float amount)
    {
        // 피격 모션 중에는 return 하여 무적시간
        if (enemyController.enemyAnimationController.isHit == true)
        {
            return;
        }

        // 무적시간이 아닐 경우
        enemyStats.maxHP -= amount;
        enemyController.OnHit();

        // 뒤로 밀려나는 로직 추가
        if (enemyController.ClosestTarget != null)
        {
            // 플레이어와의 방향 계산
            Vector2 knockbackDirection = (transform.position - enemyController.ClosestTarget.position).normalized;

            // 기존 속도와 상관없이 뒤로 밀려나는 힘을 가하기 위해 속도 초기화
            enemyController.rb.velocity = Vector2.zero;

            // 임펄스를 가하여 밀려나게 함
            enemyController.rb.AddForce(enemyStats.knockbackDistance * knockbackDirection, ForceMode2D.Impulse);
        }

        // 공격 중일 때 공격을 취소
        if (enemyController.isAttack)
        {
            CancelAttack();
        }


        if (enemyStats.maxHP <= 0)
        {
            enemyController.OnDeath();
        }
    }

    private void CancelAttack()
    {
        enemyController.isAttack = false;
        enemyController.enemyAnimationController.ResetAttackAnimation();
    }
}