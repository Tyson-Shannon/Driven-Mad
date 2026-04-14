using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MileCounter : MonoBehaviour
{
    private float milesTraveled;
    private float carSpeed;

    [SerializeField] private CarController car;

    // Start is called before the first frame update
    void Start()
    {
        milesTraveled = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        carSpeed = car.GetSpeed();
        //add miles
        milesTraveled = milesTraveled + ((carSpeed) / 1000);
        Debug.Log("Miles: " + milesTraveled);

        //Barry 04/14/26
        ScoreManager.Instance.AddDistance((carSpeed / 1000) * Time.deltaTime,true);
    }
}
