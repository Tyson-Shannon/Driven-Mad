//Thierno Barry 05/05/2026
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI finalDistanceText;

    void OnEnable()
    {
        CarHealthManager.OnCarDestroyed += ShowGameOver;
    }

    void OnDisable()
    {
        CarHealthManager.OnCarDestroyed -= ShowGameOver;
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    void ShowGameOver(CarHealthManager.DamageSource source)
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SaveHighScore();
            finalScoreText.text = "Score: " + ScoreManager.Instance.GetCurrentScore();
            bestScoreText.text = "Best: " + ScoreManager.Instance.GetBestScore();
            finalDistanceText.text = "Distance: " + (ScoreManager.Instance.GetCurrentDistance()/1000f).ToString("F1");
        }
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ResetRun();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ResetRun();
        SceneManager.LoadScene("LevelSelector");
    }
}
