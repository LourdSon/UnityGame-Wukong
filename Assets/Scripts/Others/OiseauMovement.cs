using System.Collections;
using UnityEngine;

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
    private bool isReturning = false;  // Indique si l'oiseau revient à sa position d'origine
    private Vector3 initialPosition;   // Position de départ de l'oiseau
    public float sin1 = 2f;
    public float sin2 = 0.5f;
    public SpriteRenderer sp;

    private Transform myTransform;
    private float elapsedTime;
    private float returnSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
        flyDirection.Normalize();
        sp = GetComponent<SpriteRenderer>();
        myTransform = transform;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(player.position, myTransform.position);
            
            if (distanceToPlayer <= detectionRadius && !isFlying && !isReturning)
            {
                // Orientation de l'oiseau en fonction de la position du joueur
                flyDirection = myTransform.position.x > player.position.x ? new Vector2(1, 1) : new Vector2(-1, 1);
                sp.flipX = myTransform.position.x < player.position.x;

                // Si le joueur entre dans la zone, l'oiseau s'envole
                StartFlying();
            }
            else if (!isFlying && !isReturning)
            {
                // L'oiseau est en état idle (petits sauts)
                IdleBehavior();
            }

            // Gérer le vol de l'oiseau
            if (isFlying)
            {
                HandleFlying();
            }

            // Gérer le retour de l'oiseau
            if (isReturning)
            {
                HandleReturning();
            }
        }
    }

    private void StartFlying()
    {
        isFlying = true;
        animator.SetBool("IsFlying", true);
        animator.SetBool("IsIdling", false);
        elapsedTime = 0f; // Réinitialise le temps écoulé
    }

    private void HandleFlying()
    {
        if (elapsedTime < flyDuration)
        {
            myTransform.Translate(flyDirection * flyAwaySpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
        }
        else
        {
            StartReturning();
        }
    }

    private void StartReturning()
    {
        isReturning = true;
        isFlying = false;
        animator.SetBool("IsFlying", false);
        returnSpeed = flyAwaySpeed; // Vitesse pour le retour (ajustable)
    }

    private void HandleReturning()
    {
        if (Vector2.Distance(myTransform.position, initialPosition) > 0.1f)
        {
            myTransform.position = Vector2.MoveTowards(myTransform.position, initialPosition, returnSpeed * Time.deltaTime);
        }
        else
        {
            // Quand l'oiseau est revenu à sa position, il redevient idle
            isReturning = false;
            animator.SetBool("IsIdling", true);
        }
    }

    void IdleBehavior()
    {
        // L'oiseau effectue de petits mouvements ou sauts pendant qu'il est en idle
        float idleMovement = Mathf.Sin(Time.time * sin1) * sin2; // Mouvement léger gauche-droite
        myTransform.position += new Vector3(idleMovement * Time.deltaTime, 0, 0);
    }

    void OnDrawGizmosSelected()
    {
        // Dessiner la zone de détection dans l'éditeur
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(myTransform.position, detectionRadius);
    }
}
