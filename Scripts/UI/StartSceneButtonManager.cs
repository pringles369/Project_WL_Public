using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneButtonManager : MonoBehaviour
{
    public GameObject settingPanel;

    private void Start()
    {
        // StartScene BGM
        SoundManager.Instance.PlayBGM("WayBack");
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) && settingPanel.activeSelf)
        {
            settingPanel.SetActive(false);
        }
    }

    public void NewGameBtn() // 튜토리얼
    {
        //Debug.Log("게임 시작");
        SoundManager.Instance.PlaySFX("Button_2");
        SoundManager.Instance.PlayBGM("SpringTime");
        SceneManager.LoadScene("TutorialScene"); // 씬이름 변경
    }

    public void LoadGameBtn()
    {
        //Debug.Log("게임 불러오기");
        SoundManager.Instance.PlaySFX("Button_2");
    }

    public void SettingBtn()
    {
        //Debug.Log("설정 열기");
        SoundManager.Instance.PlaySFX("Box_Open_2");
        settingPanel.SetActive(true);
    }

    public void BackBtn()
    {
        SoundManager.Instance.PlaySFX("Box_Close_2");
        settingPanel.SetActive(false);
    }

    public void ExitBtn()
    {
        //Debug.Log("Exit");
        SoundManager.Instance.PlaySFX("Box_Close_2");
        Application.Quit();
    }
}