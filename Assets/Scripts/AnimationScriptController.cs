using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class AnimationScriptController : MonoBehaviour
{
    Animator animator;
   
    public CharacterController player;
    public Transform indicatorWheel;
    public float velocityZ,velocityX;
    public Wand wandScript;
    PlayerMovementScript controls;
    public GameObject fireboltHand;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controls = GameObject.FindObjectOfType<PlayerMovementScript>() as PlayerMovementScript;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
      
        Vector3 direction = controls.direction;
        
        if (controls.isGrounded)
        {
            animator.SetBool("IsGrounded", true);
        }
        else
        {
            animator.SetBool("IsGrounded", false);
        }
       
        direction = Quaternion.Euler(0, -indicatorWheel.eulerAngles.y, 0) * direction;
        float velocityRun= ( controls.runspeed) / ( controls.maxRunSpeed- controls.speed);
        velocityZ = (direction.z+  velocityRun * direction.z);
        velocityX = (direction.x + velocityRun * direction.x);

        if (!controls.lockOn)
        {
            fireboltHand.SetActive(false);
            animator.SetLayerWeight(1, 0);
            if (direction != new Vector3(0f, 0f, 0f))
            {
                animator.SetFloat("Velocity X", 0f);
                animator.SetFloat("Velocity Z", 1 + velocityRun);
            }
            else
            {
                animator.SetFloat("Velocity Z", 0f);
                animator.SetFloat("Velocity X", 0f);
            }
        }
        else
        {
            fireboltHand.SetActive(true);
            animator.SetLayerWeight(1, 1);
            animator.SetFloat("Velocity Z", velocityZ);
            animator.SetFloat("Velocity X", velocityX);
        }

        if (controls.mousedown_1)
        {
            animator.SetTrigger("CastBasic");
            StartCoroutine(cast());
        }

        if (controls.mousedown_2)
        {
            animator.SetBool("Chanelling", true);
        }
        else if (wandScript.channeling)
        {
            animator.SetBool("Chanelling", false);
        }


        if (!controls.canMove)
        {
            Debug.Log("test");
            animator.SetTrigger("HardLanding");
        }
        
    }
    IEnumerator cast()
    {
        yield return new WaitForSeconds(0.7f);
        fireboltHand.SetActive(false);
        yield return new WaitForSeconds(1f);
        fireboltHand.SetActive(true);
    }
}
