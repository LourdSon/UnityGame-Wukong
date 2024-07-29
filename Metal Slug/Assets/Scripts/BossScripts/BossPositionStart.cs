using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Rendering;
using UnityEngine;

public class BossPositionStart : MonoBehaviour
{
    public Transform playerTr;
    public Transform bossTr;
    public float offset = 5f;
    public Animator animator;
    public GameObject[] bosses;
    public float speed = 20f;
    public SpriteRenderer spriteRenderer;
    private MonsterHealth monsterHealth;
    

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = spriteRenderer.GetComponent<SpriteRenderer>();
        monsterHealth = animator.GetComponent<MonsterHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        bosses = GameObject.FindGameObjectsWithTag("Boss");
        int countB = bosses.Length; 
        if (countB == 1)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("introBoss") && animator != null)
            {
                StartCoroutine(Intro());
            }else if(animator.GetCurrentAnimatorStateInfo(0).IsName("BossOnGround"))
            {
                /*Vector3 bossPosition = bossTr.position;
                bossPosition.x = playerTr.position.x + offset;
                bossTr.position = bossPosition;
                Debug.Log("Alors");*/
            }else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
            {
                
                //StartCoroutine(Walking());
                Walking();
                
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
    public void Walking()
    {
        /*animator.SetBool("IsWalking", true);
        float newXPosition = Mathf.MoveTowards(bossTr.position.x, playerTr.position.x, speed/5 * Time.deltaTime);
        bossTr.position = new Vector2(newXPosition, bossTr.position.y);
        bossTr.transform.Translate(bossTr.position);
        animator.SetFloat("Speed",Mathf.Abs(speed/5));
        if(playerTr.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        } else 
        {
            spriteRenderer.flipX = false;
        }
        //yield return null;
        */
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
    }
}
