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

    [SerializeField]
    private GameObject _AI;
    [SerializeField]
    private Transform _spawnLocation;
    [SerializeField]
    private Transform _aiContainer;
    [SerializeField]
    private List<GameObject> _aiPool;
    [SerializeField]
    private List<Transform> _aiWayPoints;

    private AudioSource _audio;

    void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        if (_audio == null)
            Debug.LogError("The Spawn Manager does not have an AudioSource");

        _aiPool = GeneratePool(_AI, _aiPool, 20, _aiContainer);
        StartCoroutine(StartSpawningAI());
        SpawnAI();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameRunning() == false)
        {
            StopCoroutine(StartSpawningAI());
        }
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

    private void SpawnAI()
    {
        foreach (var go in _aiPool)
        {
            if (go.activeInHierarchy == false)
            {
                go.transform.position = _spawnLocation.position;
                go.SetActive(true);
                GameManager.Instance.UpdateEnemyCount(1);
                return;
            }
        }
    }

    public void AssignWaypoints(List<Transform> waypoints)
    {
        foreach (var point in _aiWayPoints)
        {
            waypoints.Add(point);
        }
    }

    public void PlayAudio(AudioClip clip)
    {
        _audio.PlayOneShot(clip);
    }

    IEnumerator StartSpawningAI ()
    {
        while(GameManager.Instance.IsGameRunning())
        {
            float randTime = Random.Range(1f, 7f);
            yield return new WaitForSeconds(randTime);
            if(GameManager.Instance.IsGameRunning())
                SpawnAI();
        }
    }
}
