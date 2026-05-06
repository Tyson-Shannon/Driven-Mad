//Thierno Barry 05/05/2026
using TMPro;
using UnityEngine;

public class LevelSelectorUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI basicCarBest;
    [SerializeField] private TextMeshProUGUI muscleCarBest;
    [SerializeField] private TextMeshProUGUI vanBest;
    [SerializeField] private TextMeshProUGUI semiTruckBest;

    void Start()
    {
        basicCarBest.text = "Best: " + (PlayerPrefs.GetFloat("BasicCar_BestDistance", 0f) / 100f).ToString("F1") + " Miles";
        muscleCarBest.text = "Best: " + (PlayerPrefs.GetFloat("MuscleCar_BestDistance", 0f) / 100f).ToString("F1") + " Miles";
        vanBest.text = "Best: " + (PlayerPrefs.GetFloat("Van_BestDistance", 0f) / 100f).ToString("F1") + " Miles";
        semiTruckBest.text = "Best: " + (PlayerPrefs.GetFloat("SemiTruck_BestDistance", 0f) / 100f).ToString("F1") + " Miles";
    }
}