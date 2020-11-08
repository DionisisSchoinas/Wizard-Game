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

    private void Start()
    {
        currentFireWall = Instantiate(gameObject) as GameObject;
        currentFireWall.SetActive(false);
    }

    public override void SetFirePoints(Transform point1, Transform point2)
    {
        simpleFirePoint = point1;
        channelingFirePoint = point2;
    }

    public override void FireSimple()
    {
        if (!currentFireWall.activeInHierarchy)
        {
            RaycastHit hit;
            if (Physics.Raycast(simpleFirePoint.position, simpleFirePoint.TransformDirection(Vector3.forward), out hit))
            {
                if (hit.transform.CompareTag("Ground"))
                {
                    Vector3 fwd = Camera.main.transform.forward;
                    fwd.y = 0f;
                    currentFireWall.transform.position = hit.normal * transform.localScale.y / 2 + hit.point;
                    currentFireWall.transform.rotation = Quaternion.LookRotation(fwd);
                    currentFireWall.SetActive(true);
                    Invoke(nameof(DeactivateWall), 10f);
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
    public override void FireHold(bool holding)
    {
    }

    public override ParticleSystem GetSource()
    {
        return ((GameObject)Resources.Load("Spells/Default Fire Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }
}