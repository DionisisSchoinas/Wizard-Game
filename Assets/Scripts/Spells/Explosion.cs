using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private float damage = 35f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damageable"))
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, other.transform.position, out hit))
            {
                if (hit.collider.CompareTag("Damageable"))
                {
                    other.SendMessage("Damage", damage);
                }
            }
        }
    }
}
