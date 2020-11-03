using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebolt : Spell
{
    [SerializeField]
    private float damage = 15f;
    [SerializeField]
    private float speed = 8f;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private GameObject explosionParticles;

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
        if (collision.transform.CompareTag("Damageable"))
            collision.transform.SendMessage("Damage", damage);
        Destroy(Instantiate(explosionParticles, transform.position, transform.rotation), 1f);
        Destroy(gameObject);
    }

    public override void FireHold(bool holding)
    {
    }
}
