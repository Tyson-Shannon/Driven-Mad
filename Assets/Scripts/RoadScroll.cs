//Tyson Shannon 2026-02-19, 2026=03-31

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScroll : MonoBehaviour
{
    //how fast road scrolls by
    private float carSpeed;
    //timer set to zero
    private float timer = 0f;
    //delay before speed increase
    private float speedDelay = 60f;
    //speed increment
    private float speedIncrement = 0.1f;

    //make sure the car is attached to the road and ground objects
    [SerializeField] private CarController car;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        carSpeed = car.GetSpeed();
        //add time since last update
        timer += Time.deltaTime;
        //specified time has passed to increase speed
        if (timer >= speedDelay)
        {
            timer = 0f;
            car.UpdateSpeed(speedIncrement);
        }

        //get ammount to offset texture times framerate
        float offset = Time.time * carSpeed;
        //move texture on plane to simulate movement
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
    }
}
