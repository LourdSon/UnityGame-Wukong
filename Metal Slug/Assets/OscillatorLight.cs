using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class testLightChanging : MonoBehaviour
{
    public float randomFallOf;
    public Light2D mylight2D;

    public float oscillationSpeed = 2f; // La vitesse d'oscillation
    public float minFalloff = 5f;     // Valeur minimale du falloff
    public float maxFalloff = 10f;
    // Start is called before the first frame update
    void Start()
    {
        mylight2D = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Créer une oscillation avec Mathf.Sin
        randomFallOf = Mathf.Lerp(maxFalloff, minFalloff, (Mathf.Sin(Time.time * oscillationSpeed) + 1) / 2);
        mylight2D.shapeLightFalloffSize = randomFallOf;
    }


}
