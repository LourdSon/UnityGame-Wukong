using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorHammer : MonoBehaviour
{

    public Transform player;
    public float throwSpeed = 10f;
    public float returnSpeed = 20f;

    private Rigidbody2D rb;
    private bool isThrown = false;
    private bool isStuck = false;
    private bool isReturning = false;
    private Vector2 initialPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !isThrown)
        {
            ThrowObject();
        }

        if (Input.GetKeyDown(KeyCode.R) && isStuck)
        {
            ReturnObject();
        }

        if (isReturning)
        {
            MoveTowardsPlayer();
        }
    }

    void ThrowObject()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 throwDirection = (mousePos - (Vector2)transform.position).normalized;
        rb.velocity = throwDirection * throwSpeed;
        isThrown = true;
    }

    public void ReturnObject()
    {
        isStuck = false;
        isReturning = true;
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        rb.velocity = direction * returnSpeed;

        if (Vector2.Distance(rb.position, player.position) < 0.5f)
        {
            isReturning = false;
            isThrown = false;
            rb.velocity = Vector2.zero;
            transform.position = player.position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isThrown && !isStuck)
        {
            rb.velocity = Vector2.zero;
            isThrown = false;
            isStuck = true;
            transform.position = collision.contacts[0].point;
            rb.isKinematic = true; // Make the object stick
        }
    }

    public void ResetObject()
    {
        isThrown = false;
        isStuck = false;
        isReturning = false;
        rb.isKinematic = false;
        transform.position = initialPosition;
    }
}



