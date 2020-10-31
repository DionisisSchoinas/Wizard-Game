using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebolt : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private Rigidbody rb;

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(Instantiate(explosion, transform.position + transform.forward * 0.2f, transform.rotation), 0.3f);
        Destroy(gameObject);
    }
}
