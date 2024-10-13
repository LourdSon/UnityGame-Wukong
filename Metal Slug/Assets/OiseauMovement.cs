using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class OiseauMovement : MonoBehaviour
{
    public Transform player;  // Référence au joueur (assigner depuis l'inspecteur)
    public float detectionRadius = 5f; // Rayon de détection du joueur
    public float flyAwaySpeed = 5f;    // Vitesse de vol
    public float flyDuration = 3f;     // Durée du vol avant que l'oiseau ne revienne
    public Vector2 flyDirection = new Vector2(1, 1); // Direction du vol
    public Transform returnPosition;   // Position de retour de l'oiseau (son point de départ)
    
    private Animator animator;
    private bool isFlying = false;
    private Vector3 initialPosition;   // Position de départ de l'oiseau
    private bool isReturning = false;  // Indique si l'oiseau revient à sa position d'origine
    public float sin1= 2f;
    public float sin2 = 0.5f;
    public SpriteRenderer sp;

    void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
        flyDirection.Normalize();
        sp = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if(player != null)
        {
            float distanceToPlayer = Vector2.Distance(player.position, transform.position);
            
            if (distanceToPlayer <= detectionRadius && !isFlying && !isReturning)
            {
                if(transform.position.x > player.position.x)
                {
                    sp.flipX = false;
                    flyDirection = new Vector2(1,1);
                } else if(transform.position.x < player.position.x)
                {
                    flyDirection = new Vector2(-1,1);
                    sp.flipX = true;
                }
                // Si le joueur entre dans la zone, l'oiseau s'envole
                StartCoroutine(FlyAwayRoutine());
            }
            else if (!isFlying && !isReturning)
            {
                // L'oiseau est en état idle (petits sauts)
                IdleBehavior();
            }
        }

        
    }

    IEnumerator FlyAwayRoutine()
    {
        isFlying = true;
        animator.SetBool("IsFlying", true);
        animator.SetBool("IsIdling", false);
        
        float elapsedTime = 0f;

        // Voler pendant la durée définie
        while (elapsedTime < flyDuration)
        {
            transform.Translate(flyDirection * flyAwaySpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Après le temps écoulé, l'oiseau revient
        StartCoroutine(ReturnToStart());
    }

    IEnumerator ReturnToStart()
    {
        isReturning = true;
        isFlying = false;
        animator.SetBool("IsFlying", false);
        if(transform.position.x > initialPosition.x && isReturning)
        {
            sp.flipX = true;
        } else if(transform.position.x < initialPosition.x && isReturning)
        {
            sp.flipX = false;
        }
        // L'oiseau revient à sa position initiale
        float returnSpeed = flyAwaySpeed; // Vitesse plus lente pour le retour (ajustable)
        while (Vector2.Distance(transform.position, initialPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }

        // Quand l'oiseau est revenu à sa position, il redevient idle
        isReturning = false;
        animator.SetBool("IsIdling", true);
    }

    void IdleBehavior()
    {
        // L'oiseau effectue de petits mouvements ou sauts pendant qu'il est en idle
        float idleMovement = Mathf.Sin(Time.time * sin1) * sin2; // Mouvement léger gauche-droite
        transform.position = new Vector2(transform.position.x + idleMovement * Time.deltaTime, transform.position.y);
    }

    void OnDrawGizmosSelected()
    {
        // Dessiner la zone de détection dans l'éditeur
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
