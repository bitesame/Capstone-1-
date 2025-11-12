using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class StringPair1 
{
    public string key; 
    public string value; 
}

[System.Serializable]
public class LanguageTable1 
{ 
    public List<LanguageEntry1> languages; 
}

[System.Serializable]
public class LanguageEntry1
{
    public string country;
    public List<StringPair1> strings;
}

public class LanguageManager1 : MonoBehaviour
{
    public static LanguageManager1 Instance { get; private set; }
    private const string PlayerPrefsKey = "lang_country";
    private const string DefaultCountry = "US";

    private LanguageTable1 _table;
    private Dictionary<string, string> _currentStrings = new Dictionary<string, string>();
    public string CurrentCountry { get; private set; } = DefaultCountry;
   // public object Instance { get; private set; }

    public delegate void LanguageChanged(string country);
    public event LanguageChanged OnLanguageChanged;
    // Start is called before the first frame update
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        GameObject go = new GameObject("LanguageManager1");
        go.AddComponent<LanguageManager1>();
        DontDestroyOnLoad(go);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadTableFromResources();
        string saved = PlayerPrefs.GetString(PlayerPrefsKey, DefaultCountry);
        SetCountry(saved, invokeEvent: false);
    }

    private void LoadTableFromResources()
    {
        TextAsset json = Resources.Load<TextAsset>("language.json");
        _table = JsonUtility.FromJson<LanguageTable1>(json.text);
    }

    public void SetCountry(string country, bool invokeEvent = true)
    {
        var entry = _table.languages.Find(l => l.country == country);
        if (entry == null)
        {
            entry = _table.languages.Find(l => l.country == DefaultCountry);
            country = DefaultCountry;
        }

        _currentStrings.Clear();
        foreach (var p in entry.strings)
        {
            _currentStrings[p.key] = p.value;
        }

        CurrentCountry = country;
        PlayerPrefs.SetString(PlayerPrefsKey, CurrentCountry);
        PlayerPrefs.Save();
        if (invokeEvent) OnLanguageChanged?.Invoke(CurrentCountry);
    }

    public string Get(string key)
    {
        if (_currentStrings.TryGetValue(key, out var v))
            return v;

        return key; // 키가 없으면 키 그대로 표시 (디버깅 쉽게)
    }

}
