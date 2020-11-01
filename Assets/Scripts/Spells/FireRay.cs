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
    private Transform firePoint;

    void Start()
    {
        tmpLaser = Instantiate(laser, firePoint) as GameObject;
        tmpLaser.SetActive(false);
    }

    public override void WakeUp()
    {
        Start();
    }

    public override void SetFirePoint(Transform point)
    {
        firePoint = point;
    }
    
    public override void FireHold(bool holding)
    {
        if (holding)
        {
            tmpLaser.SetActive(true);
        }
        else
        {
            tmpLaser.SetActive(false);
        }
    }

    public override void FireSimple()
    {
    }
}