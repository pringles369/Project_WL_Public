using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startPoint; // 시작 위치
    public Transform endPoint; // 끝 위치
    public Transform alternateEndPoint; // 대체 끝 위치
    public float speed = 2f; // 이동 속도
    private bool movingToEnd = true;
    private bool usingAlternateEndPoint = false;
    private bool toggleRequested = false;

    void Update()
    {
        Transform targetPoint = usingAlternateEndPoint ? alternateEndPoint : endPoint;

        if (movingToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.01f)
            {
                movingToEnd = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPoint.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPoint.position) < 0.01f)
            {
                movingToEnd = true;
                if (toggleRequested)
                {
                    ToggleEndPoint();
                    toggleRequested = false;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //Debug.Log("Player entered platform.");
            collision.transform.SetParent(transform); // 플레이어를 플랫폼의 자식으로 설정
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //Debug.Log("Player exited platform.");
            collision.transform.SetParent(null); // 플레이어가 플랫폼을 벗어나면 자식 관계 해제
            DontDestroyOnLoad(collision.gameObject); // 플레이어를 다시 DontDestroyOnLoad 상태로 설정
        }
    }

    public void RequestToggleEndPoint()
    {
        toggleRequested = true;
    }

    public void ToggleEndPoint()
    {
        usingAlternateEndPoint = !usingAlternateEndPoint;
    }
}