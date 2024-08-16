using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    public string EnemyName;
    [Range(0.0f, 20.0f)] public float maxHP;
    public float moveSpeed;
    public float knockbackDistance = 1f;
    public AttackSO[] enemyAttackSO; // 근거리와 원거리공격을 모두하는 경우를 대비해 배열로 선언 << Enum으로 타입을 만들어서 (Basic, Both) 공격 시 구분할 수 있도록
}