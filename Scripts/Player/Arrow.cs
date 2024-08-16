using UnityEngine;

public class Arrow : PooledObject<Arrow>, IPooledObject
{
    public float speed = 10f;    

    public void OnObjectSpawn()
    {
        // 초기화 작업
    }

    public void Initialize(bool isFacingRight)
    {
        Collider2D arrowCollider = GetComponent<Collider2D>();        

        if (arrowCollider != null)
        {
            arrowCollider.isTrigger = false; // 발사 시 isTrigger를 끔
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = (isFacingRight ? Vector2.right : Vector2.left) * speed;
        }
    }
}