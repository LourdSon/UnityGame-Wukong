

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KamikazeAttack : MonoBehaviour
{
    public Vector2 detectionPosition;
    public float detectionRadius;
    public LayerMask playerLayerMask;
    public SpriteRenderer enemySpriteRenderer;
    public Rigidbody2D enemyRb;
    public float forceMagnitudeForward2;
    public int damage = 10;
    public Vector2 detectionOffset = new Vector2(10, 0);
    public int direction;
    public Animator anim;

    private Rigidbody2D playerRb;
    private Rigidbody2D enemyRbOthers;
    private PlayerHealth playerHealth;
    private MonsterHealth monsterHealth;
    private float kamikazeDamageHimself;
    private Quaternion rotation;
    public ParticleSystem explosionParticles;
    // Start is called before the first frame update
    void Start()
    {
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        enemyRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        monsterHealth = GetComponent<MonsterHealth>();
        kamikazeDamageHimself = monsterHealth.maxHealth/2;
        rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void Attack()
    {
        anim.SetTrigger("SimpleAttackTriggerFollow");
        direction = enemySpriteRenderer.flipX ? -1 : 1;
        detectionPosition = (Vector2)transform.position + Vector2.right * direction * detectionOffset;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionPosition, detectionRadius, playerLayerMask);
        Instantiate(explosionParticles,transform.position, rotation);
        if (colliders.Length >= 1)
        {
            direction = enemySpriteRenderer.flipX ? -1 : 1;
            //playerRb.velocity = new Vector2(diagonal.x * selfForceMagnitudeForward,playerRb.velocity.y);
            //enemyRb.AddForce(Vector2.right * selfForceMagnitudeForward2 * -direction, ForceMode2D.Impulse);
        }
        // Appliquer une force pour projeter les ennemis vers l'avant
        foreach (Collider2D collider in colliders)
        {
            if(collider.gameObject.CompareTag("Player"))
            {
                playerRb = collider.GetComponent<Rigidbody2D>();
                playerHealth = collider.GetComponent<PlayerHealth>();
            }
            if(collider.gameObject.CompareTag("Enemy"))
            {
                enemyRbOthers = collider.GetComponent<Rigidbody2D>();
            }
         
            if (playerRb != null)
            {
                Vector2 directionVector = ((Vector2)playerRb.transform.position - (Vector2)transform.position).normalized;
                playerRb.AddForce(directionVector * forceMagnitudeForward2, ForceMode2D.Impulse);
                //playerRb.AddForce(Vector2.right * -selfForceMagnitudeForward, ForceMode2D.Impulse);
                
                
                playerHealth.TakeDamage(damage);
                
                
            }
            if (enemyRbOthers != null)
            {
                Vector2 directionVector = ((Vector2)enemyRbOthers.transform.position - (Vector2)transform.position).normalized;
                enemyRbOthers.AddForce(directionVector * forceMagnitudeForward2, ForceMode2D.Impulse);
            }
        }
        monsterHealth.TakeDamage(kamikazeDamageHimself);
    }


    private void OnDrawGizmosSelected()
    {
        // Dessiner le rayon de détection dans l'éditeur
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(detectionPosition, detectionRadius);
    }

}

