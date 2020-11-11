using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Fireray : Spell
{
    [SerializeField]
    private float damageTicksPerSecond = 5;
    [SerializeField]
    private GameObject laser;

    private GameObject tmpLaser;
    private Transform simpleFirePoint;
    private Transform channelingFirePoint;

    private SpellIndicatorController indicatorController;

    void Start()
    {
        tmpLaser = Instantiate(laser, channelingFirePoint) as GameObject;
        tmpLaser.SetActive(false);
    }

    public override void WakeUp()
    {
        Start();
    }

    public override void SetFirePoints(Transform point1, Transform point2)
    {
        simpleFirePoint = point1;
        channelingFirePoint = point2;
    }

    public override void FireHold(bool holding)
    {
        if (holding)
        {
            indicatorController.SelectLocation(channelingFirePoint, 3f, 18f);
            tmpLaser.SetActive(true);
        }
        else
        {
            indicatorController.DestroyIndicator();
            tmpLaser.SetActive(false);
        }
    }

    public override void FireSimple()
    {
    }

    public override ParticleSystem GetSource()
    {
        return tmpLaser.transform.Find("Source").GetComponent<ParticleSystem>();
    }
    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }
}