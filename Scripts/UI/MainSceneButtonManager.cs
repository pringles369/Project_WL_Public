using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneButtonManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject stopPanel;
    public GameObject settingPanel;

    public Button pauseButton;
    public Button resumeButton;
    public Button optionButton;
    public Button saveButton;
    public Button exitButton;
    public Button backButton;

    private bool isPaused = false;

    private void Start()
    {
        SetNavigationToNone(pauseButton);
        SetNavigationToNone(resumeButton);
        SetNavigationToNone(optionButton);
        SetNavigationToNone(saveButton);
        SetNavigationToNone(exitButton);
        SetNavigationToNone(backButton);
    }

    // 일시정지 패널에서 화살표를 띄워주는 코드
    private void SetNavigationToNone(Button button)
    {
        var nav = button.navigation;
        nav.mode = Navigation.Mode.None;
        button.navigation = nav;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingPanel.activeSelf)
            {
                BackBtn();
            }
            else if (pauseCanvas.activeSelf)
            {
                ResumeBtn();
            }
            else
            {
                PauseBtn();
            }
        }
    }

    public void PauseBtn()
    {
        SoundManager.Instance.PlaySFX("Box_Open_2");
        pauseCanvas.SetActive(true);
        isPaused = true;
        //Time.timeScale = 0f;
    }

    public void ResumeBtn()
    {
        SoundManager.Instance.PlaySFX("Button_2");
        pauseCanvas.SetActive(false);
        isPaused = false;
        //Time.timeScale = 1f;
    }

    public void OptionBtn()
    {
        SoundManager.Instance.PlaySFX("Box_Open_2");
        stopPanel.SetActive(false);
        settingPanel.SetActive(true);
    }

    public void SaveBtn()
    {
        //게임 진행상황 저장
        SoundManager.Instance.PlaySFX("Button_2");
        //Debug.Log("게임 저장");
    }

    public void ExitBtn()
    {
        Time.timeScale = 1f;
        SoundManager.Instance.PlaySFX("Box_Close_2");
        SceneManager.LoadScene("StartScene");
    }

    public void BackBtn()
    {
        SoundManager.Instance.PlaySFX("Box_Close_2");
        settingPanel.SetActive(false);
        stopPanel.SetActive(true);
    }
   
}