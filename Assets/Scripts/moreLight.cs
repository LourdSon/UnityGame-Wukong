using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class moreLight : MonoBehaviour
{
    public PlayerLevel playerLevel;
    public PlayerShooting playerShooting;
    private bool lightIncreased;
    // Start is called before the first frame update
    void Start()
    {
        lightIncreased = false;
    }

    // Update is called once per frame
    void Update()
    {
        moreLevelMoreLight();
    }

    public void moreLevelMoreLight()
    {
        if(playerLevel.isLevelingUp && !lightIncreased)
        {
            GetComponent<Light2D>().intensity *= playerShooting.scaleMultiplier;
            lightIncreased = true;
        }else if (!playerLevel.isLevelingUp)
        {
            lightIncreased = false;
        }
    }
}
