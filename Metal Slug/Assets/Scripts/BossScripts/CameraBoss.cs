using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraBoss : MonoBehaviour
{
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera bossCamera;
    public Animator bossAnimator;
    private string bossAnimationName;
    private GameObject bosses;

    private void Start()
    {
        // Initialement, la caméra suit le joueur
        playerCamera.Priority = 10;
        bossCamera.Priority = 0;
        GameObject bosses = GameObject.FindGameObjectWithTag("Boss");
        Animator animator = bosses.GetComponent<Animator>();
        Animator bossAnimator = animator;


    }

    private void Update()
    {
        GameObject bosses = GameObject.FindGameObjectWithTag("Boss");
        Animator animator = bosses.GetComponent<Animator>();
        Animator bossAnimator = animator;
        // Vérifiez si l'animation du boss commence
        if (bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("introBoss") || bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("BossOnGround"))
        {
            // Augmentez la priorité de la caméra du boss
            bossCamera.Priority = 20;
            playerCamera.Priority = 0;
        }
        else
        {
            // Revenez à la caméra du joueur après l'animation du boss
            bossCamera.Priority = 0;
            playerCamera.Priority = 10;

        }
    }
}
