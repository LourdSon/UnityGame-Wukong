using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyOnEnemies : MonoBehaviour
{
    public ParticleSystem destroyOnImpact;
    private ParticleSystem instanceDestroy;
    public GameObject self;
    private CinemachineImpulseSource impulseSource;
    public bool isReadyToHit;
    
    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    void OnEnable()
    {
        isReadyToHit = false;
        StartCoroutine(WaitForIt());
    }
    public void OnInstanceDestroyParticle(GameObject others)
    {
        CameraShakeManager.instance.CameraShake(impulseSource);
        instanceDestroy = Instantiate(destroyOnImpact, others.transform.position, Quaternion.identity);       
    }   
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy" && isReadyToHit == true)
        {
            OnInstanceDestroyParticle(self);
            if(other.gameObject.GetComponent<MonsterHealth>() != null)
                other.gameObject.GetComponent<MonsterHealth>().TakeDamage2(30);
            gameObject.SetActive(false);
        }
    }

    public IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(1.5f);
        isReadyToHit = true;
    }
}
