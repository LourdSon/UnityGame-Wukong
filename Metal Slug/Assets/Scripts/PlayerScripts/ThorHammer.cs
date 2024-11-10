

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorHammer : MonoBehaviour
{

    public Transform player;
    public float throwSpeed = 20f;
    public float returnSpeed = 20f;

    public Rigidbody2D rb;
    public bool isThrown = false;
    public bool isStuck = false;
    public bool isReturning = false;
    public Vector2 initialPosition;
    public GameObject Hammer;
    public GameObject hammerInstance;
    public SpriteRenderer spriteRenderer;


    private int direction2;
    private Vector2 throwDirection;
    private Vector2 direction;
    private Transform myTransform;


    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        myTransform = transform;
    }

    void Update()
    {
        if (Input.GetButtonDown("Boomerang") && !isThrown)
        {
            ThrowObject();
        }

        if (Input.GetButtonDown("Return Boomerang") && (isStuck || isThrown))
        {
            ReturnObject();
        }

        if (isReturning)
        {
            MoveTowardsPlayer();
        }

        
    }

    public void ThrowObject()
    {
        direction2 = spriteRenderer.flipX ? -1 : 1;
        throwDirection = (Vector2.right * direction2).normalized;

        hammerInstance = Instantiate(Hammer, myTransform.position, Quaternion.identity);
        rb = hammerInstance.GetComponent<Rigidbody2D>();
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
        direction = ((Vector2)myTransform.position - rb.position).normalized;
        rb.velocity = direction * returnSpeed;

        if (Vector2.Distance(rb.position, myTransform.position) < 0.5f)
        {
            isReturning = false;
            isThrown = false;
            rb.velocity = Vector2.zero;
            Destroy(hammerInstance);
            ResetObject();
        }
    }

    /*void OnCollisionEnter2D(Collision2D other)
    {
        if ((isThrown || isReturning) && !isStuck && other.gameObject.tag == "Enemy")
        {
            
            isThrown = false;
            isStuck = true;
            onStuck();
            
        }
    }
    public void onStuck()
    {
        if(isStuck)
        {
            rb.velocity = Vector2.zero;
            //rb.transform.position = other.transform.position;
            rb.isKinematic = true; // Make the object stick
        }
    }
*/
    public void ResetObject()
    {
        isThrown = false;
        isStuck = false;
        isReturning = false;
        rb.isKinematic = false;
        //transform.position = initialPosition;
    }
}



