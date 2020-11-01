using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamBall : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private GameObject smokeScreen;

    private float speed = 5f;

    public void SetSpeed(float s)
    {
        speed = s;
    }

    void FixedUpdate()
    {
        rb.AddForce((transform.rotation * Vector3.forward) * Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(Instantiate(smokeScreen, transform.position, transform.rotation), 10f);
        Destroy(gameObject);
    }
}
