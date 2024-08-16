using UnityEngine;

[CreateAssetMenu(fileName = "ThornTrapAttackSO", menuName = "AttackSO/Attacks/Stage1Boss/ThornTrapAttackSO", order = 0)]
public class ThornTrapAttackSO : MeleeAttackSO
{
    public GameObject leftThornTrapPrefab;
    public GameObject rightThornTrapPrefab;

    public override void Attack(GameObject attacker)
    {
        Vector3 position = attacker.transform.position;

        // 가시를 소환할 위치를 결정
        Vector3 leftPosition = new Vector3(position.x, position.y -3.8f, position.z);
        Vector3 rightPosition = new Vector3(position.x, position.y -3.8f, position.z);

       // 가시 오브젝트를 소환
        if (leftThornTrapPrefab != null && rightThornTrapPrefab != null)
        {
            GameObject leftThorn = Instantiate(leftThornTrapPrefab, leftPosition, Quaternion.identity);
            GameObject rightThorn = Instantiate(rightThornTrapPrefab, rightPosition, Quaternion.identity);

            // 5초 후 가시 오브젝트 제거
            Destroy(leftThorn, 3f);
            Destroy(rightThorn, 3f);
        }
    }
}