using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float damage;
    
    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

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
