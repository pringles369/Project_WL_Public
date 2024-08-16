using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneUI : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.PlayBGM("In the Clouds");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}