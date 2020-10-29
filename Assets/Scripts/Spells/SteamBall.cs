using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamBall : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private GameObject smokeScreen;
    [SerializeField]
    private float smokeDuration = 10f;

    private float speed = 5f;
    private GameObject smoke;

    public void SetSpeed(float s)
    {
        speed = s;
    }

    void FixedUpdate()
    {
        rb.AddForce((transform.rotation * Vector3.forward) * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        smoke = Instantiate(smokeScreen, transform.position, Quaternion.identity);
        Invoke(nameof(StopParticles), smokeDuration);
        Destroy(smoke, smokeDuration+1);
        Destroy(gameObject);
    }

    void StopParticles()
    {
        smoke.GetComponent<ParticleSystem>().Stop();
    }
}
