using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerCollision : MonoBehaviour
{

    private ThorHammer hammer;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if ((hammer.isThrown || hammer.isReturning) && !hammer.isStuck && other.gameObject.tag == "Enemy")
        {
            rb.velocity = Vector2.zero;
            hammer.isThrown = false;
            hammer.isStuck = true;
            rb.transform.position = other.transform.position;
            rb.isKinematic = true; // Make the object stick
        }
    }
}
