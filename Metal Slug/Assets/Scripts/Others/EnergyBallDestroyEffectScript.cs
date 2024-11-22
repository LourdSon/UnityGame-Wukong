using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnergyBallDestroyEffectScript : MonoBehaviour
{
    public ParticleSystem destroyOnImpact;
    private ParticleSystem instanceDestroy;
    public GameObject self;
    

    public void OnInstanceDestroyParticle(GameObject others)
    {
        instanceDestroy = Instantiate(destroyOnImpact, others.transform.position, Quaternion.identity);       
    }   
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground" && self.tag == "EnergyBall")
        {
            OnInstanceDestroyParticle(self);
            Destroy(self);
        }
        if(other.gameObject.tag == "Ground" && self.tag == "EnergyBall3")
        {
            OnInstanceDestroyParticle(self);
            self.SetActive(false);
        } 
        if(other.gameObject.tag == "Ground" && self.tag == "EnergyBall2")
        {
            OnInstanceDestroyParticle(self);
        }
        if(other.gameObject.tag == "Roofs")
        {
            OnInstanceDestroyParticle(self);
        }
    }
}
