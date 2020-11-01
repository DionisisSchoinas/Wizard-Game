using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Firewall : Spell
{
    private Transform firePoint;
    private GameObject currentFireWall;

    public override void SetFirePoint(Transform point)
    {
        firePoint = point;
    }


    public override void FireSimple()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit))
        {
            if (currentFireWall != null) Destroy(currentFireWall);

            if (hit.transform.CompareTag("Ground"))
            {
                Vector3 fwd = Camera.main.transform.forward;
                fwd.y = 0f;
                currentFireWall = Instantiate(gameObject, hit.normal * transform.localScale.y / 2 + hit.point, Quaternion.LookRotation(fwd));
            }
        }
    }

    public override void FireHold(bool holding)
    {
    }
    public override void WakeUp()
    {
    }
}