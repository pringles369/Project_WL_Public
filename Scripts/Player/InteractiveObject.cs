using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    private PlayerController player;
    public ObjectType objectType;
    protected bool isInteractiveObject;

    protected void OnTriggerEnter2D(Collider2D collision) //한정자 변경
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerController>();
            isInteractiveObject = true;

            if (player != null)
            {
                player.interactiveObject = this;

                if (objectType == ObjectType.Arrow)
                {
                    player.AddArrows(1);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInteractiveObject = false;

            if (player != null)
            {
                player.interactiveObject = null;
                player = null;
            }
        }
    }

    public void Interaction()
    {
        if (isInteractiveObject)
        {
            //Debug.Log("상호작용");
            OnInteraction();
        }
    }

    protected virtual void OnInteraction()
    {
        // 상속된 클래스에서 재정의 가능
    }
}