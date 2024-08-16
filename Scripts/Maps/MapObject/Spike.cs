using UnityEngine;

public class Spike : MonoBehaviour
{
    public float damage = 1f; // 가시가 주는 데미지 설정

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.damageAmount = damage; // 데미지 설정
                player.OnHit(); // 플레이어에게 데미지를 줌
                //Debug.Log("Player hit by spike! Damage: " + damage);
            }
        }
    }
}