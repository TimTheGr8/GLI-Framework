using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("SpawnManager is NULL");

            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    public List<GameObject> GeneratePool(GameObject gameObject, List<GameObject> pool, int numberOfObjects, Transform container)
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            GameObject go = Instantiate(gameObject);
            go.transform.parent = container;
            go.SetActive(false);
            pool.Add(go);
        }

        return pool;
    }

    public GameObject RequestGameObject(GameObject gameObject, List<GameObject> pool, Transform container, Transform spawnLocation)
    {
        foreach (var go in pool)
        {
            if(go.activeInHierarchy == false)
            {
                go.transform.position = spawnLocation.position;
                go.SetActive(true);
                return go;
            }
        }
        // Create a new game object and add it to the pool
        GameObject gameObj = Instantiate(gameObject);
        gameObj.transform.parent = container;
        pool.Add(gameObj);

        return gameObj;
    }
}
