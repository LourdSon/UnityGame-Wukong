using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreHitBoxShoot : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreLayerCollision(12,19,true);
        Physics2D.IgnoreLayerCollision(20,21,true);
    }
}
