using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class FireWall : Spell
{
    [SerializeField]
    private float damage = 5f;
    [SerializeField]
    private float damageTicksPerSecond = 5f;
    [SerializeField]
    private float lengthBurst = 10f;

    private Transform firePoint;
    private bool allowDamage = true;
    private Collider[] colliders;
    private GameObject currentFireWall;
    private bool hold = false;

    private void Start()
    { 
    }

    private void FixedUpdate()
    {
        if (!hold)
            colliders = Physics.OverlapBox(transform.position, transform.localScale/2, transform.rotation);

        if (allowDamage && colliders != null)
        {
            allowDamage = false;
            foreach (Collider other in colliders)
            {
                if (other.CompareTag("Damageable"))
                    other.SendMessage("Damage", damage);
            }
            Invoke("DamageTimer", 1 / damageTicksPerSecond);
        }
    }

    public void SetHold(bool holding)
    {
        hold = holding;
    }

    public void SetColliders(Collider[] cols)
    {
        colliders = cols;
    }

    public override void SetFirePoint(Transform point)
    {
        firePoint = point;
    }

    public override void WakeUp()
    {
        Start();
    }

    public override void FireSimple()
    {
        RaycastHit hit;
        int layer = 1 << 8;
        layer = ~layer;
        if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit))
        {
            if (currentFireWall != null) Destroy(currentFireWall);

            if (hit.transform.CompareTag("Ground"))
            {
                Vector3 fwd = Camera.main.transform.forward;
                fwd.y = 0f;
                currentFireWall = Instantiate(gameObject, hit.normal * transform.localScale.y / 2 + hit.point, Quaternion.LookRotation(fwd));
                currentFireWall.transform.RotateAround(currentFireWall.transform.position, Vector3.up, 90);
            }
        }
    }

    public override void FireHold(bool holding)
    {/*
        if (holding)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit, 30f, 1 << 2))
            {
                if (hit.transform.CompareTag("FireWall"))
                {
                    //currentFireWall.transform.position += -hit.normal.normalized * Time.deltaTime * speed;
                    currentFireWall.SendMessage("SetHold", true);
                    colliders = Physics.OverlapBox((-hit.normal.normalized * lengthBurst) / 2 + currentFireWall.transform.position, (hit.normal.normalized.Abs() * lengthBurst + currentFireWall.transform.localScale) / 2f, currentFireWall.transform.rotation);
                    currentFireWall.SendMessage("SetColliders", colliders);
                    if (flamesSwap)
                    {
                        fireWallFlames.transform.position += (-hit.normal.normalized * lengthBurst) / 2f;
                        fireWallFlames.transform.localScale += hit.normal.normalized.Abs() * lengthBurst * 0.8f;
                        flamesSwap = false;
                    }
                }
            }
        }
        else
        {
            if (currentFireWall)
            {
                currentFireWall.SendMessage("SetHold", false);
                fireWallFlames.transform.position = currentFireWall.transform.position + Vector3.down;
                fireWallFlames.transform.localScale = currentFireWall.transform.localScale / 2;
                flamesSwap = true;
            }
        }
        */
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(Vector3.right * 5 + transform.position, (Vector3.right*10 + transform.localScale));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.up * 5 + transform.position, (Vector3.up * 10 + transform.localScale) );
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.forward * 5 + transform.position, (Vector3.forward * 10 + transform.localScale));
    }
    */
    void DamageTimer()
    {
        allowDamage = true;
    }
}
