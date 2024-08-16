using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttackSO", menuName = "AttackSO/Attacks/MeleeAttack", order = 0)]
public class MeleeAttackSO : AttackSO
{
    public override void Attack(GameObject attacker)
    {
        // 근거리공격로직
    }

    
    public void TriggerAttack(GameObject attacker)
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attacker.transform.position, attackRange, target);
        foreach (Collider2D hitTarget in hitTargets)
        {
            IDamagable damagable = hitTarget.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(Mathf.RoundToInt(attackPower));
                //Debug.Log("Hit target: " + hitTarget.name + " with damage: " + attackPower);
            }
        }
    }
}