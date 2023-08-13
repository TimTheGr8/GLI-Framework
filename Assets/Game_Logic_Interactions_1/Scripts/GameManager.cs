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

    private bool _gameRunning = false;
    private int _totalEnemies = 0;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _gameRunning = true;
    }

    void Update()
    {
        
    }

    public bool IsGameRunning()
    {
        return _gameRunning;
    }

    public void GameOver()
    {
        _gameRunning = false;
    }

    public void UpdateEnemyCount(int amount)
    {
        _totalEnemies += amount;
        UIManager.Instance.UpdateEnemies(_totalEnemies);
    }
}
