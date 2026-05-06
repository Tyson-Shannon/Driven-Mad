//Thierno Barry 04/14/2026 Singleton Pattern
using System;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{

    //Event manager 
    public static event Action<int> OnScoreChanged;
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
        OnScoreChanged?.Invoke(CalculateScore()); // notify HUD
        //Debug.Log("CarType: " + currentCarType + " Distance: " + currentDistance + " Score: " + CalculateScore());
    }

    public int CalculateScore()
    {
        int distanceScore = (int)(currentDistance * 100);
        int zombieScore = zombiesKilled * 50;
        int offRoadPenalty = (int)(offRoadDistance * 5);
        return distanceScore + zombieScore - offRoadPenalty;
    }

    public void AddZombieKill()
    {
        zombiesKilled++;
        OnScoreChanged?.Invoke(CalculateScore()); // notify HUD
    }

    public void ResetRun()
    {
        zombiesKilled = 0;
        currentDistance = 0;
        offRoadDistance = 0;
    }

    // Getters for HUD
    public int GetCurrentScore() => CalculateScore();
    public float GetCurrentDistance() => currentDistance;
    public float GetBestDistance() => bestDistance;
    public int GetBestScore() => bestScore;

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