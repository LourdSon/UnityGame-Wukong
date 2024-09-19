

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBoxKamikaze : MonoBehaviour
{
    public KamikazeAttack kamikazeAttack;
    public float waitingForAttack = 3f;
    
    public Rigidbody2D enemyRb;
    public GameObject meleeHitbox;
    public float xGauche = 0f;
    public float xDroite = 1f;
    private Animator anim;
    public bool isAttacking;

    void Start()
    {
        enemyRb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<Animator>();
        isAttacking = false;
          
    }

    // Update est appelée une fois par frame
    void Update()
    {
        kamikazeAttack = GetComponentInParent<KamikazeAttack>(); 
        if (enemyRb.transform.rotation.y == 180)
        {
            // Si le joueur est retourné, positionne la hitbox de mêlée à gauche du joueur
            meleeHitbox.transform.localPosition = new Vector3(xGauche, 0f, 0f);
        }
        else
        {
            // Si le joueur n'est pas retourné, positionne la hitbox de mêlée à droite du joueur
            meleeHitbox.transform.localPosition = new Vector3(xDroite, 0f, 0f);
        }
        
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {                
        if(coll.gameObject.CompareTag("Player"))
        {
            StartCoroutine(AttackSpe());         
        }
    }

    private IEnumerator AttackSpe()
    {
        isAttacking = true;
        Color tempColor = GetComponentInParent<SpriteRenderer>().color;
        Color.RGBToHSV(tempColor, out float h, out float s, out float v);
        s *= 0.25f;
        Color newColor = Color.HSVToRGB(h, s, v);
        GetComponentInParent<SpriteRenderer>().color = newColor;
        anim.SetTrigger("SimpleAttackTrigger");
        yield return new WaitForSeconds(waitingForAttack);
        kamikazeAttack.Attack();
        yield return null;
        isAttacking = false;
        GetComponentInParent<SpriteRenderer>().color = tempColor;
    }
}

