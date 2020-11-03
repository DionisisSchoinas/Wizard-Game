using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 rotation;
    [SerializeField]
    private Vector3[] randomRotation;
    [SerializeField]
    private float meteorHitDamage;
    [SerializeField]
    private GameObject meteorHitEffect;
    [SerializeField]
    private Rigidbody rb;

    private void Start()
    {
        if (randomRotation.Length >= 2)
        {
            for (int i=0; i < 3; i++)
            {
                rotation[i] += Random.Range(randomRotation[0][i], randomRotation[1][i]);
            }
        }
        transform.rotation = Quaternion.LookRotation(rotation);
    }

    private void FixedUpdate()
    {
        transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 45f);
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damageable"))
        {
            other.transform.SendMessage("Damage", meteorHitDamage);
        }
        Destroy(Instantiate(meteorHitEffect, transform.position, transform.rotation), 1f);
        Destroy(gameObject);
    }
}
