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
        speed = driveStrategy.GetDriveSpeed();
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
        return speed;
    }
}
