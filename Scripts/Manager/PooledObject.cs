using UnityEngine;

public class PooledObject<T> : MonoBehaviour where T : Component
{
    public virtual void OnObjectSpawn()
    {
        // 오브젝트가 풀에서 스폰될 때 실행되는 코드
        // 초기화 작업을 여기에 추가합니다.
    }

    private void OnDisable()
    {
        if (ObjectPool<T>.Instance != null)
        {
            ObjectPool<T>.Instance.ReturnToPool(this as T);
        }
    }
}