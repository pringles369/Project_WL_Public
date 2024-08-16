using UnityEngine;

[CreateAssetMenu(fileName = "ThrowAppleAttackSO", menuName = "AttackSO/Attacks/Stage1Boss/ThrowAppleAttackSO", order = 1)]
public class ThrowAppleAttackSO : RangeAttackSO
{
    public GameObject applePrefab;

    public override void Attack(GameObject attacker)
    {

        if (applePrefab != null)
        {
            Vector3 position = attacker.transform.position;

            // 사과를 소환할 위치를 결정
            Vector3 applePosition = new Vector3(position.x -1.5f, position.y -1, position.z);

            GameObject apple = Instantiate(applePrefab, applePosition, Quaternion.identity);

            // 플레이어 방향으로 사과 던지기
            ForestWitchController controller = attacker.GetComponent<ForestWitchController>();
            if (controller != null && controller.player != null)
            {
                Vector3 direction = (controller.player.transform.position - position).normalized;
                apple.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            }
        }
    }
}