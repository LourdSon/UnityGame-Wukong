using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHitBoxSide : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public GameObject bodyHitbox;
    public float xGauche, xDroite;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  
        bodyHitbox = GetComponent<GameObject>();     
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer.flipX)
        {
            // Si le joueur est retourné, positionne la hitbox de mêlée à gauche du joueur
            bodyHitbox.transform.localPosition = new Vector3(xGauche, 0f, 0f);
        }
        else
        {
            // Si le joueur n'est pas retourné, positionne la hitbox de mêlée à droite du joueur
            bodyHitbox.transform.localPosition = new Vector3(xDroite, 0f, 0f);
        }
    }
}
