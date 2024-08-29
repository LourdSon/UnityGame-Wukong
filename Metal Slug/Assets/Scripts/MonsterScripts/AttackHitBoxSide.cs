

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBoxSide : MonoBehaviour
{
    public MonsterAttack2 monsterAttack2;
    public float waitingForAttack = 3f;
    
    public SpriteRenderer spriteRenderer;
    public GameObject meleeHitbox;
    public float xGauche = 0f;
    public float xDroite = 1f;
    private Animator anim;
    public bool isAttacking;

    void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        anim = GetComponentInParent<Animator>();
        isAttacking = false;
    }

    // Update est appelée une fois par frame
    void Update()
    {
        monsterAttack2 = GetComponentInParent<MonsterAttack2>(); 
        if (spriteRenderer.flipX)
        {
            // Si le joueur est retourné, positionne la hitbox de mêlée à gauche du joueur
            meleeHitbox.transform.localPosition = new Vector3(xGauche, 0f, 0f);
        }
        else
        {
            // Si le joueur n'est pas retourné, positionne la hitbox de mêlée à droite du joueur
            meleeHitbox.transform.localPosition = new Vector3(xDroite, 0f, 0f);
        }
        
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {                
        if(coll.gameObject.CompareTag("Player"))
        {
            StartCoroutine(AttackSpe());         
        }
    }

    private IEnumerator AttackSpe()
    {
        isAttacking = true;
        anim.SetTrigger("SimpleAttackTrigger");
        yield return new WaitForSeconds(waitingForAttack);
        monsterAttack2.Attack();
        yield return null;
        isAttacking = false;
    }
}

