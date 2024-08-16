using UnityEngine;

public class AppleProjectile : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 충돌 시
        if (collision.CompareTag("Player"))
        {
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("groundLayer"))
        {
            Destroy(gameObject);
        }
    }
}