using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour
{
    public GameObject currentMap; // 현재 맵 프리팹
    public Transform mapParent; // 맵이 위치할 부모 오브젝트
    public Transform player; // 플레이어의 Transform
    public Vector3 defaultPlayerPosition; // 플레이어의 기본 위치

    public void LoadMap(GameObject mapPrefab)
    {
        StartCoroutine(LoadMapAsync(mapPrefab));
    }

    private IEnumerator LoadMapAsync(GameObject mapPrefab)
    {
        // 기존 맵 언로드
        if (currentMap != null)
        {
            Destroy(currentMap);
            yield return Resources.UnloadUnusedAssets();
            System.GC.Collect(); // 가비지 컬렉션 강제 실행
        }

        yield return null; // 한 프레임 대기

        // 새로운 맵 로드
        currentMap = Instantiate(mapPrefab, mapParent);

        // 플레이어 위치 설정
        Transform startPoint = currentMap.transform.Find("StartPosition");
        if (startPoint != null)
        {
            player.position = startPoint.position;
        }
        else
        {
            //Debug.LogWarning("StartPosition not found in the new map.");
            player.position = defaultPlayerPosition; // 기본 위치로 설정
        }
    }
}