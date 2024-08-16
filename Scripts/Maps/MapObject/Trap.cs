using UnityEngine;

public class Trap : InteractiveObject
{
    public GameObject monsterPrefab;  // 소환할 몬스터 프리팹
    private bool isActivated = false; // 함정이 활성화되었는지 여부를 추적

    protected override void OnInteraction()
    {
        base.OnInteraction();

        if (objectType == ObjectType.Trap && !isActivated) // 아직 활성화되지 않은 경우에만 동작
        {
            isActivated = true; // 함정을 활성화된 상태로 설정
            Vector3 spawnPosition = transform.position + Vector3.up * 2; // 몬스터가 트랩 오른쪽에 소환되도록 위치 설정
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            //Debug.Log("몬스터 소환");
        }
    }
}