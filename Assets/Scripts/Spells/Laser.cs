using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float damagePerFrame = 5f;

    private List<GameObject> collisions;

    private void Start()
    {
        collisions = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        foreach(GameObject gm in collisions)
        {
            gm.SendMessage("Damage", damagePerFrame);
        }
        collisions.Clear();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Damageable"))
        {
            if (!collisions.Contains(other))
                collisions.Add(other);
        }
    }
}