using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowsPlayer : MonoBehaviour
{

    public GameObject player;
    public UnityEngine.Vector3 offset = new UnityEngine.Vector3(5.75f,2.1f,-1);

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void LateUpdate()
    {
        if(player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }
}
