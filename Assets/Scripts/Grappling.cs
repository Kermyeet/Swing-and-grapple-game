using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grappling : MonoBehaviour
{
    // These are all the necessary variables to get the grappling gun to work
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    private Rigidbody rigidbody1;


    // This is just a UI image piece that gets activated when the grapple can be shot,
    // could have just been an image or whatever, but this was early on
    public GameObject canHit;

    // These variables are to get the reference from the camera position instead of guns position to grapple
    public new Transform camera;
    
    // Grapple pull speed was added to give the player more ability to gain momentum and swing power
    public float grapplePullSpeed = 10f;
    
    void Awake()
    {
        rigidbody1 = player.GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
    }

    void Update() {
        // Pretty self explanatory, just checks if mouse 1 is clicked
        if (Input.GetMouseButtonDown(0)) {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0)) {
            StopGrapple();
        }

        // This is used to mainly check if the distance between the camera and the grapple object
        // and sets the red cross hair to true
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            canHit.SetActive(true);    
        }
        else
        {
            canHit.SetActive(false);
        }

        // This checks that if the joint made from grappling is not null, it will add the pull power properly
        if (joint != null) {
            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
            
            Vector3 grappleDirection = (grapplePoint - player.position).normalized;
            
            rigidbody1.AddForce(grappleDirection * grapplePullSpeed);
            
            // This updates how long the grapple line is, like it only gets smaller and can't get longer
            joint.maxDistance = Mathf.Min(distanceFromPoint * 0.8f, maxDistance);
        }
    }

    //Called after Update, as was done in Dani's Tutorial and only works if the grapple has been shot
    void LateUpdate() {
        DrawRope();
    }
    
    void StartGrapple() {
        // Process used to make the basic grappling work
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)) {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }
    
    void StopGrapple() {
        // Gets rid of any joint or rope that could be on screen
        lr.positionCount = 0;
        Destroy(joint);
    }

    // The whole process below is used to make sure the rope is not drawn from the players camera
    // and instead from the gun so it doesn't hinder the view
    private Vector3 currentGrapplePosition;

    void DrawRope() {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling() {
        return joint != null;
    }

    // This is used for the script Rotate Grapple
    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }
}
