

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRunning : StateMachineBehaviour
{

    public float detectionRange = 50f; // Portée de détection de l'ennemi
    public float speed = 10f; // Vitesse de déplacement de l'ennemi
    public GameObject player;
    private Transform playerTr;
    private Transform bossTr;
    public int direction;
    private MonsterHealth monsterHealth;

    private SpriteRenderer spriteRenderer;
    private Animator anim;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTr = player.GetComponent<Transform>();       
        bossTr = animator.GetComponent<Transform>();
        monsterHealth = animator.GetComponent<MonsterHealth>();
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        anim = animator.GetComponent<Animator>();    
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //bossMovement();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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
            bossTr.transform.Translate(Vector2.right * Time.deltaTime * speed * direction);
            anim.SetFloat("Speed",Mathf.Abs(speed));
            anim.SetBool("IsWalking", true);
            if(playerTr.position.x > bossTr.transform.position.x)
            {
                spriteRenderer.flipX = true;
            } else 
            {
                spriteRenderer.flipX = false;
            }
        } 
    }
}
