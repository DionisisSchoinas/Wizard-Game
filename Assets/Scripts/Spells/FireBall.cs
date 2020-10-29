using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FireBall : Spell
{
    [SerializeField]
    private float damageExplosion = 50f;
    [SerializeField]
    private float damageOverTime = 1f;
    [SerializeField]
    private Vector3 scale;
    [SerializeField]
    private float damageTicksPerSecond = 5;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private float explosionToSizeRatio = 2f;
    [SerializeField]
    private GameObject burning;
    [SerializeField]
    private float burningToSizeRatio = 2f;

    private Transform firePoint;
    private Ray ray;
    private float sizeChange = 0.1f;
    private float sizeChangeTicksPerSecond = 10f;
    private bool allow = true;
    private bool allowDamage = true;
    private bool chargingUp = false;
    private bool explosive = true;

    private GameObject tmpFireBall;

    private Collider[] colliders;

    private void Start()
    {
        transform.localScale = scale;
    }

    private void FixedUpdate()
    {
        if (!explosive)
        {
            colliders = Physics.OverlapSphere(transform.position, transform.localScale.x / 2);
            if (allowDamage && colliders != null)
            {
                foreach (Collider other in colliders)
                {
                    if (other.CompareTag("Damageable"))
                        other.SendMessage("Damage", damageOverTime);
                }
                allowDamage = false;
                Invoke("DamageTimer", 1 / damageTicksPerSecond);
            }
        }
    }

    public override void SetFirePoint(Transform point)
    {
        firePoint = point;
    }

    public override void WakeUp()
    {
        Start();
    }

    public void SetExplosive(bool ex)
    {
        explosive = ex;
    }

    public override void FireSimple()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit))
        {
            GameObject g = Instantiate(gameObject, hit.point, firePoint.rotation);
            GameObject ex = Instantiate(explosion, g.transform);
            ex.transform.localScale = scale / explosionToSizeRatio;
            Destroy(g, 1f);
        }
    }

    public override void FireHold(bool holding)
    {
        if (!chargingUp)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit))
            {
                chargingUp = true;
                tmpFireBall = Instantiate(gameObject, hit.point, Quaternion.identity);
                tmpFireBall.GetComponent<FireBall>().SetExplosive(false);
                tmpFireBall.transform.localScale = new Vector3(2,2,2);
                GameObject ex = Instantiate(burning, tmpFireBall.transform);
                ex.transform.localScale = tmpFireBall.transform.localScale / burningToSizeRatio;
                ex.transform.localPosition = hit.normal * tmpFireBall.transform.localScale.x / 2;
            }
        }
        if (holding)
        {
            if (allow && chargingUp)
            {
                if (tmpFireBall.transform.localScale.magnitude < 40)
                    tmpFireBall.transform.localScale += sizeChange * Vector3.one;
                allow = false;

                Invoke("SizeTimer", 1 / sizeChangeTicksPerSecond);
            }
        }
        else
        {
            Destroy(tmpFireBall);
            allow = true;
            chargingUp = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damageable") && explosive)
        {
            other.SendMessage("Damage", damageExplosion);
        }
    }

    void SizeTimer()
    {
        allow = true;
    }
    void DamageTimer()
    {
        allowDamage = true;
    }
}
