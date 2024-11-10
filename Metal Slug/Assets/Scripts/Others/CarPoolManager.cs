using UnityEngine;
using System.Collections.Generic;

public class CarPoolManager : MonoBehaviour
{
    public GameObject[] carPrefabs; // Array de prefabs de voiture
    public int poolSize = 10; // Nombre total de voitures dans la pool
    public float carSpeed = 5f; // Vitesse des voitures
    public Vector2 spawnPositionLeft; // Position de spawn pour les voitures venant de la gauche
    public Vector2 spawnPositionRight; // Position de spawn pour les voitures venant de la droite
    public float despawnX = -10f; // X où la voiture disparaît (de droite à gauche)
    public float respawnX = 10f;  // X où la voiture disparaît (de gauche à droite)
    public float spawnInterval = 2f; // Intervalle entre chaque spawn

    private List<GameObject> carPool = new List<GameObject>();




    private int randomIndex;
    private GameObject car;
    private bool fromLeft;
    

    void Start()
    {
        // Initialisation de la pool de voitures avec des prefabs aléatoires
        for (int i = 0; i < poolSize; i++)
        {
            randomIndex = Random.Range(0, carPrefabs.Length); // Choisir un prefab aléatoire
            car = Instantiate(carPrefabs[randomIndex]);
            car.SetActive(false);
            carPool.Add(car);
        }

        // Planifier le premier spawn
        InvokeRepeating("SpawnCar", spawnInterval, spawnInterval);
    }

    void SpawnCar()
    {
        // Chercher une voiture inactive dans la pool
        car = GetPooledCar();
        if (car != null)
        {
            // Choisir aléatoirement un côté pour faire spawn la voiture
            fromLeft = Random.value > 0.5f;

            if (fromLeft)
            {
                // Faire apparaître la voiture depuis la gauche
                car.transform.position = spawnPositionLeft;
                car.transform.rotation = Quaternion.Euler(0,180,0);
                car.GetComponent<SpriteRenderer>().sortingOrder = 49;
                car.GetComponent<CarMovement>().SetDirection(Vector2.left); // De gauche à droite
            }
            else
            {
                // Faire apparaître la voiture depuis la droite
                car.transform.position = spawnPositionRight;
                // car.GetComponent<SpriteRenderer>().flipX = false;
                car.transform.rotation = Quaternion.Euler(0,0,0);
                car.GetComponent<SpriteRenderer>().sortingOrder = 50;
                car.GetComponent<CarMovement>().SetDirection(Vector2.left); // De droite à gauche
            }

            car.SetActive(true); // Activer la voiture
        }
    }

    GameObject GetPooledCar()
    {
        // Trouver une voiture inactive dans la pool
        for (int i = 0; i < carPool.Count; i++)
        {
            if (!carPool[i].activeInHierarchy)
            {
                return carPool[i];
            }
        }
        return null; // Retourne null si aucune voiture n'est disponible
    }
}
