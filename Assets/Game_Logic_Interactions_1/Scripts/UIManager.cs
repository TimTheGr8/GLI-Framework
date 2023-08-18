using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("There is no UIManager");
            
            return _instance;
        }
    }

    [SerializeField]
    private TMP_Text _score;
    [SerializeField]
    private TMP_Text _ammoCount;
    [SerializeField]
    private TMP_Text _enemyCount;
    [SerializeField]
    private TMP_Text _timeRemaining;
    [SerializeField]
    private TMP_Text _playerScore;
    //[SerializeField]
    //private float _secondsRemaining = 60f;

    private void Awake()
    {
        _instance = this;
    }

    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        _timeRemaining.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateScore(int score)
    {
        _score.text = score.ToString();
    }

    public void UpdateEnemies(int enemyCount)
    {
        _enemyCount.text = enemyCount.ToString();
    }

    public void UpdateAmmoCount(int ammoCount)
    {
        _ammoCount.text = ammoCount.ToString();
    }

    public void UpdatePlayerScore(int score)
    {
        _playerScore.text = score.ToString();
    }
}
