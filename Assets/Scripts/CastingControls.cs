using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;

public class CastingControls : MonoBehaviour
{
    [SerializeField]
    private Wand wand;
    [SerializeField]
    private OverlayController overlayController;

    private PlayerMovementScript controls;

    private bool fire1 = false;
    private bool fire2 = false;

    private void Start()
    {
        controls = GameObject.FindObjectOfType<PlayerMovementScript>() as PlayerMovementScript;
    }

    private void FixedUpdate()
    {
        if (controls.menu)
        {
            overlayController.Enable(true);
            fire1 = false;
            if (fire2)
            {
                fire2 = false;
                wand.Fire2(false);
            }
        }
        else if (overlayController.isEnabled)
        {
            overlayController.Enable(false);
        }

        if (!controls.menu)
        {
            fire1 = controls.mousedown_1;
            fire2 = controls.mousedown_2;

            if (!fire2 && Wand.channeling)
            {
                wand.Fire2(false);
            }
        }

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
