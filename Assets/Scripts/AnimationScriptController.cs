using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class AnimationScriptController : MonoBehaviour
{
    public Animator animator;
    public CharacterController player;
    public Transform indicatorWheel;
    public float velocityZ,velocityX;
    public GameObject handForBasicSpells;

    private PlayerMovementScript controls;
    //public GameObject fireboltHand;
    public bool allowStopCasting;

    // Start is called before the first frame update
    void Start()
    {
        allowStopCasting = true;
        controls = GameObject.FindObjectOfType<PlayerMovementScript>() as PlayerMovementScript;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //===== Movement Animations ======
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

        if (!controls.canMove)
        {
            animator.SetTrigger("HardLanding");
        }

        //===== Spell Casting Animations ======
        if (!controls.casting && allowStopCasting)
        {
            //fireboltHand.SetActive(false);
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
            //fireboltHand.SetActive(true);
            animator.SetLayerWeight(1, 1);
            animator.SetFloat("Velocity Z", velocityZ);
            animator.SetFloat("Velocity X", velocityX);
        }
        
    }

    public void CastBasic(ParticleSystem source)
    {
        animator.SetTrigger("CastBasic");
        StartCoroutine(CastBasicAnimation(source));
    }

    public void CastChannel(bool hold)
    {
        /*
        if (controls.mousedown_2)
        {
            animator.SetBool("Chanelling", hold);
        }
        else if (wandScript.channeling)
        {
            animator.SetBool("Chanelling", false);
        }
        */
    }

    IEnumerator CastBasicAnimation(ParticleSystem source)
    {
        allowStopCasting = false;
        ParticleSystem tmp = Instantiate(source, handForBasicSpells.transform);
        tmp.transform.position += Vector3.left * 0.1f;
        yield return new WaitForSeconds(0.8f);
        Destroy(tmp.gameObject);
        yield return new WaitForSeconds(1.4f);
        allowStopCasting = true;
    }
}
