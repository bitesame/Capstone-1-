using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsSceneController1 : MonoBehaviour
{
    [SerializeField] private string tutorialSceneName = "Tutorial";

    private string selectedCountry = null;
    public UnityEngine.UI.Slider volumeSlider;

    public void OnClickFlag_KR()
    {
        selectedCountry = "KR";
    }

    public void OnClickFlag_US()
    {
        selectedCountry = "US";
    }

    public void OnClickFlag_JA()
    {
        selectedCountry = "JA";
    }

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("bgm_volume", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickConfirm()
    {
        if (string.IsNullOrEmpty(selectedCountry))
        {
            Debug.LogWarning("국가를 선택하지 않았습니다.");
            return;
        }

        LanguageManager1.Instance.SetCountry(selectedCountry);
        //SceneManager.LoadScene(tutorialSceneName);
    }

    public void OnVolumeChanged(float value)
    {
        VolumeManager1.Instance.SetVolume(value);
    }
}
