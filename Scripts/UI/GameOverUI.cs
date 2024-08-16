using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Button restartButton;

    private void Start()
    {
        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        GameManager.Instance.RestartGame();
    }
}