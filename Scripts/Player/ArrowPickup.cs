using UnityEngine;

public class ArrowPickup : InteractiveObject
{
    protected override void OnInteraction()
    {
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (player != null)
        {
            player.AddArrows(1);
            Destroy(gameObject);
        }
    }
}