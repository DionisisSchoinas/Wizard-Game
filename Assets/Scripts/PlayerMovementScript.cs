using System;
using System.Collections;
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
    public bool canMove = true;
    public bool lockOn = false;

   
    // Update is called once per frame
    void FixedUpdate()
    {


        //===================GROUND CHECK=================== 
        //check if its close to the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Reset the velocity 
        Debug.Log(velocity.y);
        if (isGrounded && velocity.y < 0)
        {
            if (velocity.y <= -20)
            {
                StartCoroutine(stun(2f));
            }

            velocity.y = -2f;
        }

        //===================Movement=================== 

        //get horizontal and vertical axes
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

       

        if (canMove)
        {
            //calculate and normalize direction
            direction = (Quaternion.Euler(0, 45, 0) * new Vector3(horizontal, 0f, vertical).normalized);
          
        }
        else
        {
            direction = new Vector3(0f, 0f, 0f);
        }

        

        if (Input.GetMouseButton(0))
        {
            lockOn = true;
            if (canMove)
            {
                transform.rotation = indicatorWheel.rotation;
            }
        }
        else
        {
            lockOn = false;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, smoothing);
            if (direction != new Vector3(0, 0, 0)) { transform.rotation = Quaternion.Euler(0f, angle, 0f); }
        }
        


        //Handles Running
        if (Input.GetKey(KeyCode.LeftShift))
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
        

      
       
        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
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
