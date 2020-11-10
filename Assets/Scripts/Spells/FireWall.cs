using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Firewall : Spell
{
    private Transform simpleFirePoint;
    private Transform channelingFirePoint;
    private GameObject currentFireWall;
    private Vector3 spawningLocation;
    private Vector3 spellRotation;
    private bool pickedSpot;
    private SpellAOE aoe;

    private void Start()
    {
        currentFireWall = Instantiate(gameObject) as GameObject;
        currentFireWall.SetActive(false);
        pickedSpot = false;
    }

    public override void SetFirePoints(Transform point1, Transform point2)
    {
        simpleFirePoint = point1;
        channelingFirePoint = point2;
    }

    public override void FireSimple()
    {
        if (pickedSpot)
        {
            aoe.DestroyIndicator();
            pickedSpot = false;
            currentFireWall.transform.position = Vector3.up * transform.localScale.y / 2 + spawningLocation;
            currentFireWall.transform.eulerAngles = spellRotation;
            currentFireWall.SetActive(true);
            Invoke(nameof(DeactivateWall), 10f);
        }
    }

    public override void FireHold(bool holding)
    {
        if (!currentFireWall.activeInHierarchy)
        {
            if (holding)
            {
                aoe = FindObjectOfType<SpellAOE>();
                aoe.SelectLocation(20f, 24f, 4f);
                pickedSpot = false;
            }
            else
            {
                if (aoe != null)
                {
                    spawningLocation = aoe.LockLocation()[0];
                    spellRotation = aoe.LockLocation()[1];
                    pickedSpot = true;
                    Invoke(nameof(CancelSpell), 5f);
                }
            }
        }
    }

    private void DeactivateWall()
    {
        currentFireWall.SetActive(false);
    }

    public override void WakeUp()
    {
        Start();
    }

    private void CancelSpell()
    {
        aoe.DestroyIndicator();
        pickedSpot = false;
    }

    public override ParticleSystem GetSource()
    {
        return ((GameObject)Resources.Load("Spells/Default Fire Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }
}