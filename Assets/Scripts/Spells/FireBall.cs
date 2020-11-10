using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fireball : Spell
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private Rigidbody rb;

    private Transform simpleFirePoint;
    private Transform channelingFirePoint;

    private SpellIndicatorController indicatorController;

    public override void FireSimple()
    {
        GameObject tmp = Instantiate(gameObject, simpleFirePoint.position, simpleFirePoint.rotation) as GameObject;
        Destroy(tmp, 5f);
    }

    public override void SetFirePoints(Transform point1, Transform point2)
    {
        simpleFirePoint = point1;
        channelingFirePoint = point2;
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
    public override ParticleSystem GetSource()
    {
        GameObject tmp = Instantiate(gameObject, Vector3.up * 1000, Quaternion.identity) as GameObject;
        Destroy(tmp, 0.1f);
        return tmp.transform.Find("Source").GetComponent<ParticleSystem>();
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
}
