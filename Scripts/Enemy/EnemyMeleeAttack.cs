using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
    protected override void Start()
    {
        base.Start();
    }

    public void MeleeAttack()
    {
        if (enemyController.ClosestTarget)
        {
            for (int i = 0; i < enemyController.enemyStats.enemyAttackSO.Length; i++)
            {
                AttackSO attack = enemyController.enemyStats.enemyAttackSO[i];
                if (attack.attackType == AttackType.Melee)
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