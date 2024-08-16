using UnityEngine;

[CreateAssetMenu(fileName = "RangeAttackSO", menuName = "AttackSO/Attacks/RangeAttack", order = 1)]
public class RangeAttackSO : AttackSO
{
    // AttackSO를 상속하므로 원거리 공격에만 있는 옵션 정의
    [Header("Range Attack Info")]
    public string bulletNameTag;
    public float bulletDuration;
    public float bulletSpeed;

    public override void Attack(GameObject attacker)
    {
        // 원거리 공격 로직
    }
}