using System.Collections.Generic;
using UnityEngine;

public class MapSettings : MonoBehaviour
{
    [SerializeField] private int requiredActivatedStones;  // 필요한 활성화된 스톤 개수
    [SerializeField] private GameObject doorPrefab;  // 맵에 대한 문 프리팹
    [SerializeField] private List<SpriteChanger> interactiveStones;  // 맵에 있는 인터랙티브 스톤 목록
    [SerializeField] private Transform startPoint;  // 플레이어의 시작 지점

    public int RequiredActivatedStones => requiredActivatedStones;
    public GameObject DoorPrefab => doorPrefab;
    public List<SpriteChanger> InteractiveStones => interactiveStones;
    public Transform StartPoint => startPoint;
}