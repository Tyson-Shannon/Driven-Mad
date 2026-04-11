//Tyson Shannon 2026-04-11

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private CarController car;

    void Update()
    {
        UpdateSpeedUI(car.GetSpeed()*100f);
    }

    void UpdateSpeedUI(float current)
    {
        //update text
        if (speedText != null)
        {
            speedText.text = $"{current} mph";
        }
    }
}