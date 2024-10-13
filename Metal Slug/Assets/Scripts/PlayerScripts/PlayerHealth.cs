using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using System.Collections;

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
    public float fade = 1f;
    Material material;

    public float healCost = 25f;
    private PlayerMovement playerKi;
    private PlayerAttack playerAttack;
    public bool isHealing;
    public float secondsBeforeHeal = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        knockBackCounter = 0;
        isTakingDamage = false;

        impulseSource = GetComponent<CinemachineImpulseSource>();
        material = GetComponent<SpriteRenderer>().material;

        playerKi = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        isHealing = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        material.SetFloat("_Fade", fade);
        DamageCheck();
        GetMyHealthBack();
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
            Physics2D.IgnoreLayerCollision(9,11,true);
            knockBackCounter += Time.deltaTime;
            if (knockBackCounter >= knockBackDuration)
            {
                isTakingDamage = false;
                knockBackCounter = 0;
                Physics2D.IgnoreLayerCollision(9,11,false);
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
    public void GetMyHealthBack()
    {
        if (PlayerController.instance.playerInputActions.Player.Heal.triggered && playerKi.currentKi >= healCost && health != maxHealth && !isHealing)
        {
            StartCoroutine(WaitBeforeHeal(secondsBeforeHeal));
            
        }
    }
    public IEnumerator WaitBeforeHeal(float sec)
    {
        isHealing = true;
        yield return new WaitForSeconds(sec);
        health += 10;
        UpdateHealthBar();
        health = Mathf.Clamp(health, 0, maxHealth);
        playerKi.currentKi -= healCost;
        playerKi.UpdateKiBar();
        isHealing = false;
        
    }
    
    
}
