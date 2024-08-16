using UnityEngine;

[CreateAssetMenu(fileName = "DropTreehAttackSO", menuName = "AttackSO/Attacks/Stage1Boss/DropTreehAttackSO", order = 2)]
public class DropTreeAttackSO : MeleeAttackSO
{
    public GameObject treePrefab;
    public float offset = 3f; // 양옆으로 떨어질 거리
    public int numberOfTrees = 10; // 떨어질 나무의 수
    public float patternOffset = 0.5f; // 두 가지 패턴을 위한 X 오프셋

    public override void Attack(GameObject attacker)
    {
        //Debug.Log("DropTreeAttackSO Attack method called");
        Vector3 basePosition = attacker.transform.position;

        bool isFirstPattern = Random.value > 0.5f; // 패턴을 랜덤하게 결정

        for (int i = 0; i < numberOfTrees; i++)
        {
            float xPosition = basePosition.x + (i - numberOfTrees / 2) * offset;

            if (isFirstPattern)
            {
                // 첫번째 패턴: x축으로 -0.5
                Vector3 spawnPosition = new Vector3(xPosition - patternOffset, basePosition.y +2, basePosition.z);
                GameObject tree = Instantiate(treePrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                // 두번째 패턴: x축으로 +0.5
                Vector3 spawnPosition = new Vector3(xPosition + patternOffset, basePosition.y +2, basePosition.z);
                GameObject tree = Instantiate(treePrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}