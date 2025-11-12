using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class QuizData
{
    public string question;
    public string[] options;
    public int correctIndex;
    public Sprite background;
    public int damageOnWrong = 10; // 틀렸을 때 감소할 체력
}

public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    public Image backgroundImage;
    public Text questionText;
    public Button[] optionButtons; // size 3 (또는 필요 만큼)
    public Text feedbackText;
    public Image healthFill; // HealthBarFill(Image with Filled)

    [Header("Quiz Data")]
    public QuizData[] quizzes;

    [Header("References")]
    public FadeController fadeController;

    private int currentIndex = 0;
    private bool inputLocked = false;

    void Start()
    {
        // 불러올 때: PlayerStatusManager의 저장된 인덱스 (있다면)
        if (PlayerStatusManager.Instance != null)
        {
            currentIndex = PlayerStatusManager.Instance.currentQuizIndex;
            UpdateHealthBar();
        }

        ShowQuiz();
    }

    void ShowQuiz()
    {
        inputLocked = false;
        if (feedbackText != null)
            feedbackText.text = "";
            
        if (quizzes == null || quizzes.Length == 0)
        {
            Debug.LogError("QuizManager: quizzes 배열이 비어있거나 할당되지 않았습니다!");
            return;
        }
        
        if (currentIndex >= quizzes.Length)
        {
            // 모든 퀴즈 완료 시 엔딩으로 이동
            StartCoroutine(GoToEnding());
            return;
        }

        QuizData q = quizzes[currentIndex];

        // 배경 이미지
        if (backgroundImage != null && q.background != null)
            backgroundImage.sprite = q.background;

        // 질문 표시
        if (questionText != null)
            questionText.text = q.question;

        // 옵션 버튼
        if (optionButtons != null)
        {
            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (optionButtons[i] == null) continue;
                
                if (i < q.options.Length)
                {
                    optionButtons[i].gameObject.SetActive(true);
                    Text buttonText = optionButtons[i].GetComponentInChildren<Text>();
                    if (buttonText != null)
                        buttonText.text = q.options[i];
                    int idx = i;
                    optionButtons[i].onClick.RemoveAllListeners();
                    optionButtons[i].onClick.AddListener(() => OnAnswer(idx));
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("QuizManager: optionButtons 배열이 할당되지 않았습니다!");
        }
    }

    public void OnAnswer(int index)
    {
        if (inputLocked) return;
        inputLocked = true;

        if (quizzes == null || currentIndex >= quizzes.Length)
        {
            Debug.LogError("QuizManager: 유효하지 않은 퀴즈 인덱스입니다!");
            return;
        }

        QuizData q = quizzes[currentIndex];

        if (index == q.correctIndex)
        {
            if (feedbackText != null)
                feedbackText.text = "정답입니다!";
            // 정답 효과: 버튼 색/효과 추가 가능
            StartCoroutine(NextQuizWithFade());
        }
        else
        {
            // 오답: 체력 감소
            if (PlayerStatusManager.Instance != null)
            {
                PlayerStatusManager.Instance.LoseHealth(q.damageOnWrong);
                UpdateHealthBar();
                if (feedbackText != null)
                    feedbackText.text = "틀렸어요!";
                // 오답 효과: 애니메이션, 사운드(나중) 등 추가 가능

                // 체력이 0이면 엔딩으로
                if (PlayerStatusManager.Instance.health <= 0)
                {
                    StartCoroutine(GoToEnding());
                    return;
                }
            }
            else
            {
                Debug.LogWarning("PlayerStatusManager.Instance가 null입니다! 씬에 PlayerStatusManager가 있는지 확인해주세요.");
                if (feedbackText != null)
                    feedbackText.text = "틀렸어요!";
            }
            
            // 피드백 보여주고 다시 입력 받기
            StartCoroutine(AllowInputAfterDelay(0.8f));
        }
    }

    IEnumerator AllowInputAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        inputLocked = false;
        if (feedbackText != null)
            feedbackText.text = "";
    }

    IEnumerator NextQuizWithFade()
    {
        if (fadeController != null && fadeController.fadeImage != null)
        {
            yield return StartCoroutine(fadeController.FadeOut(0.4f));
        }
        currentIndex++;
        if (PlayerStatusManager.Instance != null)
            PlayerStatusManager.Instance.currentQuizIndex = currentIndex;
        ShowQuiz();
        if (fadeController != null && fadeController.fadeImage != null)
        {
            yield return StartCoroutine(fadeController.FadeIn(0.4f));
        }
    }

    IEnumerator GoToEnding()
    {
        // 페이드 아웃 후 엔딩 씬으로 이동
        if (fadeController != null && fadeController.fadeImage != null)
        {
            yield return StartCoroutine(fadeController.FadeOut(0.4f));
        }
        // 엔딩 씬으로 이동 (Build Settings에 추가되어 있어야 함)
        // 주의: "Outdoor_Ending" 씬이 Build Settings에 추가되어 있어야 합니다!
        if (Application.CanStreamedLevelBeLoaded("Outdoor_Ending"))
        {
            SceneManager.LoadScene("Outdoor_Ending");
        }
        else
        {
            Debug.LogError("Outdoor_Ending 씬을 찾을 수 없습니다! Build Settings에 씬을 추가해주세요.");
            // 대안: 현재 씬을 다시 로드하거나 에러 메시지 표시
        }
    }

    void UpdateHealthBar()
    {
        if (healthFill == null || PlayerStatusManager.Instance == null) return;
        float max = Mathf.Max(1, PlayerStatusManager.Instance.batteryCount * 10);
        float ratio = (float)PlayerStatusManager.Instance.health / max;
        healthFill.fillAmount = Mathf.Clamp01(ratio);
    }

    // 외부에서 체력 UI를 다시 업데이트하고 싶을 때 public으로 호출 가능
    public void ExternalUpdateHealthUI()
    {
        UpdateHealthBar();
    }


}
