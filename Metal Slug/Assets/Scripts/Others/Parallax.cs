

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    private float lengthx, startposx, startposy;
    public GameObject cam;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startposx = transform.position.x;
        lengthx = GetComponent<SpriteRenderer>().bounds.size.x;

        startposy = transform.position.y;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        parallaxF();
    }

    
    private void parallaxF()
    {
        float tempx = cam.transform.position.x * (1 - parallaxEffect);
        float distancex = cam.transform.position.x * parallaxEffect;
        float distancey = cam.transform.position.y * parallaxEffect;

        
        transform.position  = new Vector3(startposx + distancex, startposy + distancey, transform.position.z);

        if(tempx > startposx + lengthx) 
        {
            startposx += lengthx;

        } else if (tempx < startposx - lengthx) 
        {
            startposx -= lengthx;
        }

    }


}

