using UnityEngine;
public class ForceTransfer : MonoBehaviour
{
public Rigidbody2D parentRigidbody;
private Vector2 force;

void OnCollisionEnter2D(Collision2D collision)
    {
        // Transfère la force de la collision au Rigidbody du parent
        force = collision.relativeVelocity * collision.rigidbody.mass;
        parentRigidbody.AddForce(force, ForceMode2D.Impulse);
    }
}