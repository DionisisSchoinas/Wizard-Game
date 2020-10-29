using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBar;

    public void SetMaxHealth(float health)
    {
        healthBar.maxValue = health;
    }

    public void SetHealth(float health)
    {
        healthBar.value = health;
    }
}
