

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveNumber : MonoBehaviour
{
    public Text waveText;
    public WaveManager waveManager;

    // Start is called before the first frame update
    void Start()
    {
        //waveManager = GetComponent<WaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //waveManager = GetComponent<WaveManager>();
        if(waveManager != null)
        {
            int waveManagerNumber = waveManager.currentWaveIndex;
            //int waveNumber = waveManager.currentWaveIndex;
            string v = "Wave number : " + waveManagerNumber;
            waveText.text = v;

        }
    }

}
