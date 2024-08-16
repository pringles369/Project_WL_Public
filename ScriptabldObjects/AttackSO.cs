using UnityEngine;

public abstract class AttackSO : ScriptableObject
{
    [Header("Attack Info")]
    public string animationTrigger; // 애니메이션 트리거 이름 추가 ( MeleeAttack or RangeAttack로 작성)
    public float attackDelay;
    public float attackPower;
    public float attackSpeed;
    public float attackRange;
    public LayerMask target;
    public AttackType attackType;

    // 타입별 공격로직 작성을 시키게하기 위한 추상메서드사용
    public abstract void Attack(GameObject attacker); // attacker를 통해 위치와 상태 정보를 가져온다
}