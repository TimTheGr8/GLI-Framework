using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth = 2;
    [SerializeField]
    private float _rechargeTime = 1.5f;

    private int _currentHealth;
    private MeshRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        if (_renderer == null)
            Debug.LogError("There is no MeshRenderer on the barrier.");
        _currentHealth = _maxHealth;
    }

    public void TakeDamage()
    {
        _currentHealth--;
        if(_currentHealth <= 0)
        {
            StartCoroutine(RechargeBarrier(_rechargeTime));
            _renderer.enabled = false;
        }
    }

    IEnumerator RechargeBarrier(float time)
    {
        yield return new WaitForSeconds(time);
        _renderer.enabled = true;
        _currentHealth = _maxHealth;
    }
}
