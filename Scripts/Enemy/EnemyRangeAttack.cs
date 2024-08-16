using UnityEngine;

public class EnemyRangeAttack : EnemyAttack
{
    protected override void Start()
    {
        base.Start();
    }

    public void RangeAttack()
    {
        if (enemyController.ClosestTarget != null)
        {
            for (int i = 0; i < enemyController.enemyStats.enemyAttackSO.Length; i++)
            {
                AttackSO attack = enemyController.enemyStats.enemyAttackSO[i];
                if (attack.attackType == AttackType.Range)
                {
                    distanceToTarget = Vector2.Distance(transform.position, enemyController.ClosestTarget.position);
                    if (distanceToTarget <= attack.attackRange)
                    {
                        enemyController.isAttack = true;
                        attack.Attack(this.gameObject);
                        enemyController.OnAttack(attack);
                        StartAttackCooldown();
                        break;
                    }
                }
            }
        }
    }
}