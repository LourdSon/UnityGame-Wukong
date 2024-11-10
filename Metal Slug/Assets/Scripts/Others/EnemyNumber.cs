

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNumber : MonoBehaviour
{
    public Text enemyText;

    public WaveManager waveManager;
    private int numberOfEnemies;
    private string v;


    // Update is called once per frame
    void Update()
    {
        numberOfEnemies = waveManager.enemiesRemainingAlive.Length;
        v = "Enemy number : " + numberOfEnemies;
        enemyText.text = v;
    }
}

