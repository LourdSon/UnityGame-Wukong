

using System.Collections;
using UnityEngine;

public class GiantPositionStart : MonoBehaviour
{
    public Transform playerTr;
    public Transform bossTr;
    public float offset = 5f;
    public Animator animator;
    public GameObject[] bosses;
    public float speed = 20f;
    public SpriteRenderer spriteRenderer;
    private MonsterHealth monsterHealth;
    public float detectionRange = 500f;
    public EndMission endMission;
    

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = spriteRenderer.GetComponent<SpriteRenderer>();
        monsterHealth = animator.GetComponent<MonsterHealth>();
        endMission = GetComponent<EndMission>();
    }

    // Update is called once per frame
    void Update()
    {
        bosses = GameObject.FindGameObjectsWithTag("Boss");
        endMission = GetComponent<EndMission>();
        
        int countB = bosses.Length; 
        if (countB == 1)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("IntroBoss") && animator != null)
            {
                StartCoroutine(Intro());
            }else if(animator.GetCurrentAnimatorStateInfo(0).IsName("BossOnGround"))
            {
                
            }else if(animator.GetCurrentAnimatorStateInfo(0).IsName("BossWalking"))
            {
                //GetComponent<FlyingMonsterMovement>().enabled = true;
                //GetComponent<GiantPositionStart>().enabled = false;
                
                //StartCoroutine(Walking());
                //Walking();
                DetectPlayer();
                
            }
        } else if(countB == 0)
        {
            //Destroy(GetComponent<GameObject>());
        }
    }

    public IEnumerator Intro()
    {
        Vector3 bossPosition = bossTr.position;
        bossPosition.x = playerTr.position.x + offset;
        bossPosition.y = 0f;
        bossTr.position = bossPosition;
        yield return null;
    }
    /*public void Walking()
    {
        int direction;
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
            animator.SetFloat("Speed",Mathf.Abs(speed));
            animator.SetBool("IsWalking", true);
            if(playerTr.position.x > bossTr.transform.position.x)
            {
                spriteRenderer.flipX = true;
            } else 
            {
                spriteRenderer.flipX = false;
            }
        }
    }*/
    public void DetectPlayer()
    {
        MonsterHealth monsterHealth = GetComponentInChildren<MonsterHealth>();
        AttackHitBoxSide attackHitBoxSide = GetComponentInChildren<AttackHitBoxSide>();
        if (playerTr == null)
        {
            return;
        }
        if(!monsterHealth.isTakingDamage  && attackHitBoxSide.isAttacking == false)
        {
            
            // Vérifie la distance entre l'ennemi et le joueur
            float distanceToPlayer = Vector2.Distance(transform.position, playerTr.position);        
            if (distanceToPlayer <= detectionRange)
            {
                // Déplace l'ennemi vers le joueur uniquement sur l'axe X
                transform.position = Vector2.MoveTowards(transform.position, playerTr.position, speed * Time.deltaTime);
                animator.SetFloat("Speed",Mathf.Abs(speed * Time.deltaTime));
                animator.SetBool("IsWalking", true);
                if(playerTr.position.x > transform.position.x)
                {
                    spriteRenderer.flipX = false;
                } else 
                {
                    spriteRenderer.flipX = true;
                }
                
            } else
            {
                animator.SetBool("IsWalking", false);
            }
            
        }
        
    }
}
