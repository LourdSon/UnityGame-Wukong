
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{

    public Slider slider;
    public Slider slider2;
    // public Camera camera;
    public Transform target;
    public Vector3 offset;
    public Vector3 offset2;

    private Camera mainCam;
    private Transform myTransform;
    private Transform myTransform2;


    void Start()
    {
        mainCam = Camera.main;
        myTransform = transform;
        myTransform2 = slider2.transform;
    }
    // Update is called once per frame
    void Update()
    {
        myTransform.rotation = mainCam.transform.rotation;
        myTransform.position = target.position + offset;
        myTransform2.position = target.position + offset2;
    }


    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    public void UpdateShieldBar(float currentValue, float maxValue)
    {
        slider2.value = currentValue / maxValue;
    }
}
