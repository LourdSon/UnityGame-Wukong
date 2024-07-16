using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Cinemachine;

public class PlayerHealth : MonoBehaviour
{

    public float maxHealth = 500;
    public float health;

    public UnityEngine.UI.Image healthBarFill;
    public UnityAction OnDeath;

    public float knockBackDuration = 0.5f;
    public float knockBackCounter;
    public bool isTakingDamage = false;

    private CinemachineImpulseSource impulseSource;

    
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        knockBackCounter = 0;
        isTakingDamage = false;

        impulseSource = GetComponent<CinemachineImpulseSource>();

        
    }

    // Update is called once per frame
    void Update()
    {
        DamageCheck();
    }


    public void TakeDamage(int damage)
    {
        

        if(!isTakingDamage)
        {
            knockBackF();
            health -= damage;
            UpdateHealthBar();
            health = Mathf.Clamp(health, 0, maxHealth);
            CameraShakeManager.instance.CameraShake(impulseSource);
        }
        if (health <= 0)
        {
            Die();
        }
    }

    public void knockBackF()
    {
        knockBackCounter = 0;  // Initialize the counter when knockback starts
        isTakingDamage = true;
    }
    public void DamageCheck()
    {
        if (isTakingDamage)
        {
            knockBackCounter += Time.deltaTime;
            if (knockBackCounter >= knockBackDuration)
            {
                isTakingDamage = false;
                knockBackCounter = 0;
            }
        }
    }

    public void UpdateHealthBar()
    {
        float targetFillAmount = health / maxHealth;
        healthBarFill.fillAmount = targetFillAmount;
    }

    public void Die()
    {
        if (OnDeath != null)
        {
            Destroy(gameObject);
            OnDeath.Invoke();
        }
    }
}
