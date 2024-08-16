using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    protected EnemyController enemyController;
    private Coroutine attackCooldownCoroutine;
    public float distanceToTarget;

    protected virtual void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    protected void StartAttackCooldown()
    {
        if (attackCooldownCoroutine != null)
        {
            StopCoroutine(attackCooldownCoroutine);
        }
        attackCooldownCoroutine = StartCoroutine(AttackCooldonwn());
    }

    private IEnumerator AttackCooldonwn()
    {
        for (int i = 0; i < enemyController.enemyStats.enemyAttackSO.Length; i++)
        {
            yield return new WaitForSeconds(enemyController.enemyStats.enemyAttackSO[i].attackDelay);
        }
    }
}