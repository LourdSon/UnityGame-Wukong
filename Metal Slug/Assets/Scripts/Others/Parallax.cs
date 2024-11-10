

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    private float lengthx, startposx, startposy;
    public GameObject cam;
    public float parallaxEffect;

    private float tempx;
    private float distancex, distancey;
    private Transform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
        startposx = myTransform.position.x;
        lengthx = GetComponent<SpriteRenderer>().bounds.size.x;

        startposy = myTransform.position.y;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        parallaxF();
    }

    
    private void parallaxF()
    {
        tempx = cam.transform.position.x * (1 - parallaxEffect);
        distancex = cam.transform.position.x * parallaxEffect;
        distancey = cam.transform.position.y * parallaxEffect;

        
        myTransform.position  = new Vector3(startposx + distancex, startposy + distancey, myTransform.position.z);

        if(tempx > startposx + lengthx) 
        {
            startposx += lengthx;

        } else if (tempx < startposx - lengthx) 
        {
            startposx -= lengthx;
        }

    }


}

