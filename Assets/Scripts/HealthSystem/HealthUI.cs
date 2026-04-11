//Tyson Shannon 2026-04-11

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    void OnEnable()
    {
        CarHealthManager.OnHealthChanged += UpdateHealthUI;
    }

    void OnDisable()
    {
        CarHealthManager.OnHealthChanged -= UpdateHealthUI;
    }

    void UpdateHealthUI(int current, int max)
    {
        //update slider
        healthBar.maxValue = max;
        healthBar.value = current;

        //update text
        if (healthText != null)
        {
            healthText.text = $"{current}";
        }
    }
}