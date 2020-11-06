using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private Rigidbody rb;

    private Transform firePoint;

    public override void FireSimple()
    {
        GameObject tmp = Instantiate(gameObject, firePoint.position, firePoint.rotation) as GameObject;
        Destroy(tmp, 5f);
    }

    public override void SetFirePoint(Transform point)
    {
        firePoint = point;
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject exp = Instantiate(explosion, transform.position + transform.forward * 0.2f, transform.rotation) as GameObject;
        Destroy(exp, 1f);
        Destroy(gameObject);
    }
    public override void FireHold(bool holding)
    {
    }
    public override void WakeUp()
    {
    }
}
