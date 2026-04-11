using UnityEngine;

public class HealthTester : MonoBehaviour
{
    private CarHealthManager carHealth;

    void Start()
    {
        carHealth = GetComponent<CarHealthManager>();
    }

    void Update()
    {
        // Press SPACE to take 20 damage
        if (Input.GetKeyDown(KeyCode.Space))
        {
            carHealth.TakeDamage(20, CarHealthManager.DamageSource.Zombie);
            Debug.Log("SPACE pressed - took 20 damage!");
        }

        /*(now done by powerups
        // Press H to heal 30 HP
        if (Input.GetKeyDown(KeyCode.H))
        {
            carHealth.Repair(30);
            Debug.Log("H pressed - healed 30 HP!");
        }

        // Press S to activate shield
        if (Input.GetKeyDown(KeyCode.S))
        {
            carHealth.ActivateShield(50);
            Debug.Log("S pressed - shield activated (50 HP)!");
        }
        */
    }
}