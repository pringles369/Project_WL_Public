using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartSceneSetting : MonoBehaviour
{
    public GameObject settingPanel;

    public Slider Bgm;
    public Slider Fsx;

    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;

    public TMP_Text resolutionLabel;

    public Toggle fullscreenTog;

    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        selectedResolution = resolutions.Count - 1;

        // 초기 슬라이더 값 설정
        Bgm.value = SoundManager.Instance.bgmSource.volume;
        Fsx.value = SoundManager.Instance.sfxSources[0].volume;

        UpdateResLabel();

        // 슬라이더 값 변경 시 메서드 호출 설정
        Bgm.onValueChanged.AddListener(SetBGMVolume);
        Fsx.onValueChanged.AddListener(SetSFXVolume);
    }

    public void BackBtn()
    {
        SoundManager.Instance.PlaySFX("Button_2");
        settingPanel.SetActive(false);
    }

    public void ResLeft()
    {
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = 0;
        }

        SoundManager.Instance.PlaySFX("Button_2");
        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;
        if(selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }

        SoundManager.Instance.PlaySFX("Button_2");
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplySettings()
    {
        SoundManager.Instance.PlaySFX("Button_2");
        Screen.fullScreen = fullscreenTog.isOn;
        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
    }

    public void SetBGMVolume(float volume)
    {
        SoundManager.Instance.SetBGMVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        SoundManager.Instance.SetSFXVolume(volume);
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal;
    public int vertical;
}