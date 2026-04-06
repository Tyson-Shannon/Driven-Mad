using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private IDrivingSpeedStrategy driveStrategy;
    private IDrivingSteerStrategy steerStrategy;

    private float horizontalInput;
    private float steerAmount;
    private float speed;

    public void Initialize(IDrivingSpeedStrategy driveStrategy, IDrivingSteerStrategy steerStrategy)
    {
        this.driveStrategy = driveStrategy;
        this.steerStrategy = steerStrategy;

        steerAmount = steerStrategy.GetDriveSteer();
    }

    void Update()
    {
        HandleInput();
        HandleSteering();
    }

    void HandleInput()
    {
        //A or D/Left or Right
        horizontalInput = Input.GetAxis("Horizontal");
    }

    void HandleSteering()
    {
        transform.Translate(Vector3.right * horizontalInput * steerAmount * Time.deltaTime);
    }

    public float GetSpeed()
    {
        //called by RoadScroll
        speed = driveStrategy.GetDriveSpeed();
        return speed;
    }

    //SpeedUp PowerUp--
    public void ApplySpeedUp(float boostAmount, float duration)
    {
        StartCoroutine(SpeedUpRoutine(boostAmount, duration));
    }

    private IEnumerator SpeedUpRoutine(float boostAmount, float duration)
    {
        IDrivingSpeedStrategy original = driveStrategy;

        //wrap with decorator
        driveStrategy = new SpeedUpDecorator(original, boostAmount);

        yield return new WaitForSeconds(duration);

        //revert back
        driveStrategy = original;
    }
}
