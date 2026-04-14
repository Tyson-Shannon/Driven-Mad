using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFacade : MonoBehaviour
{
    [SerializeField] private CarController car;
    [SerializeField] private CarHealthManager carHealth;
    
    public void Die()
    {
        car.SetIsAlive(false);//set speed to 0
        carHealth.ChangeHealth(0);//set health to 0
        carHealth.Die(CarHealthManager.DamageSource.Obstacle);//stop car steer movement

        // Barry 04/14/26
        ScoreManager.Instance.SaveHighScore();
        ScoreManager.Instance.ResetRun();
    }
}
