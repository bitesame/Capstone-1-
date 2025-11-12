using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static LanguageManager1;
public class TutorialUIController1 : MonoBehaviour
{
    public static TutorialUIController1 Instance { get; private set; }

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text saveButtonLabel;
    [SerializeField] private TMP_Text startButtonLabel;
    [SerializeField] private TMP_Text OneText;
    [SerializeField] private TMP_Text TwoText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable()
    {
        ApplyTexts();

        if (LanguageManager1.Instance != null)
            LanguageManager1.Instance.OnLanguageChanged += OnLanguageChanged;
    }

    void OnDisable()
    {
        if (LanguageManager1.Instance != null)
            LanguageManager1.Instance.OnLanguageChanged -= OnLanguageChanged;
    }

    private void OnLanguageChanged(string country)
    {
        ApplyTexts();
    }

    private void ApplyTexts()
    {
        if (titleText)
            titleText.text = LanguageManager1.Instance.Get("title");

        if (saveButtonLabel)
            saveButtonLabel.text = LanguageManager1.Instance.Get("save");

        if (startButtonLabel)
            startButtonLabel.text = LanguageManager1.Instance.Get("start");
        if (OneText)
            OneText.text = LanguageManager1.Instance.Get("one");
        if (TwoText)
            TwoText.text = LanguageManager1.Instance.Get("two");

    }
}
