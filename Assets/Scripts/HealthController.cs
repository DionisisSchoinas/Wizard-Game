using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private bool invunarable = false;

    public bool respawn = false;

    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void Damage(float damage)
    {
        if (!invunarable)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                if (!respawn)
                    Destroy(gameObject);
                else
                {
                    currentHealth = maxHealth;
                    healthBar.SetHealth(maxHealth);
                }
            }
        }
    }
}
