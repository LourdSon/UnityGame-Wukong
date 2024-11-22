

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveNumber : MonoBehaviour
{
    public Text waveText;
    public WaveManager waveManager;
    private int waveManagerNumber;
    private string v;

    

    
    void Update()
    {
        //waveManager = GetComponent<WaveManager>();
        if(waveManager != null)
        {
            waveManagerNumber = waveManager.numerOfWaveDone;
            //int waveNumber = waveManager.currentWaveIndex;
            v = "Round : " + waveManagerNumber;
            waveText.text = v;

        }
    }

}
