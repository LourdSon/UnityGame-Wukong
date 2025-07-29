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
    public GameObject HealCharging;
    public GameObject HitEffect;
    public float newAlpha = 0.25f;


    private float targetFillAmount;
    private GameObject comicBoom;
    private Light lighter;
    private Transform myTransform;
    private Color tempColor;
    private Color newColor;
    private Color currentColor;
    public GameObject healUpEffect;
    private GameObject healUp;
    private Animator animator;
    public PlayerScore playerScore;
    public FinalScore finalScore;

    
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
        myTransform = transform;
        currentColor = GetComponentInParent<SpriteRenderer>().color;
        animator = GetComponent<Animator>();
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
            animator.SetTrigger("DamageTrigger");
            knockBackF();
            health -= damage;
            UpdateHealthBar();
            health = Mathf.Clamp(health, 0, maxHealth);
            CameraShakeManager.instance.CameraShake(impulseSource);
            StartCoroutine(DestroyHitEffect());
            StartCoroutine(PlayerBlinking());
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
        targetFillAmount = health / maxHealth;
        healthBarFill.fillAmount = targetFillAmount;
        
    }

    public void Die()
    {
        
        if (OnDeath != null)
        {
            finalScore.BestScore();
            Destroy(gameObject);
            OnDeath.Invoke();
        }
    }
    public void GetMyHealthBack()
    {
        if (PlayerController.instance.playerInputActions.Player.Heal.triggered && playerKi.currentKi >= healCost && health != maxHealth && !isHealing)
        {
            StartCoroutine(WaitBeforeHeal(secondsBeforeHeal));
            animator.SetBool("IsCharging", true);
            animator.SetTrigger("ChargingTrigger");
            
        }
    }
    public IEnumerator WaitBeforeHeal(float sec)
    {
        isHealing = true;
        StartCoroutine(DestroyHealUpEffect());
        HealCharging.gameObject.SetActive(true);
        yield return new WaitForSeconds(sec);
        if(playerScore != null)
        playerScore.healed += 1;
        animator.SetBool("IsCharging", false);
        HealCharging.gameObject.SetActive(false);
        health += 10;
        UpdateHealthBar();
        health = Mathf.Clamp(health, 0, maxHealth);
        playerKi.currentKi -= healCost;
        playerKi.UpdateKiBar();
        isHealing = false;
        
    }

    public IEnumerator DestroyHitEffect()
    {
        comicBoom = Instantiate(HitEffect,new Vector2(myTransform.position.x, myTransform.position.y + 2f), Quaternion.identity);
        lighter = comicBoom.GetComponentInChildren<Light>();
        yield return new WaitForSeconds(0.3f);
        Destroy(comicBoom);
        Destroy(lighter);
        
        yield return null;
    }

    public IEnumerator DestroyHealUpEffect()
    {
        healUp = Instantiate(healUpEffect,new Vector2(myTransform.position.x, myTransform.position.y + 2f), Quaternion.identity);
        lighter = healUp.GetComponentInChildren<Light>();
        yield return new WaitForSeconds(0.3f);
        Destroy(healUp);
        Destroy(lighter);
        
        yield return null;
    }
    
    private IEnumerator PlayerBlinking()
    {
        tempColor = currentColor;
        Color.RGBToHSV(tempColor, out float h, out float s, out float v);
        newColor = Color.HSVToRGB(h, s, v);
        newColor.a = newAlpha;
        currentColor = newColor;
        yield return new WaitForSeconds(knockBackDuration/12);
        currentColor = tempColor;
        yield return new WaitForSeconds(knockBackDuration/12);
        currentColor = newColor;
        yield return new WaitForSeconds(knockBackDuration/12);
        currentColor = tempColor;
        yield return new WaitForSeconds(knockBackDuration/12);
        currentColor = tempColor;
        yield return new WaitForSeconds(knockBackDuration/12);
        currentColor = newColor;
        yield return new WaitForSeconds(knockBackDuration/12);
        currentColor = tempColor;
        yield return new WaitForSeconds(knockBackDuration/12);
        currentColor = tempColor;
        yield return new WaitForSeconds(knockBackDuration/12);
        currentColor = newColor;
        yield return new WaitForSeconds(knockBackDuration/12);
        currentColor = tempColor;
        yield return new WaitForSeconds(knockBackDuration/12);
        currentColor = tempColor;
        yield return new WaitForSeconds(knockBackDuration/12);
        currentColor = newColor;
        yield return new WaitForSeconds(knockBackDuration/12);
        currentColor = tempColor;
        currentColor = tempColor;
        yield return null;
    }
    
        //tileDestroyer.DestructionMouse();
        
}
