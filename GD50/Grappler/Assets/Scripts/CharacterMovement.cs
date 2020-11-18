using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // player scripts and bodies
    private Rigidbody playerBody;
    public Transform orientation;
    private static GroundCheck gCheck;

    private static CrouchHeadCheck headCheck;

    // get gun for scaling with crouching
    private static GrapplingGun gGun;

    public Transform gun;

    // movement
    public float playerGravity = 35f;
    public float moveSpeed = 4500f; 
    public float currentMaxSpeed;
    public float currentSpeed;
    public float maxSpeed = 10f;
    public float swingMaxSpeed = 25f;
    public float maxAirSpeed = 5f;
    public float airSpeed = 1500f;
    public float crouchSpeed = 4f;
    public float jumpHeight = 150f;
    public bool onGround;
    public LayerMask levelGround;
    public float counterMovement = 0.175f;
    private float threshold = 0.01f;

    // player size
    private float normPlayerHeight = 1.5f;
    private float crouchPlayerHeight = 0.75f;

    // gun size
    private float gunSize = 0.075f;

    //playerInput
    private float x, z;
    private bool jumping, crouching;

    private void Awake() => playerBody = GetComponent<Rigidbody>();

    private void Start() {
        gCheck = GetComponentInChildren<GroundCheck>();
        headCheck = GetComponentInChildren<CrouchHeadCheck>();
        gGun = GetComponentInChildren<GrapplingGun>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    // Update is called once per frame
    private void Update() 
    {
        playerInput();
        ClampPlayerSpeed();
    }
    private void FixedUpdate()
    {
        if (crouching)
        {
            Crouch();
            currentMaxSpeed = crouchSpeed;
            Movement();
        }   
        else
        {
            if (!headCheck.IsHeadClearance())
            {
                UnCrouch();
            }
            if (gCheck.IsGrounded())
            {
                currentMaxSpeed = maxSpeed;
                Movement();
            }
            else
            {
                currentMaxSpeed = maxAirSpeed;
                Movement();
            }
        }
        if (jumping)
        {
            jump();
        }

    }

    private void Movement()
    {  
        //Set max speed
        float maxSpeed = this.currentMaxSpeed;

        // add extra gravity to the player
        playerBody.AddForce(Vector3.down * Time.deltaTime * playerGravity);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (gCheck.IsGrounded())
        {
            currentSpeed = moveSpeed;
            // Counteract sliding and sloppy movement
            CounterMovement(x, z, mag);

            // check whether adding speed will bring player over max speed
            if (x > 0 && xMag > maxSpeed) x = 0;
            if (x < 0 && xMag < -maxSpeed) x = 0;
            if (z > 0 && yMag > maxSpeed) z = 0;
            if (z < 0 && yMag < -maxSpeed) z = 0;
        }
        else
        {
            currentSpeed = airSpeed;

            // only if there is no rope limit air move speed
            if (!gGun.IsRope())
            {
                if (x > 0 && xMag > maxSpeed) x = 0;
                if (x < 0 && xMag < -maxSpeed) x = 0;
                if (z > 0 && yMag > maxSpeed) z = 0;
                if (z < 0 && yMag < -maxSpeed) z = 0;
            }
        }

        //Apply forces to playerBody
        playerBody.AddForce(orientation.transform.forward * z * currentSpeed * Time.deltaTime);
        playerBody.AddForce(orientation.transform.right * x * currentSpeed * Time.deltaTime);
    }

    private void jump()
    {
        if (gCheck.IsGrounded())
        {
            playerBody.AddForce(orientation.transform.up * jumpHeight);
        }
    }

    private void playerInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.LeftControl);
    }

    public Vector2 FindVelRelativeToLook() 
    {
        // players current forward angle
        float lookAngle = orientation.transform.eulerAngles.y;
        // players angle of movement with 0 being forward
        float moveAngle = Mathf.Atan2(playerBody.velocity.x, playerBody.velocity.z) * Mathf.Rad2Deg;

        // finds the relative velocity angle compared to the moveAngle
        float velY = Mathf.DeltaAngle(lookAngle, moveAngle);
        // the x velocity angle is just 90 degrees away
        float velX = 90 - velY;


        // multuply the magnitude by the angle to get magnitude in each direction
        float magnitude = playerBody.velocity.magnitude;
        float yMag = magnitude * Mathf.Cos(velY * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(velX * Mathf.Deg2Rad);
        
        // return directional magnitude
        return new Vector2(xMag, yMag);
    }

    private void CounterMovement(float x, float y, Vector2 mag) 
    {
        //Counter movement
        if (Mathf.Abs(mag.x) > threshold && Mathf.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) 
        {
            playerBody.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) 
        {
            playerBody.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
        
        // Limit the speed of diagonal running to the maxSpeed
        if (Mathf.Sqrt((Mathf.Pow(playerBody.velocity.x, 2) + Mathf.Pow(playerBody.velocity.z, 2))) > currentMaxSpeed) 
        {
            float tempFallspeed = playerBody.velocity.y;
            Vector3 normSpeed = playerBody.velocity.normalized * currentMaxSpeed;
            playerBody.velocity = new Vector3(normSpeed.x, tempFallspeed, normSpeed.z);
        }
    }

    private void Crouch()
    {
        transform.localScale = new Vector3(1, crouchPlayerHeight, 1);
        gun.transform.localScale = new Vector3(gunSize, gunSize * 2, gunSize);
    }

    private void UnCrouch()
    {
        transform.localScale = new Vector3(1, normPlayerHeight, 1);
        gun.transform.localScale = new Vector3(gunSize, gunSize, gunSize);
    }

    private void ClampPlayerSpeed()
    {
        // Limit the player swing speed to the max airspeed
        if (Mathf.Sqrt((Mathf.Pow(playerBody.velocity.x, 2) + Mathf.Pow(playerBody.velocity.z, 2))) > swingMaxSpeed) 
        {
            float tempFallspeed = playerBody.velocity.y;
            Vector3 normSpeed = playerBody.velocity.normalized * swingMaxSpeed;
            playerBody.velocity = new Vector3(normSpeed.x, tempFallspeed, normSpeed.z);
        }
    }


}