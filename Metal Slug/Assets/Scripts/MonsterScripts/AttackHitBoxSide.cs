using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBoxSide : MonoBehaviour
{
    public SpriteRenderer enemySpriteRenderer;
    public GameObject meleeHitbox;
    private PlayerHealth playerHealth;
    public int damage;
    public bool isAttacking;

    void Start()
    {
        
        //playerHealth = GetComponent<PlayerHealth>();
        isAttacking = false;
    }

    // Update est appelée une fois par frame
    void Update()
    {
        
        
        if (enemySpriteRenderer.flipX)
        {
            // Si le joueur est retourné, positionne la hitbox de mêlée à gauche du joueur
            meleeHitbox.transform.localPosition = new Vector3(-1f, 0f, 0f);
        }
        else
        {
            // Si le joueur n'est pas retourné, positionne la hitbox de mêlée à droite du joueur
            meleeHitbox.transform.localPosition = new Vector3(1f, 0f, 0f);
        }
    }

    public void  OnTriggerEnter2D(Collider2D coll)
    {
        
        if(coll.gameObject.CompareTag("Player"))
        {
            playerHealth = coll.gameObject.GetComponent<PlayerHealth>();
            
            isAttacking = true;
            StartCoroutine(EnemyAttack());
            
            
        }else {
            isAttacking = false;
        }
        
    }
    private IEnumerator EnemyAttack()
    {   
        
        yield return new WaitForSeconds(5);
        if(isAttacking == true)
        playerHealth.TakeDamage(damage);
        yield return null;
        isAttacking = false;
    }
}
