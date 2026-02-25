using UnityEngine;

public interface IHealthSystem
{
    int CurrentHealth { get; }
    int MaxHealth { get; }
    bool IsAlive { get; }

    void TakeDamage(int damage);
    void Repair(int amount);
}

// STANDARD HEALTH - Normal car, balanced
public class StandardHealth : IHealthSystem
{
    private int currentHealth;
    private int maxHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsAlive => currentHealth > 0;

    public StandardHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log($"Standard car took {damage} damage. Health: {currentHealth}/{maxHealth}");
    }

    public void Repair(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Debug.Log($"Repaired {amount}. Health: {currentHealth}/{maxHealth}");
    }
}

// ARMORED HEALTH  reduced damage
public class ArmoredHealth : IHealthSystem
{
    private int currentHealth;
    private int maxHealth;
    private float armorReduction = 0.5f; // Takes only 50% damage

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsAlive => currentHealth > 0;

    public ArmoredHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        int reducedDamage = Mathf.RoundToInt(damage * armorReduction);
        currentHealth -= reducedDamage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log($"Armored car took {damage} (reduced to {reducedDamage}). Health: {currentHealth}/{maxHealth}");
    }

    public void Repair(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Debug.Log($"Repaired {amount}. Health: {currentHealth}/{maxHealth}");
    }
}

// FAST CAR HEALTH  takes MORE damage 
public class FastCarHealth : IHealthSystem
{
    private int currentHealth;
    private int maxHealth;
    private float damageMultiplier = 1.25f; // Takes 25% MORE damage

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsAlive => currentHealth > 0;

    public FastCarHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        int increasedDamage = Mathf.RoundToInt(damage * damageMultiplier);
        currentHealth -= increasedDamage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log($"Fast car took {damage} (increased to {increasedDamage}). Health: {currentHealth}/{maxHealth}");
    }

    public void Repair(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Debug.Log($"Repaired {amount}. Health: {currentHealth}/{maxHealth}");
    }
}

// SHIELD HEALTH 
public class ShieldHealth : IHealthSystem
{
    private int currentHealth;
    private int maxHealth;
    private int shieldHealth;
    private int maxShieldHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsAlive => currentHealth > 0;
    public int ShieldAmount => shieldHealth;

    public ShieldHealth(int currentHealth, int maxHealth, int shieldAmount)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
        this.maxShieldHealth = shieldAmount;
        this.shieldHealth = shieldAmount;
    }

    public void TakeDamage(int damage)
    {
        if (shieldHealth > 0)
        {
            shieldHealth -= damage;

            if (shieldHealth < 0)
            {
                // Shield broke, leftover damage goes to health
                currentHealth += shieldHealth; // shieldHealth is negative here
                if (currentHealth < 0) currentHealth = 0;
                Debug.Log($"Shield broke! Remaining damage to health. Health: {currentHealth}/{maxHealth}");
                shieldHealth = 0;
            }
            else
            {
                Debug.Log($"Shield absorbed {damage} damage. Shield: {shieldHealth}/{maxShieldHealth}");
            }
        }
        else
        {
            // No shield, direct damage to health
            currentHealth -= damage;
            if (currentHealth < 0) currentHealth = 0;
            Debug.Log($"Took {damage} damage. Health: {currentHealth}/{maxHealth}");
        }
    }

    public void Repair(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Debug.Log($"Repaired {amount}. Health: {currentHealth}/{maxHealth}");
    }
}