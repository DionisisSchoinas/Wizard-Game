using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FireRay : Spell
{
    [SerializeField]
    private float wandDamage = 10f;
    [SerializeField]
    private float wandRange = 50f;
    [SerializeField]
    private float damageTicksPerSecond = 5;
    [SerializeField]
    private GameObject laser;


    private GameObject tmpLaser;
    public bool instantiateLaser;

    private Transform firePoint;
    private Ray ray;
    private bool allowDamage;
    private bool resize;
    private bool hitting;
    private bool createBeam = true;
    private bool createBeamHitting = true;

    void Start()
    {
        instantiateLaser = true;
        Reset();
    }

    public override void WakeUp()
    {
        Start();
    }

    public override void SetFirePoint(Transform point)
    {
        firePoint = point;
    }
    
    public override void FireSimple()
    {
    }

    public override void FireHold(bool holding)
    {
        if (instantiateLaser)
        {
            tmpLaser = Instantiate(laser, firePoint) as GameObject;
            tmpLaser.SetActive(true);
            instantiateLaser = false;
        }
        else
        {
            if (holding)
            {
                UpdatePosition();
                Damage();
            }
            else
            {
                Disable();
                instantiateLaser = true;
                Reset();
            }
        }
    }

    void UpdatePosition()
    {
        tmpLaser.SetActive(true);
    }

    private void Disable()
    {
        tmpLaser.SetActive(false);
    }

    private void Reset()
    {
        resize = true;
        hitting = false;
        allowDamage = true;
    }

    void Damage()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit, wandRange))
        {
            if (hit.collider.CompareTag("Damageable") && allowDamage)
            {
                allowDamage = false;
                hit.collider.SendMessage("Damage", wandDamage);
                Invoke("DamageTimer", 1 / damageTicksPerSecond);
            }
            tmpLaser.transform.localScale = Vector3.forward * (hit.point - firePoint.position).magnitude;
        }
        else
        {
            tmpLaser.transform.localScale = Vector3.forward * Mathf.Abs(wandRange);
        }
    }

    void DamageTimer()
    {
        allowDamage = true;
    }
}
