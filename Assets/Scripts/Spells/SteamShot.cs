using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamShot : Spell
{
    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private GameObject firedSteam;

    private Transform simpleFirePoint;
    private Transform channelingFirePoint;

    private SpellIndicatorController indicatorController;

    public override void SetFirePoints(Transform point1, Transform point2)
    {
        simpleFirePoint = point1;
        channelingFirePoint = point2;
    }

    public override void FireSimple()
    {
        GameObject tmp = Instantiate(firedSteam, simpleFirePoint.position, simpleFirePoint.rotation) as GameObject;
        tmp.SendMessage("SetSpeed", speed);
        Destroy(tmp, 5f);
    }
    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    public override void FireHold(bool holding)
    {
    }

    public override void WakeUp()
    {
    }

    public override ParticleSystem GetSource()
    {
        return ((GameObject)Resources.Load("Spells/Default Smoke Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }
}
