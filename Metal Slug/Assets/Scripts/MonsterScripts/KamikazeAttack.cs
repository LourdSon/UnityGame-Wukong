

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



    private Collider2D[] colliders;
    private Vector2 directionVector;
    private Transform myTransform;
    public GameObject ComicBoomEffect;
    private GameObject comicBoom;
    private Light lighter;
    public GameObject circleArea;
    

    
    void Start()
    {
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        enemyRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        monsterHealth = GetComponent<MonsterHealth>();
        kamikazeDamageHimself = monsterHealth.maxHealth/2;
        rotation = Quaternion.Euler(0f, 0f, 0f);
        myTransform = transform;
        circleArea.SetActive(false);
    }


    public void Attack()
    {
        anim.SetTrigger("SimpleAttackTriggerFollow");
        direction = myTransform.rotation.y == 0f ? 1 : -1;
        detectionPosition = (Vector2)myTransform.position + Vector2.right * direction * detectionOffset;
        colliders = Physics2D.OverlapCircleAll(detectionPosition, detectionRadius, playerLayerMask);
        
        Instantiate(explosionParticles, myTransform.position, rotation);
        if (colliders.Length >= 1)
        {
            direction = myTransform.rotation.y == 0f ? 1 : -1;
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

                directionVector = ((Vector2)playerRb.transform.position - (Vector2)myTransform.position).normalized;
                playerRb.AddForce(directionVector * forceMagnitudeForward2, ForceMode2D.Impulse);
                //playerRb.AddForce(Vector2.right * -selfForceMagnitudeForward, ForceMode2D.Impulse);
                playerHealth.TakeDamage(damage);
                
            }
            if(collider.gameObject.CompareTag("Enemy"))
            {
                enemyRbOthers = collider.GetComponent<Rigidbody2D>();
                directionVector = ((Vector2)enemyRbOthers.transform.position - (Vector2)myTransform.position).normalized;
                enemyRbOthers.AddForce(directionVector * forceMagnitudeForward2, ForceMode2D.Impulse);
            }
         
            
        }
        StartCoroutine(DestroyComicBoomEffect());
        monsterHealth.TakeDamage(kamikazeDamageHimself);
    }


    private void OnDrawGizmos()
    {
        // Dessiner le rayon de détection dans l'éditeur
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(detectionPosition, detectionRadius);
    }

    public IEnumerator DestroyComicBoomEffect()
    {
        comicBoom = Instantiate(ComicBoomEffect,new Vector2(myTransform.position.x, myTransform.position.y + 2f), Quaternion.identity);
        lighter = comicBoom.GetComponentInChildren<Light>();
        
        yield return new WaitForSeconds(0.3f);
        circleArea.SetActive(false);
        Destroy(comicBoom);
        Destroy(lighter);
        
        yield return null;
    }

}

