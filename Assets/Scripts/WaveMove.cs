using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMove : MonoBehaviour
{
    // Simple script to start the wave off slow but build speed slowly
    // and then stop accelerating when it has hit a max speed
    public float startSpeed = 1.0f;
    public float accelerationRate = 0.1f;
    public float maxSpeed = 5.0f;
    
    private float currentSpeed;
    
    void Start()
    {
        currentSpeed = startSpeed;

        // I had some weird mesh rendering issues early on but figured out
        // it was probuilder being weird about some mesh stuff
        Renderer mesh = gameObject.GetComponent<Renderer>();
        mesh.enabled = true;
    }
    
    void FixedUpdate()
    {
        // The calculations used to accelerate the wave
        currentSpeed += accelerationRate * Time.deltaTime;

        currentSpeed = Mathf.Clamp(currentSpeed, startSpeed, maxSpeed);

        transform.Translate(Vector3.forward * (currentSpeed * Time.deltaTime), Space.World);
    }
    
}
