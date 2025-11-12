using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


// JSON 데이터 모델
[System.Serializable]
public class DialogueLine
{
    public string speaker;
    public string text;
}

[System.Serializable]
public class DialogueData
{
    public DialogueLine[] lines;
}

public class BadEndingManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    public Image backgroundImage;    // news image
    public GameObject dialoguePanel; // 대화창 패널
    public Text speakerText;
    public Text dialogueText;
    public Button nextButton;

    [Header("Video")]
    public RawImage videoRawImage;   // Video 출력용 UI
    public RenderTexture renderTexture; // 연결된 렌더텍스처
    public VideoPlayer videoPlayer;  // VideoPlayer 컴포넌트
    public VideoClip[] videoClips;   // 재생할 동영상들 (순서대로)

    [Header("Flow")]
    public string dialogResourcePath = "Dialogs/bad_ending"; // Resources 폴더 내부 경로 (확장자 없이)

    private DialogueData dialogData;
    private int currentLine = 0;

    void Start()
    {
        // 초기 UI 설정
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (videoRawImage != null) videoRawImage.gameObject.SetActive(false);

        LoadDialog();
        ShowCurrentLine();

        // 버튼 리스너
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(OnNextClicked);
        }

        // 비디오 플레이어 설정
        if (videoPlayer != null && renderTexture != null && videoRawImage != null)
        {
            videoPlayer.targetTexture = renderTexture;
            videoRawImage.texture = renderTexture;
            videoPlayer.playOnAwake = false;
            videoPlayer.loopPointReached += OnVideoFinished; // 재생 완료 콜백
        }
    }

    void LoadDialog()
    {
        // Resources에서 텍스트 에셋 로드
        TextAsset ta = Resources.Load<TextAsset>(dialogResourcePath);
        if (ta == null)
        {
            Debug.LogError("Dialog JSON not found at Resources/" + dialogResourcePath);
            dialogData = new DialogueData { lines = new DialogueLine[0] };
            return;
        }

        dialogData = JsonUtility.FromJson<DialogueData>(ta.text);
        if (dialogData == null || dialogData.lines == null)
        {
            Debug.LogError("Dialog JSON parse error!");
            dialogData = new DialogueData { lines = new DialogueLine[0] };
        }
    }

    void ShowCurrentLine()
    {
        if (dialogData == null || dialogData.lines.Length == 0)
        {
            // 바로 영상 재생
            StartCoroutine(PlayVideosSequence());
            return;
        }

        if (currentLine < dialogData.lines.Length)
        {
            var line = dialogData.lines[currentLine];
            if (speakerText != null) speakerText.text = line.speaker;
            if (dialogueText != null) dialogueText.text = line.text;
        }
        else
        {
            // 모든 대사 끝 → 비디오 재생
            StartCoroutine(PlayVideosSequence());
        }
    }

    void OnNextClicked()
    {
        currentLine++;
        if (currentLine < dialogData.lines.Length)
        {
            ShowCurrentLine();
        }
        else
        {
            // 대사 패널 숨기고 비디오 패널 활성화
            if (dialoguePanel != null) dialoguePanel.SetActive(false);
            if (videoRawImage != null) videoRawImage.gameObject.SetActive(true);

            StartCoroutine(PlayVideosSequence());
        }
    }

    IEnumerator PlayVideosSequence()
    {
        if (videoPlayer == null || videoClips == null || videoClips.Length == 0)
        {
            Debug.LogWarning("No video clips assigned. Returning to Main.");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Main");
            yield break;
        }

        for (int i = 0; i < videoClips.Length; i++)
        {
            videoPlayer.clip = videoClips[i];
            videoPlayer.Prepare();
            // 준비될 때까지 대기
            while (!videoPlayer.isPrepared)
                yield return null;

            videoPlayer.Play();

            // 재생 종료까지 대기 (또는 loopPointReached 이벤트로도 처리)
            while (videoPlayer.isPlaying)
                yield return null;

            // 소량의 짧은 딜레이 (필요 시)
            yield return new WaitForSeconds(0.2f);
        }

        // 비디오 모두 재생 후 씬 전환
        SceneManager.LoadScene("MainScene");
    }

    // VideoPlayer.loopPointReached 이벤트 핸들러 (안전장치)
    void OnVideoFinished(VideoPlayer vp)
    {
        // 아무것도 안함 ? PlayVideosSequence 코루틴이 우선 사용됨
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
    }
}

