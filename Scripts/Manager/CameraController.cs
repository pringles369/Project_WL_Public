using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        // Player 태그를 가진 오브젝트를 찾아 할당
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            //Debug.LogError("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        // 카메라의 위치를 플레이어의 위치로 고정
        Vector3 newPosition = player.transform.position;
        newPosition.z = this.transform.position.z; // 카메라의 z축 위치는 그대로 유지
        this.transform.position = newPosition;
    }
}