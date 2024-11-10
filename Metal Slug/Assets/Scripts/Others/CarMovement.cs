using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private Vector2 direction;
    public float speed = 5f;
    public float limiteGauche = -100f; // La vitesse de la voiture
    public float limiteDroite = 100f;
    private Transform myTransform;

    
    void Start()
    {
        direction = Vector2.left;
        myTransform = transform;
    }
    void FixedUpdate()
    {

        // Déplacer la voiture dans la direction choisie
        myTransform.Translate(direction * speed * Time.deltaTime);
        if (myTransform.position.x < limiteGauche || myTransform.position.x > limiteDroite)
        {
            gameObject.SetActive(false); // Désactiver la voiture pour la réutiliser plus tard
        }
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
        if(dir == null)
        {
            direction = Vector2.left;
        }
    }
}
