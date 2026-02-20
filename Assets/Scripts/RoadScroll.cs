//Tyson Shannon 2026-02-19

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScroll : MonoBehaviour
{
    //how fast road scrolls by
    private float carSpeed;

    //make sure the car is attached to the road and ground objects
    [SerializeField] private CarController car;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
        //get speed to move road from car type
        carSpeed = car.GetSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        //get ammount to offset texture times framerate
        float offset = Time.time * carSpeed;
        //move texture on plane to simulate movement
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
    }
}
