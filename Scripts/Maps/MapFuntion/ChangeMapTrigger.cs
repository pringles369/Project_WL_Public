using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMapTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        //Debug.Log("씬 전환 시작: " + nextSceneName);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("씬 로드 완료: " + scene.name);
        GameManager.Instance.FindAndAssignPlayer();
        GameManager.Instance.InitializeCurrentScene();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}