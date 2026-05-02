//Thierno Barry 04/14/2026 Observer Pattern
using TMPro;
using UnityEngine;
public class ScoreHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI bestDistanceText;

    void OnEnable()
    {
        ScoreManager.OnScoreChanged += UpdateScoreUI;
    }

    void OnDisable()
    {
        ScoreManager.OnScoreChanged -= UpdateScoreUI;
    }

    void Start()
    {
        bestScoreText.text = "Best Score: " + ScoreManager.Instance.GetBestScore();
        bestDistanceText.text = "Best: " + ScoreManager.Instance.GetBestDistance().ToString("F1") + " Miles";
    }

    void UpdateScoreUI(int currentScore)
    {
        currentScoreText.text = "Score: " + currentScore;
    }
}