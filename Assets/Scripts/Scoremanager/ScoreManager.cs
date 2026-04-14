//Thierno Barry 04/14/2026 Singleton Pattern
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Current run
    private float currentDistance;
    private int zombiesKilled;
    private float offRoadDistance;
    private string currentCarType;

    // High scores (per car)
    private float bestDistance;
    private int bestScore;

    public static ScoreManager Instance { get; private set; }



    public void SetCarType(string carType)
    {
        currentCarType = carType;
        LoadHighScore();
    }

    void LoadHighScore()
    {
        bestDistance = PlayerPrefs.GetFloat(currentCarType + "_BestDistance", 0f);
        bestScore = PlayerPrefs.GetInt(currentCarType + "_BestScore", 0);
    }

    public void SaveHighScore()
    {
        if (currentDistance > bestDistance)
            PlayerPrefs.SetFloat(currentCarType + "_BestDistance", currentDistance);

        if (CalculateScore() > bestScore)
            PlayerPrefs.SetInt(currentCarType + "_BestScore", CalculateScore());

        PlayerPrefs.Save();
    }

    public void AddDistance(float distance, bool isOnRoad)
    {
        currentDistance += distance;
        if (!isOnRoad)
            offRoadDistance += distance;
    }

    public int CalculateScore()
    {
        int distanceScore = (int)(currentDistance * 10);
        int zombieScore = zombiesKilled * 50; // placeholder until Simon is done
        int offRoadPenalty = (int)(offRoadDistance * 5); // penalty for going off road

        return distanceScore + zombieScore - offRoadPenalty;
    }

    public void AddZombieKill()
    {
        zombiesKilled += 1;
    }

    public void ResetRun()
    {
        zombiesKilled = 0;
        currentDistance = 0;
        offRoadDistance = 0;
    }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

  
}