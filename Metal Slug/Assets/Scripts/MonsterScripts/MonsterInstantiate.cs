

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInstantiate : MonoBehaviour
{

    public GameObject ennemyPrefab;
    public Vector3 enemyPosition = new Vector2(3, 5);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Vérifie si la touche "J" est enfoncée
        if (Input.GetKeyDown(KeyCode.J))
        {
            
            // Instancie un ennemi à la position actuelle du GameObject EnnemySpawner
            Instantiate(ennemyPrefab, transform.position + enemyPosition, Quaternion.identity);
        }
    }
}

