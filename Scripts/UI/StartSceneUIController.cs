using System.Collections;
using UnityEngine;

public class StartSceneUIController : MonoBehaviour
{
    public RectTransform bgImage;
    public float targetY;
    public float moveDuration;

    void Start()
    {
        MoveBgImage();
    }

    private void MoveBgImage()
    {
        if(bgImage != null)
        {
            StartCoroutine(MoveImage(bgImage, targetY, moveDuration));
        }
    }

    private IEnumerator MoveImage(RectTransform rectTransform, float targetY, float duration)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 targetPosition = new Vector2(startPosition.x, targetY);
        float elapsed = 0f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }
}