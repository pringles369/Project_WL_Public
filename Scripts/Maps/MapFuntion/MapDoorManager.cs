using UnityEngine;

public class MapDoorManager : MonoBehaviour
{
    private int requiredActivatedStones; // 필요한 활성화된 스톤 개수
    private int activatedStoneCount = 0; // 현재 활성화된 스톤 개수
    private GameObject door;

    private void Awake()
    {
        door = this.gameObject; // 문 오브젝트를 스크립트가 부착된 오브젝트로 설정
        //Debug.Log("MapDoorManager가 초기화되었습니다: " + door.name);
    }

    public void Initialize(MapSettings mapSettings)
    {
        //Debug.Log("MapDoorManager 초기화 시작");

        requiredActivatedStones = mapSettings.RequiredActivatedStones;
        activatedStoneCount = 0;

        if (door != null)
        {
            door.SetActive(true); // 초기에는 문을 활성화 상태로 설정
            //Debug.Log("Door 활성화됨: " + door.name);
        }
        else
        {
            //Debug.LogError("문 오브젝트가 설정되지 않았습니다.");
        }

        foreach (var stone in mapSettings.InteractiveStones)
        {
            if (stone != null)
            {
                stone.OnInteractionEvent += StoneActivated;
                //Debug.Log("이벤트가 돌에 연결되었습니다: " + stone.name);
            }
        }

        //Debug.Log("MapDoorManager 초기화 완료");
    }

    private void StoneActivated()
    {
        activatedStoneCount++;
        //Debug.Log($"Stone activated. Current count: {activatedStoneCount}/{requiredActivatedStones}");

        if (activatedStoneCount >= requiredActivatedStones)
        {
            //Debug.Log("Required stones activated. Opening door...");
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        if (door != null)
        {
            door.SetActive(false); // 문을 비활성화하여 제거
            //Debug.Log("문이 열렸습니다: " + door.name);
        }
        else
        {
            //Debug.LogError("문 오브젝트가 null입니다. 문을 열 수 없습니다.");
        }
    }

    public bool IsDoorOpen()
    {
        return activatedStoneCount >= requiredActivatedStones;
    }
}