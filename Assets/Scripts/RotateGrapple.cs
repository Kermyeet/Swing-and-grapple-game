using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGrapple : MonoBehaviour
{
    // This is also a simple script from Dani's Tutorial that turns the grappling gun to face the rope
    public Grappling grappling;
    private Quaternion desiredRotation;
    private float rotationSpeed = 5f;

    private void Awake()
    {
        grappling = gameObject.GetComponentInChildren<Grappling>();
    }

    void Update() {
        if (!grappling.IsGrappling()) {
            desiredRotation = transform.parent.rotation;
        }
        else {
            desiredRotation = Quaternion.LookRotation(grappling.GetGrapplePoint() - transform.position);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }
}
