using UnityEngine;
using System.Collections;

public class BossStatHandler : MonoBehaviour, IDamagable
{
    private ForestWitchController forestWitchController;
    private EnemyStats enemyStats;
    private SpriteRenderer spriteRenderer;

    private bool isInvincible = false; // 무적 상태를 나타내는 변수

    private void Start()
    {
        forestWitchController = GetComponent<ForestWitchController>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyStats = forestWitchController.enemyStats;
    }

    private void Update()
    {
        if (enemyStats.maxHP <= 0)
        {
            forestWitchController.OnDeath();
        }
    }

    public void TakeDamage(float amount)
    {
        // return 하여 무적시간
        if (isInvincible)
        {
            return;
        }

        // 무적 상태가 아닐 경우
        enemyStats.maxHP -= amount;

        // 피격 후 무적 상태로 변경
        StartCoroutine(BecomeInvincible());
    }

    private IEnumerator BecomeInvincible()
    {
        if (enemyStats.maxHP <= 0) yield break;

        isInvincible = true;
        forestWitchController.isHit = true;
        forestWitchController.OnHit();

        // 무적 상태 지속 시간 (3초)
        yield return new WaitForSeconds(3f);

        isInvincible = false;
        forestWitchController.isHit = false;
        forestWitchController.OnHit();

        yield break;
    }
}