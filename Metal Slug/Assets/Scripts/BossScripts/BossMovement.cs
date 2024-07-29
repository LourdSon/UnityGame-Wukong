using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float detectionRange = 50f; // Portée de détection de l'ennemi
    public float speed = 10f; // Vitesse de déplacement de l'ennemi
    public GameObject player;
    private Rigidbody2D playerRb;
    private Rigidbody2D bossRb;
    private Transform playerTr;
    private Transform bossTr;
    public int direction;
    private MonsterHealth monsterHealth;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float offset = 5f;
    void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();
        playerTr = player.GetComponent<Transform>();
        bossRb = GetComponent<Rigidbody2D>();
        bossTr = GetComponent<Transform>();
        monsterHealth = GetComponent<MonsterHealth>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        bossMovement();
    }

    public void bossMovement()
    {

        if (playerTr.position.x - bossTr.position.x < 0)
        {
            direction = -1;
        }else
        {
            direction = 1;
        }
        if(!monsterHealth.isTakingDamage)
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed * direction);
            animator.SetFloat("Speed",Mathf.Abs(speed));
            animator.SetBool("IsWalking", true);
            if(playerTr.position.x > transform.position.x)
            {
                spriteRenderer.flipX = true;
            } else 
            {
                spriteRenderer.flipX = false;
            }
        } 
    }
}
