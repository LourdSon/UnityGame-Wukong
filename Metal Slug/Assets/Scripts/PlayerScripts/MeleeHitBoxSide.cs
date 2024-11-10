
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MeleeHitBoxSide : MonoBehaviour
{
    public SpriteRenderer playerSpriteRenderer;
    public GameObject meleeHitbox;
    public float offsetX;
    private Transform myTransform;



    void Start()
    {
        myTransform = transform;
    }
    // Update est appelée une fois par frame
    void Update()
    {
        if (myTransform.rotation.y == 0f)
        {
            // Si le joueur est retourné, positionne la hitbox de mêlée à gauche du joueur
            meleeHitbox.transform.localPosition = new Vector3(-offsetX, 0f, 0f);
        }
        else
        {
            // Si le joueur n'est pas retourné, positionne la hitbox de mêlée à droite du joueur
            meleeHitbox.transform.localPosition = new Vector3(offsetX, 0f, 0f);
        }
    }
}

