using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreIndicator : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] TextMeshProUGUI textDisplay;
    [SerializeField] TextFollower textFollower;

    const string scoredText = "+";
    const string missedText = "-";
    const float animDuration = 0.2f;

    void Start()
    {
        rectTransform.DOScale(0, 0);
        textFollower.onScoreChanged += UpdateScoreIndicator;
    }

    void UpdateScoreIndicator(int score)
    {
        textDisplay.text = score > 0 ? scoredText : missedText;
        textDisplay.color = score > 0 ? Color.green : Color.red;
        rectTransform.DOScale(1, animDuration).OnComplete(() => rectTransform.DOScale(0, animDuration));
    }
}
