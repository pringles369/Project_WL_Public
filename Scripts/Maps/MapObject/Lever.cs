using UnityEngine;

public class Lever : MonoBehaviour
{
    public MovingPlatform platform; // 연결된 이동 플랫폼
    private bool isActivated = false;

    // 플레이어와 레버의 충돌을 감지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActivated = true;
        }
    }

    // 플레이어가 레버 영역을 떠날 때
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActivated = false;
        }
    }

    private void Update()
    {
        if (isActivated && Input.GetKeyDown(KeyCode.E))
        {
            platform.RequestToggleEndPoint();
            SoundManager.Instance.PlaySFX("Button_2");

        }
    }
}