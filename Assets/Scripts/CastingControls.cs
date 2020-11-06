using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CastingControls : MonoBehaviour
{
    [SerializeField]
    private Wand wand;
    [SerializeField]
    private OverlayController overlayController;

    private bool fire1 = false;
    private bool fire2 = false;
    private bool menu = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            menu = true;
            overlayController.Enable(true);
            fire1 = false;
            if (fire2)
            {
                fire2 = false;
                wand.Fire2(false);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            menu = false;
            overlayController.Enable(false);
        }


        if (!menu)
        {
            if (Input.GetMouseButtonDown(0))
            {
                fire1 = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                fire1 = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                fire2 = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                wand.Fire2(false);
                fire2 = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (fire1)
        {
            wand.Fire1();
            fire1 = false;
        }
        if (fire2)
        {
            wand.Fire2(true);
        }
    }
}
