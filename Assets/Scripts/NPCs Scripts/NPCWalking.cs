// using UnityEditor.Callbacks;
using UnityEngine;
using System.Collections;

public class NPCWalking : MonoBehaviour
{
    public Transform home;
    public Transform dest;
    public float speed = 2f;
    public float waitTime = 2f;

    private Vector3 target;
    public Animator animator;



    private Transform myTransform;

    void Start()
    {
        target = home.position; // Commencer au "home"
        
        animator = GetComponent<Animator>();
        if(transform != null)
        myTransform = transform;
        MoveToTarget();
    }
    void Update()
    {
        Physics2D.IgnoreLayerCollision(16,16);
        Physics2D.IgnoreLayerCollision(16,14);
        Physics2D.IgnoreLayerCollision(16,11);

        if(myTransform.rotation.z != 0)
        {
            StartCoroutine(resetRotation());
        }
    }

    private void MoveToTarget()
    {
        // Déplacer le NPC vers la cible
        myTransform.position = Vector3.MoveTowards(myTransform.position, target, speed * Time.deltaTime);
        if(myTransform.position.x > target.x)
        {
            myTransform.rotation = Quaternion.Euler(0f, -180f,0f);
        }else 
        {
            myTransform.rotation = Quaternion.Euler(0f,0f,0f);
        }

        // Vérifier si le NPC est proche de la cible
        if (Vector3.Distance(myTransform.position, target) > 1f)
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

    private IEnumerator resetRotation()
    {
        yield return new WaitForSeconds(5f);
        myTransform.rotation = Quaternion.Euler(0f,myTransform.rotation.y,0f);
        yield return null;
    }
}
