using UnityEngine;

public class ThornTrap : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 태그를 가진 오브젝트와 충돌 시 데미지 적용
        if (collision.CompareTag("Player"))
        {
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
            }
        }
    }
}