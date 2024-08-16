using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private EnemyController enemyController;

    public bool isHit;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();

        // 애니메이션 이벤트 등록
        enemyController.OnMoveEvent += MoveAnimation;
        enemyController.OnAttackEvent += AttackAnimation;
        enemyController.OnHitEvent += HitAnimation;
        enemyController.OnDeathEvent += DeathAnimation;
    }

    private void MoveAnimation(Vector2 direction)
    {
        bool isMove = direction.x != 0; // direction.x가 0이아니면 true
        animator.SetBool("isMove", isMove);
    }

    private void AttackAnimation(AttackSO attack)
    {
        animator.SetTrigger(attack.animationTrigger);
    }

    private void HitAnimation(EnemyController enemyController)
    {
        isHit = true;
        animator.SetBool("isHit", isHit);
    }

    private void DeathAnimation(EnemyController controller)
    {
        animator.SetBool("isDead", true);
    }

    // 에니메이션 이벤트로 호출될 메서드
    private void EndHitAnimation()
    {
        isHit = false;
        animator.SetBool("isHit", isHit); // Hit이 false이므로 애니메이션 종료
    }

    // 에니메이션 이벤트로 호출될 메서드
    private void TriggerMeleeAttack()
    {
        MeleeAttackSO meleeAttack = (MeleeAttackSO)enemyController.enemyStats.enemyAttackSO[0];
        meleeAttack.TriggerAttack(gameObject);
    }

    public void OnDeathAnimationEnd()
    {
        Destroy(enemyController.gameObject);
    }

    public void ResetAttackAnimation()
    {
        animator.ResetTrigger("MeleeAttack");
        animator.ResetTrigger("RangeAttack");
        animator.SetBool("isAttack", false);
    }
}