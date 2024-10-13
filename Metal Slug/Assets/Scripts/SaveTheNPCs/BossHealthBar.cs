
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCHealthBar : MonoBehaviour
{

    public Slider slider;
    // public new Camera camera;
    public Vector3 position;
    public Transform npcposition;


    
    // Update is called once per frame
    void Start()
    {
        npcposition = GetComponentInParent<Transform>();
        transform.rotation = Camera.main.transform.rotation;
        transform.position = npcposition.transform.position + position;
    }


    /* public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    } */
}
