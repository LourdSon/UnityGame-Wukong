using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterDamage : MonoBehaviour
{

    public int damage = 10;
    public float forceMagnitudeForward = 75f;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D playerRb = GetComponent<Rigidbody2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();

        int direction = spriteRenderer.flipX ? -1 : 1;

        if(collision.gameObject.tag == "Enemy")
        {
            playerHealth.TakeDamage(damage);

            playerRb.AddForce(Vector2.right * -forceMagnitudeForward * direction, ForceMode2D.Impulse);
        }
    }
}
