using UnityEngine;

public class SpriteChanger : InteractiveObject
{
    [SerializeField] private Sprite defaultSprite;  // 기본 스프라이트
    [SerializeField] private Sprite activeSprite;  // 상호작용 후 스프라이트

    private SpriteRenderer spriteRenderer;
    private bool isActive = false;

    public bool IsActive => isActive;

    public event System.Action OnInteractionEvent;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = defaultSprite;  // 초기 스프라이트 설정
        }
    }

    protected override void OnInteraction()
    {
        if (spriteRenderer != null && !isActive)
        {
            isActive = true;
            spriteRenderer.sprite = activeSprite;  // 스프라이트 변경

            OnInteractionEvent?.Invoke();  // 이벤트 호출
            //Debug.Log("SpriteChanger interacted with and event invoked.");
        }
    }
}