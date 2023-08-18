using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private float _secondsRemaining = 60f;
    [SerializeField]
    private AudioClip _winClip;
    [SerializeField]
    private AudioClip _scoreClip;
    [SerializeField]
    private AudioClip _loseClip;

    private AudioSource _audio;
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
        _audio = GetComponent<AudioSource>();
        if (_audio == null)
            Debug.LogError("There is no AudioSource on the Game Manager");

        _gameCanvas.SetActive(true);
        _gameRunning = true;
        _totalEnemies = SpawnManager.Instance.GetEnemyToBeSpawned();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameRunning() && _secondsRemaining > 0)
        {
            _secondsRemaining -= Time.deltaTime;
            UIManager.Instance.DisplayTime(_secondsRemaining);
        }
        else
        {
            _secondsRemaining = 0;
            GameManager.Instance.GameOver();
        }
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
    // TODO: Fix this so that it will stop playing the audio
    IEnumerator DisplayCanvas()
    {
        while(SpawnManager.Instance.EnemyCount > 0)
        {
            yield return null;
        }
        _gameCanvas.SetActive(false);
        float killPercent = (float)_totalKills / (float)_totalEnemies;
        Cursor.lockState = CursorLockMode.None;
        _audio.loop = false;
        if (killPercent == 1)
        {
            _winCanvas.SetActive(true);
            _audio.clip = _winClip;
        }
        else if (killPercent >= 0.51f)
        {
            UIManager.Instance.UpdatePlayerScore(_totalScore);
            _scoreCanvas.SetActive(true);
            _audio.clip = _scoreClip;
        }
        else
        {
            _loseCanvas.SetActive(true);
            _audio.clip = _loseClip;
        }
        _audio.Play();
        StopCoroutine("DisplayCanvas");
    }
}
