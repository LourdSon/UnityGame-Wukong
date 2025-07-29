
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MeleeBaseState : State
{
    // How long this state should be active for before moving on
    public float duration;
    // Cached animator component
    protected Animator animator;
    // bool to check whether or not the next attack in the sequence should be played or not
    protected bool shouldCombo;
    // The attack index in the sequence of attacks
    protected int attackIndex;



    // The cached hit collider component of this attack
    protected Collider2D hitCollider;
    // Cached already struck objects of said attack to avoid overlapping attacks on same target
    private List<Collider2D> collidersDamaged;
    // The Hit Effect to Spawn on the afflicted Enemy
    private GameObject HitEffectPrefab;

    // Input buffer Timer
    private float AttackPressedTimer = 0;

    public int damageManual = 10;
    public MonsterHealth monsterHealth;
    public float forceAmount = 80f;
    private SpriteRenderer spriteRenderer;

    //Test degats
    

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        animator = GetComponent<Animator>();
        collidersDamaged = new List<Collider2D>();
        hitCollider = GetComponent<ComboCharacter>().hitbox;
        HitEffectPrefab = GetComponent<ComboCharacter>().Hiteffect;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        AttackPressedTimer -= Time.deltaTime;

        if (animator.GetFloat("Weapon.Active") > 0f)
        {
            Attack();
        }


        if (Input.GetKeyDown(KeyCode.L))
        {
            AttackPressedTimer = 2;
        }

        if (animator.GetFloat("AttackWindow.Open") > 0f && AttackPressedTimer > 0)
        {
            shouldCombo = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected void Attack()
    {
        Collider2D[] collidersToDamage = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        int colliderCount = Physics2D.OverlapCollider(hitCollider, filter, collidersToDamage);
        for (int i = 0; i < colliderCount; i++)
        {

            if (!collidersDamaged.Contains(collidersToDamage[i]))
            {
                TeamComponent hitTeamComponent = collidersToDamage[i].GetComponentInChildren<TeamComponent>();

                // Only check colliders with a valid Team Componnent attached
                if (hitTeamComponent && hitTeamComponent.teamIndex == TeamIndex.Enemy)
                {
                    GameObject.Instantiate(HitEffectPrefab, collidersToDamage[i].transform);
                    Debug.Log("Enemy Has Taken:" + attackIndex + "Damage");
                    collidersDamaged.Add(collidersToDamage[i]);
                    monsterHealth = hitTeamComponent.GetComponentInChildren<MonsterHealth>();
                    monsterHealth.TakeDamage(damageManual);


                    // Récupérer le Rigidbody de l'ennemi touché
                    Rigidbody2D enemyRb = collidersToDamage[i].GetComponent<Rigidbody2D>();
                    if(enemyRb != null)
                    {
                         // Déterminer la direction de la force en fonction des touches pressées par le joueur
                        float horizontalInput = Input.GetAxis("Horizontal");
                        float verticalInput = Input.GetAxis("Vertical");
                        Vector2 forceDirection = new Vector2(horizontalInput, verticalInput).normalized;
                        // Appliquer une force à l'ennemi
                        enemyRb.AddForce(forceDirection * forceAmount, ForceMode2D.Impulse);
                        if (forceDirection.x == 0f && forceDirection.y == 0f)
                        {
                            int direction = spriteRenderer.flipX ? -1 : 1;
                            enemyRb.AddForce(Vector2.right * direction * forceAmount, ForceMode2D.Impulse);

                        }
                    }
                    
                    
                }
            }
        }
    }

}

