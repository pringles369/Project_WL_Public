using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(1f, 0f);
        fadeImage.gameObject.SetActive(false);
    }

    public IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        yield return Fade(0f, 1f);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }
}