using UnityEngine;
using System;
using UnityEngine.UI;

public class CarHealthManager : MonoBehaviour
{
    // OBSERVER PATTERN 
    public static event Action<int, int> OnHealthChanged; // (currentHealth, maxHealth)
    public static event Action<DamageSource> OnCarDestroyed;

    // STRATEGY PATTERN 
    private IHealthSystem healthSystem;

    public enum DamageSource
    {
        Zombie,
        Obstacle,
        Unknown
    }

    void Start()
    {
        InitializeHealthSystem();
    }

    void InitializeHealthSystem()
    {
        // STRATEGY PATTERN Detect car type and assign appropriate health strategy

        if (GetComponent<BasicCarSetUp>() != null)
        {
            healthSystem = new StandardHealth(100);
            Debug.Log("BasicCar initialized - 100 HP, normal damage");
        }
        else if (GetComponent<MuscleCarSetUp>() != null)
        {
            healthSystem = new ArmoredHealth(75);
            Debug.Log("MuscleCar initialized - 75 HP, 50% damage reduction");
        }
        else if (GetComponent<SemiTruckSetUp>() != null)
        {
            healthSystem = new ArmoredHealth(150);
            Debug.Log("SemiTruck initialized - 150 HP, 50% damage reduction");
        }
        else if (GetComponent<VanSetUp>() != null)
        {
            healthSystem = new FastCarHealth(50);
            Debug.Log("Van initialized - 50 HP, 25% increased damage taken");
        }
        else
        {
            // Fallback if no SetUp script found
            healthSystem = new StandardHealth(100);
            Debug.LogWarning("No car SetUp script detected - using default StandardHealth (100 HP)");
        }

        // OBSERVER PATTERN Notify listeners of initial health
        OnHealthChanged?.Invoke(healthSystem.CurrentHealth, healthSystem.MaxHealth);
    }


    public void TakeDamage(int damage, DamageSource source = DamageSource.Unknown)
    {
        if (!healthSystem.IsAlive) return; // Already dead, ignore damage

        healthSystem.TakeDamage(damage);

        // OBSERVER PATTERN Notification
        OnHealthChanged?.Invoke(healthSystem.CurrentHealth, healthSystem.MaxHealth);


        if (!healthSystem.IsAlive)
        {
            Die(source);
        }
    }


    public void Repair(int amount)
    {
        healthSystem.Repair(amount);

        OnHealthChanged?.Invoke(healthSystem.CurrentHealth, healthSystem.MaxHealth);
    }
    public void ActivateShield(int shieldAmount)
    {

        int currentHealth = healthSystem.CurrentHealth;
        int maxHealth = healthSystem.MaxHealth;

        healthSystem = new ShieldHealth(currentHealth, maxHealth, shieldAmount);

        Debug.Log($"Shield activated! {shieldAmount} shield HP");

        OnHealthChanged?.Invoke(healthSystem.CurrentHealth, healthSystem.MaxHealth);
    }

    void Die(DamageSource source)
    {
        Debug.Log($"=== CAR DESTROYED by {source}! ===");

        OnCarDestroyed?.Invoke(source);

        // Visual feedback and disable car
        DisableCar();

        // Show game over after 1 second delay
        Invoke("ShowGameOver", 1f);
    }

    void DisableCar()
    {
        // Disable car movement (your teammate's CarController)
        CarController carController = GetComponent<CarController>();
        if (carController != null)
        {
            carController.enabled = false;
            Debug.Log("Car controller disabled");
        }

        // Flash screen red to indicate damage/death
        FlashScreenRed();

        Debug.Log("Car disabled - screen flashed red");
    }

    void FlashScreenRed()
    {
        // Find or create a red overlay UI
        GameObject overlay = GameObject.Find("DamageOverlay");

        if (overlay == null)
        {
            // Create red overlay if it doesn't exist
            overlay = new GameObject("DamageOverlay");
            Canvas canvas = overlay.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000; // Make sure it's on top

            UnityEngine.UI.Image redImage = overlay.AddComponent<UnityEngine.UI.Image>();
            redImage.color = new Color(1f, 0f, 0f, 0.5f); // Red with 50% transparency

            RectTransform rectTransform = overlay.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            Debug.Log("Created red damage overlay");
        }
        else
        {
            // If it already exists, just make sure it's visible
            overlay.SetActive(true);
        }
    }
    void ShowGameOver()
    {
        Debug.Log("=== GAME OVER ===");
        // TODO:  add game over UI here
        // TODO: Display final score from ScoreManager
        // For now, just log to console
    }


    public int GetCurrentHealth() => healthSystem.CurrentHealth;
    public int GetMaxHealth() => healthSystem.MaxHealth;
    public bool IsAlive() => healthSystem.IsAlive;
}