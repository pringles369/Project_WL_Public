using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    [SerializeField] private LoadingManager loadingManager;

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(ChangeSceneRoutine(sceneName));
    }

    private IEnumerator ChangeSceneRoutine(string sceneName)
    {
        yield return loadingManager.FadeOut();
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("씬 로드 완료: " + scene.name);
        StartCoroutine(loadingManager.FadeIn());
        GameManager.Instance.FindAndAssignPlayer();
        GameManager.Instance.InitializeCurrentScene();
    }
}