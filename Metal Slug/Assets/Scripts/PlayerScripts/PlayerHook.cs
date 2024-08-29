

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public float grappleLength;
    public LayerMask grappleLayer;
    public LineRenderer rope;

    public Vector2 grapplePoint;
    public DistanceJoint2D joint;
    public Camera mainCamera;
    
    void Start()
    {
        joint = gameObject.GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        rope.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.R))
        {
            RaycastHit2D hit = Physics2D.Raycast
            (
                origin: mainCamera.ScreenToWorldPoint(Input.mousePosition), 
                direction: Vector2.zero, 
                distance: Mathf.Infinity,
                layerMask: grappleLayer
            );

            if(hit.collider != null)
            {
                grapplePoint = hit.point;

                joint.connectedAnchor = grapplePoint;
                joint.enabled = true;
                joint.distance = grappleLength;

                rope.SetPosition(0, grapplePoint);
                rope.SetPosition(1, transform.position);
                rope.enabled = true;
            }
        }

        if(Input.GetKeyUp(KeyCode.R))
        {
           joint.enabled = false;
           rope.enabled = false;
        }

        if(rope.enabled == true)
            rope.SetPosition(1, transform.position);
    }*/

        if(Input.GetKeyDown(KeyCode.R))
        {
            Vector2 mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
            rope.SetPosition(0, mousePos);
            rope.SetPosition(1, transform.position);
            joint.connectedAnchor = mousePos;
            joint.enabled = true;
            rope.enabled = true;
        } else if (Input.GetKeyUp(KeyCode.R))
        {
            joint.enabled = false;
            rope.enabled = false;
        } if(joint.enabled)
        {
            rope.SetPosition(1, transform.position);
        }
    }
}

