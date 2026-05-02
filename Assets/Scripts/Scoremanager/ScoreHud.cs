//Thierno Barry 04/14/2026 Observer Pattern
using TMPro;
using UnityEngine;
public class ScoreHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI bestDistanceText;
    [SerializeField] private TextMeshProUGUI distanceText; // DistanceText from Canvas

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

    void Update()
    {
        distanceText.text = ScoreManager.Instance.GetCurrentDistance().ToString("F1") + " Miles";
    }

    void UpdateScoreUI(int currentScore)
    {
        currentScoreText.text = "Score: " + currentScore;
    }
}