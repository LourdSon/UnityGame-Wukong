using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public GameObject energyBallPrefab;
    public int poolSize = 20;
    private List<GameObject> pool;


    private GameObject obj;
    

    void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            obj = Instantiate(energyBallPrefab,new Vector3(-500f, -500f, 0f), Quaternion.identity);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                // obj.SetActive(true);
                return obj;
            }
        }

        // Si tous les objets sont utilisés, vous pouvez choisir de créer un nouvel objet
        GameObject newObj = Instantiate(energyBallPrefab);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
        // return null;
    }
}

