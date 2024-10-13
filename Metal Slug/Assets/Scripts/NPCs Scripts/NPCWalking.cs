// using UnityEditor.Callbacks;
using UnityEngine;

public class NPCWalking : MonoBehaviour
{
    public Transform home;
    public Transform dest;
    public float speed = 2f;
    public float waitTime = 2f;

    private Vector3 target;
    public Animator animator;


    void Start()
    {
        target = home.position; // Commencer au "home"
        MoveToTarget();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        Physics2D.IgnoreLayerCollision(16,16);
        Physics2D.IgnoreLayerCollision(16,14);
        Physics2D.IgnoreLayerCollision(16,11);
    }

    private void MoveToTarget()
    {
        // Déplacer le NPC vers la cible
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if(transform.position.x > target.x)
        {
            transform.rotation = Quaternion.Euler(0f, -180f,0f);
        }else 
        {
            transform.rotation = Quaternion.Euler(0f,0f,0f);
        }

        // Vérifier si le NPC est proche de la cible
        if (Vector3.Distance(transform.position, target) > 1f)
        {
            // Si pas encore arrivé, rappeler cette méthode dans la prochaine frame
            Invoke(nameof(MoveToTarget), Time.deltaTime);
        }
        else
        {
            // Si arrivé, attendre un moment et changer de cible
            Invoke(nameof(WaitAndMove), waitTime);
        }
    }

    private void WaitAndMove()
    {
        // Changer de cible
        target = (target == home.position) ? dest.position : home.position;
        MoveToTarget(); // Commencer à se déplacer vers la nouvelle cible
    }
}
