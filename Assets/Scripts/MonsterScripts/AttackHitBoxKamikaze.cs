

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBoxKamikaze : MonoBehaviour
{
    public KamikazeAttack kamikazeAttack;
    public float waitingForAttack = 3f;
    
    public Rigidbody2D enemyRb;
    public GameObject meleeHitbox;
    public float xGauche = 0f;
    public float xDroite = 1f;
    private Animator anim;
    public bool isAttacking;


    private Color tempColor;
    private Color newColor;
    private Color currentColor;
    public GameObject circleArea;
    private MonsterHealth monsterHealth;

    void Start()
    {
        enemyRb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<Animator>();
        isAttacking = false;
        kamikazeAttack = GetComponentInParent<KamikazeAttack>();  
        currentColor = GetComponentInParent<SpriteRenderer>().color;
        monsterHealth = GetComponentInParent<MonsterHealth>();
        
    }

    

    public void OnTriggerEnter2D(Collider2D coll)
    {                
        if(gameObject != null && gameObject.activeInHierarchy && coll.gameObject.CompareTag("Player"))
        {
            circleArea.SetActive(true);
            StartCoroutine(AttackSpe());         
        }
    }

    private IEnumerator AttackSpe()
    {
        
        isAttacking = true;
        /* tempColor = currentColor;
        Color.RGBToHSV(tempColor, out float h, out float s, out float v);
        s *= 0.25f;
        newColor = Color.HSVToRGB(h, s, v);
        currentColor = newColor; */
        anim.SetTrigger("SimpleAttackTrigger");
        yield return new WaitForSeconds(waitingForAttack);
        if(monsterHealth.isTakingDamage)
            yield return null;
        kamikazeAttack.Attack();
        isAttacking = false;
        yield return null;
        /* currentColor = tempColor; */
    }
}

