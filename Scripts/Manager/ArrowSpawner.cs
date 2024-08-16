using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public Transform firePoint;
    private PlayerController playerController;
    public GameObject Arrow;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            //Debug.LogError("PlayerController를 찾을 수 없습니다.");
        }
    }

    public void ShootArrow()
    {
        if (playerController.UseArrow())
        {
            Arrow arrow = ObjectPool<Arrow>.Instance.SpawnFromPool(firePoint.position, firePoint.rotation);
            if (arrow != null)
            {
                arrow.Initialize(playerController.isFacingRight);
                Instantiate(Arrow, firePoint.position, transform.rotation);
            }
        }
    }
}