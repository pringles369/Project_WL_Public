using UnityEngine;
using UnityEngine.Tilemaps;

public class HidePlatform : MonoBehaviour
{
    public Tilemap tilemap; // 타일맵 컴포넌트를 참조
    public TileBase activeTile; // 활성화된 상태의 타일
    public TileBase inactiveTile; // 비활성화된 상태의 타일
    public Vector3Int[] tilePositions; // 타일 위치 배열
    public string platformTag; // 오브젝트 풀에서 사용할 태그

    private int currentIndex = 0; // 현재 활성화된 타일 인덱스

    void Start()
    {
        // 초기화 시 첫 번째 타일만 활성화하고 나머지는 비활성화
        for (int i = 0; i < tilePositions.Length; i++)
        {
            tilemap.SetTile(tilePositions[i], inactiveTile);
        }
        if (tilePositions.Length > 0)
        {
            ActivateTile(0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 현재 타일 비활성화
            tilemap.SetTile(tilePositions[currentIndex], inactiveTile);

            // 다음 타일 활성화
            currentIndex = (currentIndex + 1) % tilePositions.Length;
            ActivateTile(currentIndex);
        }
    }

    void ActivateTile(int index)
    {
        tilemap.SetTile(tilePositions[index], activeTile);

        // 오브젝트 풀에서 발판 오브젝트를 가져와서 활성화
       // GameObject platform = ObjectPool.Instance.SpawnFromPool(platformTag, tilemap.CellToWorld(tilePositions[index]) + tilemap.tileAnchor, Quaternion.identity);
        //platform.SetActive(true);
    }
}