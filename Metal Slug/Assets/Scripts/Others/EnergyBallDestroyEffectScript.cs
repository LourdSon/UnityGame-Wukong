using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnergyBallDestroyEffectScript : MonoBehaviour
{
    public ParticleSystem destroyOnImpact;
    private ParticleSystem instanceDestroy;
    public GameObject self;
    

    public void OnInstanceDestroy(GameObject others)
    {
        instanceDestroy = Instantiate(destroyOnImpact, others.transform.position, Quaternion.identity);       
    }   
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground" && self.tag == "EnergyBall")
        {
            OnInstanceDestroy(self);
            Destroy(self);
        }
        if(other.gameObject.tag == "Ground" && self.tag == "EnergyBall3")
        {
            OnInstanceDestroy(self);
            self.SetActive(false);
        } 
        if(other.gameObject.tag == "Ground" && self.tag == "EnergyBall2")
        {
            OnInstanceDestroy(self);
        }
        if(other.gameObject.tag == "Roofs")
        {
            OnInstanceDestroy(self);
        }
    }
}
