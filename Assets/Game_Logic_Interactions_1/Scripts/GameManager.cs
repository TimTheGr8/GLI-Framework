using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("GameManager is NULL");

            return _instance;
        }
    }


    [SerializeField]
    private GameObject _AI;
    [SerializeField]
    private Transform _spawnLocation;
    [SerializeField]
    private Transform _aiContainer;
    [SerializeField]
    private List<GameObject> _aiPool;

    void Start()
    {
        //Invoke("SpawnAI", 1);
        _aiPool = SpawnManager.Instance.GeneratePool(_AI, _aiPool, 10, _aiContainer);
    }

    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            SpawnManager.Instance.RequestGameObject(_AI, _aiPool, _aiContainer, _spawnLocation);
        }
    }


    private void SpawnAI()
    {
    }
}
