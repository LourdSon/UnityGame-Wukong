using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class introBehaviour : StateMachineBehaviour
{

    private Transform playerPos;
    public float speed;
    public float offset = 5f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.WriteDefaultValues();
        //animator.transform.position = new Vector2(playerPos.position.x + offset, animator.transform.position.y);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
