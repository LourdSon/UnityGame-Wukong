using UnityEngine;
using System.Collections.Generic;

public class PlanePoolManager : MonoBehaviour
{
    public GameObject[] planesPrefabs; // Array de prefabs de voiture
    public int poolSize = 10; // Nombre total de voitures dans la pool
    public float planesSpeed = 5f; // Vitesse des voitures
    public Vector2 spawnPositionLeft; // Position de spawn pour les voitures venant de la gauche
    public Vector2 spawnPositionRight; // Position de spawn pour les voitures venant de la droite
    public float despawnX = -10f; // X où la voiture disparaît (de droite à gauche)
    public float respawnX = 10f;  // X où la voiture disparaît (de gauche à droite)
    public float spawnInterval = 2f; // Intervalle entre chaque spawn

    private List<GameObject> planePool = new List<GameObject>();

    void Start()
    {
        // Initialisation de la pool de voitures avec des prefabs aléatoires
        for (int i = 0; i < poolSize; i++)
        {
            int randomIndex = Random.Range(0, planesPrefabs.Length); // Choisir un prefab aléatoire
            GameObject plane = Instantiate(planesPrefabs[randomIndex]);
            plane.SetActive(false);
            planePool.Add(plane);
        }

        // Planifier le premier spawn
        InvokeRepeating("SpawnPlanes", spawnInterval, spawnInterval);
    }

    void SpawnPlanes()
    {
        // Chercher une voiture inactive dans la pool
        GameObject plane = GetPooledCar();
        if (plane != null)
        {
            // Choisir aléatoirement un côté pour faire spawn la voiture
            bool fromLeft = Random.value > 0.5f;

            if (fromLeft)
            {
                // Faire apparaître la voiture depuis la gauche
                plane.transform.position = spawnPositionLeft;
                plane.transform.rotation = Quaternion.Euler(0,180,0);
                plane.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
                plane.GetComponent<SpriteRenderer>().sortingOrder = 49;
                plane.GetComponent<CarMovement>().SetDirection(Vector2.left); // De gauche à droite
            }
            else
            {
                // Faire apparaître la voiture depuis la droite
                plane.transform.position = spawnPositionRight;
                // car.GetComponent<SpriteRenderer>().flipX = false;
                plane.transform.rotation = Quaternion.Euler(0,0,0);
                plane.GetComponent<SpriteRenderer>().sortingLayerName = "FrontGround";
                plane.GetComponent<SpriteRenderer>().sortingOrder = 50;
                plane.GetComponent<CarMovement>().SetDirection(Vector2.left); // De droite à gauche
            }

            plane.SetActive(true); // Activer la voiture
        }
    }

    GameObject GetPooledCar()
    {
        // Trouver une voiture inactive dans la pool
        for (int i = 0; i < planePool.Count; i++)
        {
            if (!planePool[i].activeInHierarchy)
            {
                return planePool[i];
            }
        }
        return null; // Retourne null si aucune voiture n'est disponible
    }
}
