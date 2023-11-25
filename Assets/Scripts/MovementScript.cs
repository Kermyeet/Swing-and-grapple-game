using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour
{
    public Transform playerCam, orientation;

    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    private Rigidbody rb;

    private float desiredX;

    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;

    //only for the start of the map
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    float x, y;
    private bool jumping;

    private Text scoreText;
    private int score= 0;

    private Text speedText;
    private float speed;

    public Canvas UI;
    public Canvas deathScreen;
    public Canvas startScreen;
    
    public Text deadScore;

    private void Awake()
    {
        Time.timeScale = 1;
        
        rb = gameObject.GetComponent<Rigidbody>();

        scoreText = GameObject.Find("Score").GetComponent<Text>();

        speedText = GameObject.Find("Speed").GetComponent<Text>();
        
        UI.gameObject.SetActive(false);
    }

    void Start()
    {
        //found something that should keep the cursor in the game during testing
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        scoreText.text = "Score: " + score;
        
        Invoke("startGameTimer", 3f);
    }

    private void FixedUpdate()
    {
        Movement();
        
        speed = rb.velocity.magnitude;
    }

    void Update()
    {
        MyInput();
        Look();
        if (speed < 0.1f)
        {
            speed = 0;
        }
        speedText.text = "Speed: " + speed.ToString("F1");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wave"))
        {
            Time.timeScale = 0;
            
            deathScreen.gameObject.SetActive(true);
            UI.gameObject.SetActive(false);

            deadScore.text = score.ToString();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            score++;
            
            scoreText.text = "Score: " + score;
        }

        if (other.CompareTag("Wave"))
        {
            Time.timeScale = 0;
            
            deathScreen.gameObject.SetActive(true);
            UI.gameObject.SetActive(false);

            deadScore.text = score.ToString();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            Debug.Log(other.gameObject.name);
        }
    }

    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
    }

    private void Movement()
    {
        rb.AddForce(Physics.gravity * (Time.deltaTime * 10));

        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        CounterMovement(x, y, mag);

        if (readyToJump && jumping) Jump();

        float maxSpeed = this.maxSpeed;

        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        float multiplier = 1f, multiplierV = 1f;

        // Movement in air
        if (!grounded)
        {
            multiplier = 0.25f;
            multiplierV = 0.25f;
        }

        rb.AddForce(orientation.transform.forward * (y * moveSpeed * Time.deltaTime * multiplier * multiplierV));
        rb.AddForce(orientation.transform.right * (x * moveSpeed * Time.deltaTime * multiplier));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Jump()
    {
        if (grounded && readyToJump)
        {
            readyToJump = false;

            rb.AddForce(Vector2.up * (jumpForce * 1.5f));

            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Look()
    {
        if (!deathScreen.gameObject.activeSelf)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

            Vector3 rot = playerCam.transform.localRotation.eulerAngles;
            desiredX = rot.y + mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
            orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
            

        }
    }

    //CounterMovement is mainly used for more realistic swinging
    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) ||
            (mag.x > threshold && x < 0))
        {
            rb.AddForce(orientation.transform.right * (moveSpeed * Time.deltaTime * -mag.x * counterMovement));
        }

        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) ||
            (mag.y > threshold && y < 0))
        {
            rb.AddForce(orientation.transform.forward * (moveSpeed * Time.deltaTime * -mag.y * counterMovement));
        }
    }

    //Function that is supposed to help get rid of the slidiness of players
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }
    
    private bool cancellingGrounded;
    
    private void OnCollisionStay(Collision other) {
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        for (int i = 0; i < other.contactCount; i++) {
            Vector3 normal = other.contacts[i].normal;
            if ((whatIsGround & (1 << other.gameObject.layer)) != 0) {
                grounded = true;
                cancellingGrounded = false;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        float delay = 3f;
        if (!cancellingGrounded) {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }
    
    private void StopGrounded() {
        grounded = false;
    }

    void startGameTimer()
    {
        startScreen.gameObject.SetActive(false);
        
        UI.gameObject.SetActive(true);
    }

}
