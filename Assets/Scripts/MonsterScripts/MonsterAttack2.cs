

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAttack2 : MonoBehaviour
{
    public Vector2 detectionPosition;
    public float detectionRadius;
    public LayerMask playerLayerMask;
    public SpriteRenderer enemySpriteRenderer;
    public Rigidbody2D enemyRb;
    public float selfForceMagnitudeForward2;
    public float forceMagnitudeForward2;
    public int damage = 10;
    public Vector2 detectionOffset = new Vector2(10, 0);
    public int direction;
    public Animator anim;
    public GameObject energyPrefab;
    public float energyBallSpeed;
    public bool isShooting;
    public Rigidbody2D playerRb;
    public float WaitBetweenShots;
    public ObjectPool objectPool;



    private Collider2D[] colliders;
    private Vector2 directionVector;
    private PlayerHealth playerHealth;
    private Transform myTransform;
    

    



    // Start is called before the first frame update
    void Start()
    {
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        enemyRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myTransform = transform;
    }


    public void Attack()
    {
        anim.SetTrigger("SimpleAttackTriggerFollow");
        direction = myTransform.rotation.y == 0f ? 1 : -1;
        detectionPosition = (Vector2)myTransform.position + Vector2.right * direction * detectionOffset;
        colliders = Physics2D.OverlapCircleAll(detectionPosition, detectionRadius, playerLayerMask);
        if (colliders.Length >= 1)
        {
            direction = myTransform.rotation.y == 0f ? 1 : -1;
            //playerRb.velocity = new Vector2(diagonal.x * selfForceMagnitudeForward,playerRb.velocity.y);
            enemyRb.AddForce(Vector2.right * selfForceMagnitudeForward2 * -direction, ForceMode2D.Impulse);
        }
        // Appliquer une force pour projeter les ennemis vers l'avant
        foreach (Collider2D collider in colliders)
        {
            playerRb = collider.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                directionVector = ((Vector2)playerRb.transform.position - (Vector2)myTransform.position).normalized;
                playerRb.AddForce(directionVector * forceMagnitudeForward2, ForceMode2D.Impulse);
                //playerRb.AddForce(Vector2.right * -selfForceMagnitudeForward, ForceMode2D.Impulse);

                playerHealth = collider.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(damage);
            }
        }
    }



    private void OnDrawGizmosSelected()
    {
        // Dessiner le rayon de détection dans l'éditeur
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(detectionPosition, detectionRadius);
    }

}

