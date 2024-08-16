using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public Transform[] targetTransforms;
    public float damage = 2f; // SafeZone에 닿았을 때 줄 데미지

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Collision Detected with: " + other.name);

        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player detected, moving to nearest target position.");

            Transform nearestTarget = GetNearestTarget(other.transform.position);

            if (nearestTarget != null)
            {
                other.transform.position = nearestTarget.position;

                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.damageAmount = damage; // 데미지 설정
                    player.OnHit();
                }
            }
        }
    }

    private Transform GetNearestTarget(Vector3 currentPosition)
    {
        Transform nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Transform target in targetTransforms)
        {
            float distance = Vector3.Distance(currentPosition, target.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = target;
            }
        }

        return nearestTarget;
    }
}