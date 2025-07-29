

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBoxShooting : MonoBehaviour
{
    public MonsterAttack2 monsterAttack2;
    public float waitingForAttack = 3f;
    
    public Rigidbody2D enemyRb;
    public GameObject meleeHitbox;
    public float xGauche = 0f;
    public float xDroite = 1f;
    private Animator anim;
    public bool isAttacking;
    public int direction;
    

    private Color tempColor;
    private Color newColor;
    private Color currentColor;

    void Start()
    {
        enemyRb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<Animator>();
        isAttacking = false;
        currentColor = GetComponentInParent<SpriteRenderer>().color;
    }


    public void OnTriggerEnter2D(Collider2D coll)
    {                
        if(coll.gameObject.CompareTag("Player") && !monsterAttack2.isShooting && gameObject.activeInHierarchy)
        {
            // StartCoroutine(AttackSpeShooting());         
        }
    }

    private IEnumerator AttackSpeShooting()
    {
        isAttacking = true;
        /* tempColor = currentColor;
        Color.RGBToHSV(tempColor, out float h, out float s, out float v);
        s *= 0.25f;
        newColor = Color.HSVToRGB(h, s, v);
        currentColor = newColor; */
        anim.SetTrigger("SimpleAttackTrigger");
        monsterAttack2.Attack();
        isAttacking = false;
        /* currentColor = tempColor; */
        yield return new WaitForSeconds(waitingForAttack);
        yield return null;
    }
}

