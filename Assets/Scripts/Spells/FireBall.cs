using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell
{
    [SerializeField]
    private float explosionDamage = 25f;
    [SerializeField]
    private float speed = 30f;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private Rigidbody rb;

    private bool allowSpawning;
    private Transform firePoint;

    private void Start()
    {
        allowSpawning = true;
    }

    public override void FireSimple()
    {
        GameObject tmp = Instantiate(gameObject, firePoint.position, firePoint.rotation) as GameObject;
        tmp.SendMessage("AllowSpawning", false);
        Destroy(tmp, 5f);
    }

    public override void SetFirePoint(Transform point)
    {
        firePoint = point;
    }

    public override void WakeUp()
    {
        Start();
    }

    public void AllowSpawning(bool al)
    {
        allowSpawning = false;
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject exp = Instantiate(explosion, transform.position + transform.forward * 0.2f, transform.rotation) as GameObject;
        exp.SendMessage("SetDamage", explosionDamage);
        Destroy(exp, 1f);
        Destroy(gameObject);
    }
    public override void FireHold(bool holding)
    {
    }
}
