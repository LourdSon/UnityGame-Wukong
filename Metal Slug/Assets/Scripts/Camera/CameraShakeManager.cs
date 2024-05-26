using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeManager : MonoBehaviour
{



    public static CameraShakeManager instance;
    public float globalShakeForce = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Assurez-vous que l'instance persiste entre les scènes si nécessaire
        }
        else
        {
            Destroy(gameObject); // Détruisez l'instance dupliquée si elle existe
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulseWithForce(globalShakeForce);
        }
        else
        {
            Debug.LogWarning("CinemachineImpulseSource est null lors de l'appel à CameraShake.");
        }
    }
}
