using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public class CarHealthManager : MonoBehaviour
{
    // OBSERVER PATTERN 
    public static event Action<int, int> OnHealthChanged; // (currentHealth, maxHealth)
    public static event Action<DamageSource> OnCarDestroyed;

    // STRATEGY PATTERN 
    private IHealthSystem healthSystem;

    // Blood overlay 
    private Sprite bloodSplash;
    private GameObject damageOverlay;
    private bool isDead = false; // prevents fading after death

    public enum DamageSource
    {
        Zombie,
        Obstacle,
        Unknown
    }

    void Start()
    {
        // Load blood sprite from Resources folder
        bloodSplash = Resources.Load<Sprite>("blood");
        Debug.Log("Loaded sprite: " + bloodSplash);

        InitializeHealthSystem();
    }

    void InitializeHealthSystem()
    {
        // STRATEGY PATTERN - detect car type and assign health strategy
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
            healthSystem = new VanCarHealth(110);
            Debug.Log("Van initialized - 110 HP, normal damage");
        }
        else
        {
            // Fallback if no SetUp script found
            healthSystem = new StandardHealth(100);
            Debug.LogWarning("No car SetUp script detected - using default StandardHealth (100 HP)");
        }

        // OBSERVER PATTERN - notify listeners of initial health
        OnHealthChanged?.Invoke(healthSystem.CurrentHealth, healthSystem.MaxHealth);
    }

    public void TakeDamage(int damage, DamageSource source = DamageSource.Unknown)
    {
        // Already dead, ignore damage
        if (!healthSystem.IsAlive) return;

        healthSystem.TakeDamage(damage);

        // OBSERVER PATTERN - notify listeners of health change
        OnHealthChanged?.Invoke(healthSystem.CurrentHealth, healthSystem.MaxHealth);

        // Show blood and start fade (only if not dead)
        if (healthSystem.IsAlive)
        {
            ShowBlood();
            StopAllCoroutines();
            StartCoroutine(FadeBloodOut());
        }

        if (!healthSystem.IsAlive)
        {
            Die(source);
        }
    }

    public void Repair(int amount)
    {
        healthSystem.Repair(amount);

        OnHealthChanged?.Invoke(healthSystem.CurrentHealth, healthSystem.MaxHealth);

        // Fade blood away when healed
        StartCoroutine(FadeBloodOut());
    }

    public void ActivateShield(int shieldAmount)
    {
        int currentHealth = healthSystem.CurrentHealth;
        int maxHealth = healthSystem.MaxHealth;

        healthSystem = new ShieldHealth(currentHealth, maxHealth, shieldAmount);

        Debug.Log($"Shield activated! {shieldAmount} shield HP");

        OnHealthChanged?.Invoke(healthSystem.CurrentHealth, healthSystem.MaxHealth);
    }

    public void Die(DamageSource source)
    {
        Debug.Log($"=== CAR DESTROYED by {source}! ===");

        isDead = true; // mark as dead so blood doesn't fade

        StopAllCoroutines();
        OnCarDestroyed?.Invoke(source);

        // Show blood permanently on death
        ShowBlood();

        // Disable car
        DisableCar();

        // Show game over after 1 second delay
        Invoke("ShowGameOver", 1f);
    }

    void DisableCar()
    {
        // Disable car movement
        CarController carController = GetComponent<CarController>();
        if (carController != null)
        {
            carController.enabled = false;
            Debug.Log("Car controller disabled");
        }

        Debug.Log("Car disabled");
    }

    void ShowBlood()
    {
        // If overlay doesn't exist yet, create it once
        if (damageOverlay == null)
        {
            // Create overlay container
            damageOverlay = new GameObject("DamageOverlay");

            // Add canvas so Unity treats this as UI
            Canvas canvas = damageOverlay.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000; // renders on top of everything

            // Add image and assign blood sprite
            UnityEngine.UI.Image bloodImage = damageOverlay.AddComponent<UnityEngine.UI.Image>();
            bloodImage.sprite = bloodSplash;
            bloodImage.color = new Color(1f, 1f, 1f, 0.8f);

            // Stretch image to fill entire screen
            RectTransform rect = damageOverlay.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
        else
        {
            // Overlay exists, turn it on and reset transparency
            damageOverlay.SetActive(true);
            UnityEngine.UI.Image img = damageOverlay.GetComponent<UnityEngine.UI.Image>();
            if (img != null) img.color = new Color(1f, 1f, 1f, 0.8f);
        }
    }

    IEnumerator FadeBloodOut()
    {
        // Wait 1 second before fading
        yield return new WaitForSeconds(1f);

        // If car died during wait
        if (isDead) yield break;

        // If overlay doesn't exist stop coroutine
        if (damageOverlay == null) yield break;

        // Get image component to change transparency
        UnityEngine.UI.Image img = damageOverlay.GetComponent<UnityEngine.UI.Image>();
        if (img == null) yield break;

        // Slowly reduce transparency every frame
        float alpha = 0.8f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * 0.5f;
            img.color = new Color(1f, 1f, 1f, alpha);
            yield return null; // wait one frame then continue
        }

        // Fully faded, hide overlay
        damageOverlay.SetActive(false);
    }

    void ShowGameOver()
    {
        Debug.Log("=== GAME OVER ===");
        // TODO: add game over UI here
        // TODO: display final score from ScoreManager
    }

    public int GetCurrentHealth() => healthSystem.CurrentHealth;
    public int GetMaxHealth() => healthSystem.MaxHealth;
    public bool IsAlive() => healthSystem.IsAlive;

    //VISITOR PATTERN - shield
    public void Accept(IPowerUpVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void ChangeHealth(int health)
    {
        OnHealthChanged?.Invoke(health, healthSystem.MaxHealth);
    }
}