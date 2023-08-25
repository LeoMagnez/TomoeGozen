using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesHP : MonoBehaviour
{
    [Header("HP")]
    public float currentHealth;
    public float maxHealth;
    [SerializeField] FloatingHealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.UpdateHealthBar(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        healthBar.UpdateHealthBar(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
