using UnityEngine;
using UnityEngine.Tilemaps;

public class JumpPad : MonoBehaviour
{
    public Tilemap jumpTilemap; // 점프 타일이 있는 타일맵
    public float jumpForce = 10f; // 점프 발판의 힘

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // CompareTag 충돌
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                Vector3Int cell = jumpTilemap.WorldToCell(hitPosition);

                // 타일이 존재하는지 확인
                TileBase tile = jumpTilemap.GetTile(cell);
                if (tile != null)
                {
                    Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    }
                }
            }
        }
    }
}