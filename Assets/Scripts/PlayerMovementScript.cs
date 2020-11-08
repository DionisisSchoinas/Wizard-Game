using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController controller;
    public Transform indicatorWheel;
    public Transform groundCheck;
    public Transform Cylinder;
    public LayerMask groundMask;
    
    public float speed = 6f;
    public float maxRunSpeed = 12f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public float jumpHeight = 2f;
    public float rollDistance = 10f;
    public float smoothing = 0.1f;
    float smoothVelocity;
    public float runspeed = 0f;

    public Vector3 direction;
   
    Vector3 velocity;

    public bool isGrounded;
    public bool canMove;
    public bool casting;
    public bool mousedown_1;
    public bool mousedown_2;
    public bool menu;

    private float horizontal;
    private float vertical;
    private bool running;
    private bool jump;

    private void Start()
    {
        canMove = true;
        casting = false;
        mousedown_1 = false;
        mousedown_2 = false;
        menu = false;

        horizontal = 0f;
        vertical = 0f;
        running = false;
        jump = false;
    }

    private void Update()
    {
        //get horizontal and vertical axes
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        //pciking spell
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            menu = true;
            mousedown_1 = false;
            mousedown_2 = false;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            menu = false;
        }

        if (!menu)
        {
            //casting spells
            if (Input.GetMouseButtonDown(0) && !mousedown_2)
            {
                mousedown_1 = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                mousedown_1 = false;
            }

            if (Input.GetMouseButtonDown(1) && !mousedown_1)
            {
                mousedown_2 = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                mousedown_2 = false;
            }
        }

        running = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetButton("Jump");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //===================GROUND CHECK=================== 
        //check if its close to the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Reset the velocity 
        if (isGrounded && velocity.y < 0)
        {
            if (velocity.y <= -20)
            {
                StartCoroutine(stun(2f));
            }
            velocity.y = -2f;
        }

        //===================Movement=================== 
        if (canMove)
        {
            //calculate and normalize direction
            direction = (Quaternion.Euler(0, 45, 0) * new Vector3(horizontal, 0f, vertical).normalized);
          
        }
        else
        {
            direction = new Vector3(0f, 0f, 0f);
        }


        if (mousedown_1 || Wand.castingBasic)  // if mouse down OR if already firing basic
        {
            casting = true;
            if (canMove)
            {
                transform.rotation = indicatorWheel.rotation;
            }
        }
        else if (mousedown_2)  // if mouse down OR if already channeling
        {
            casting = true;
            if (canMove)
            {
                transform.rotation = indicatorWheel.rotation;
            }
        }
        else
        {
            casting = false;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, smoothing);
            if (direction != new Vector3(0, 0, 0)) { transform.rotation = Quaternion.Euler(0f, angle, 0f); }
        }
        
        //Handles Running
        if (running)
        { 
            if (runspeed < maxRunSpeed - speed)
            {
                runspeed += 5f * Time.deltaTime;
                if (runspeed > (maxRunSpeed - speed))
                {
                    runspeed = maxRunSpeed - speed;
                }
            }
        }
        else
        {
            if (runspeed > 0f)
            {
                runspeed -= 5f * Time.deltaTime;
                if (runspeed < 0)
                {
                    runspeed = 0;
                }
            }

        }

        //Move player towords direction
        controller.Move(direction * (speed+ runspeed) * Time.deltaTime);
        
        if (jump && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jump = false;
        }

        //apply gravity to velocity 
        velocity.y += gravity * Time.deltaTime;
        //apply velocity to player
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator stun(float second)
    {
        canMove = false;
        yield return new WaitForSeconds(second);
        canMove = true;
    }
}
