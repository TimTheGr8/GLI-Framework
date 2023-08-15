using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    private GameObject _gameCanvas;
    [SerializeField]
    private GameObject _winCanvas;
    [SerializeField]
    private GameObject _scoreCanvas;
    [SerializeField]
    private GameObject _loseCanvas;
    [SerializeField]
    private TMP_Text _playerScore;

    private int _totalEnemies;
    private bool _gameRunning = false;
    private int _totalKills = 0;
    private int _totalScore = 0;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _gameCanvas.SetActive(true);
        _gameRunning = true;
        _totalEnemies = SpawnManager.Instance.GetEnemyToBeSpawned();
    }

    public void AddScore(int score)
    {
        _totalScore += score;
        _totalKills++;
        UIManager.Instance.UpdateScore(_totalScore);
    }

    public bool IsGameRunning()
    {
        return _gameRunning;
    }

    public void GameOver()
    {
        _gameRunning = false;
        StartCoroutine("DisplayCanvas");
    }

    public void ResartGame()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator DisplayCanvas()
    {
        while(SpawnManager.Instance.EnemyCount > 0)
        {
            yield return null;
        }
        _gameCanvas.SetActive(false);
        float killPercent = (float)_totalKills / (float)_totalEnemies;

        Cursor.lockState = CursorLockMode.None;

        if (killPercent == 1)
        {
            _winCanvas.SetActive(true);
            Debug.Log("You win");
        }
        else if (killPercent >= 0.51f)
        {
            _playerScore.text = _totalScore.ToString();
            _scoreCanvas.SetActive(true);
        }
        else
        {
            _loseCanvas.SetActive(true);
        }
        StopCoroutine("DisplayCanvas");
    }
}
