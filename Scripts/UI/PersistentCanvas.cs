using UnityEngine;

public class PersistentCanvas : MonoBehaviour
{
    private static PersistentCanvas instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);  // 이미 인스턴스가 존재하면 새로운 인스턴스를 파괴
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);  // Canvas가 씬 전환 시 파괴되지 않도록 설정
    }
}