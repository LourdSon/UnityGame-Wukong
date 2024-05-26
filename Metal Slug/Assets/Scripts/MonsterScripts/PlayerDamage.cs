using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{

    public int damage = 30;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "EnergyBall")
        {

            MonsterHealth monsterHealth = GetComponent<MonsterHealth>();
            monsterHealth.TakeDamage(damage);
            Destroy(other.gameObject);

            
        }
    }
}
