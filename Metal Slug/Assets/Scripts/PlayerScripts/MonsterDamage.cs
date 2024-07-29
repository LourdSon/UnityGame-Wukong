using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterDamage : MonoBehaviour
{

    public int damage = 10;
    public float forceMagnitudeForward = 75f;
    public Rigidbody2D playerRb;
    public SpriteRenderer spriteRenderer;
    public PlayerHealth playerHealth;
    


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        int direction = spriteRenderer.flipX ? -1 : 1;
        

        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            playerHealth.TakeDamage(damage);

            playerRb.AddForce(Vector2.right * forceMagnitudeForward * -direction, ForceMode2D.Impulse);
        }
    }
}
