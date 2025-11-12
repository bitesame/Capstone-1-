using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager1 : MonoBehaviour
{
    public static VolumeManager1 Instance { get; private set; }
    public AudioSource bgmSource;
    private const string VolumeKey = "bgm_volume";

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);  // 씬 이동해도 안 사라짐

        LoadVolume();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetVolume(float v)
    {
        if (bgmSource == null)
        {
            Debug.LogError("BGM AudioSource가 연결되지 않았습니다!");
            return;
        }
        bgmSource.volume = v;

        
        PlayerPrefs.SetFloat(VolumeKey, v);
        PlayerPrefs.Save();
    }

    public void LoadVolume()
    {
        float saved = PlayerPrefs.GetFloat(VolumeKey, 1f);
        if (bgmSource != null)
            bgmSource.volume = saved;
    }
}
