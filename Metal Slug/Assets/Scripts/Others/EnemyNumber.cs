

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNumber : MonoBehaviour
{
    public Text enemyText;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] enemiesRemainingAlive = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemiesRemainingAlive = GameObject.FindGameObjectsWithTag("Enemy");
        int numberOfEnemies = enemiesRemainingAlive.Length;
        string v = "Enemy number : " + numberOfEnemies;
        enemyText.text = v;
    }
}

