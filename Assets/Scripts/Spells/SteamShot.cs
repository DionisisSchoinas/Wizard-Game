using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamShot : Spell
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float range = 20f;
    [SerializeField]
    private GameObject firedSteam;

    private Transform firePoint;

    public override void SetFirePoint(Transform point)
    {
        firePoint = point;
    }

    public override void WakeUp()
    {
    }

    public override void FireSimple()
    {
        GameObject tmp = Instantiate(firedSteam, firePoint.position + firePoint.forward * 2f, firePoint.rotation) as GameObject;
        tmp.SendMessage("SetSpeed", speed);
        Destroy(tmp, 10f);
    }

    public override void FireHold(bool holding)
    {
    }
}
