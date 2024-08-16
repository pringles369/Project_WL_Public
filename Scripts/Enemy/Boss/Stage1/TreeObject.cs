using System.Collections;
using UnityEngine;

public class TreeObject : MonoBehaviour
{
    public float damageAmount = 1f;
    public Animator animator;
    private Rigidbody2D rb;
    //public float groundOffset = 0.1f; // 지면 위로 떨어질 높이

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // 시작 시 중력 비활성화
        rb.isKinematic = true;

        // 1초 후 중력 활성화
        StartCoroutine(ActivateGravityAfterDelay(0.5f));
    }

    private IEnumerator ActivateGravityAfterDelay(float delay)
    {
        // delay 후 중력활성화
        yield return new WaitForSeconds(delay);

        rb.isKinematic = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IDamagable damagable = collision.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.TakeDamage(damageAmount);
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("groundLayer"))
        {
            //트리 오브젝트를 지면 위로 약간 이동시킴
            //transform.position = new Vector3(transform.position.x, collision.transform.position.y + groundOffset, transform.position.z);

            // 지면에 닿으면 오브젝트의 이동을 멈추고 애니메이션 재생
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            animator.Play("TreeObject_Drop");
            Destroy(gameObject, 0.5f);
        }
    }
}