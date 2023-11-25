using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMove : MonoBehaviour
{
    public float startSpeed = 1.0f;
    public float accelerationRate = 0.1f;
    public float maxSpeed = 5.0f;
    
    private float currentSpeed;
    
    void Start()
    {
        currentSpeed = startSpeed;
    }
    
    void FixedUpdate()
    {
        currentSpeed += accelerationRate * Time.deltaTime;

        currentSpeed = Mathf.Clamp(currentSpeed, startSpeed, maxSpeed);

        transform.Translate(Vector3.forward * (currentSpeed * Time.deltaTime), Space.World);    
    }
    
}
