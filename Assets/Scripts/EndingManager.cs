using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    public Image endingImage;
    public Text endingText;

    public Sprite goodEndingSprite;
    public Sprite neutralEndingSprite;
    public Sprite badEndingSprite;

    void Start()
    {
        int health = 0;
        if (PlayerStatusManager.Instance != null)
            health = PlayerStatusManager.Instance.health;

        // 예: health 기준으로 분기 (임의 값, 필요하면 조정)
        if (health >= 20)
        {
            endingImage.sprite = goodEndingSprite;
            endingText.text = "무사히 도움을 받았어요!\n(좋은 엔딩 메시지)";
        }
        else if (health > 0)
        {
            endingImage.sprite = neutralEndingSprite;
            endingText.text = "위험했지만 구조되었어요.\n(중립 엔딩 메시지)";
        }
        else
        {
            endingImage.sprite = badEndingSprite;
            endingText.text = "위험한 상황이 발생했어요.\n(교육적 문구 포함)";
        }
    }
}
