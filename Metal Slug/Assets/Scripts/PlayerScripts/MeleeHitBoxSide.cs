using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBoxSide : MonoBehaviour
{
    // Référence au sprite renderer du joueur
    public SpriteRenderer playerSpriteRenderer;

    // Référence à la hitbox de mêlée
    public GameObject meleeHitbox;

    // Update est appelée une fois par frame
    void Update()
    {
        // Vérifie si le joueur est retourné horizontalement (flip)
        if (playerSpriteRenderer.flipX)
        {
            // Si le joueur est retourné, positionne la hitbox de mêlée à gauche du joueur
            meleeHitbox.transform.localPosition = new Vector3(-1f, 0f, 0f);
        }
        else
        {
            // Si le joueur n'est pas retourné, positionne la hitbox de mêlée à droite du joueur
            meleeHitbox.transform.localPosition = new Vector3(1f, 0f, 0f);
        }
    }
}
