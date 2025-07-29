using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Destroying : MonoBehaviour
{
    public GameObject gameObjectSelf;
    public Light2D light2D;
    public float destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyComicBoomEffect());
    }

    // Update is called once per frame
    public IEnumerator DestroyComicBoomEffect()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(light2D);
        Destroy(gameObjectSelf);
        
        yield return null;
    }
}
