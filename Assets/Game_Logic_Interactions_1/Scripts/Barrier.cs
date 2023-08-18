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
    private Collider _collider;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        if (_renderer == null)
            Debug.LogError("There is no MeshRenderer on the barrier.");
        _collider = GetComponent<Collider>();
        if (_collider == null)
            Debug.LogError("There is no collider on this object.");
        _currentHealth = _maxHealth;
    }

    public void TakeDamage()
    {
        _currentHealth--;
        if(_currentHealth <= 0)
        {
            StartCoroutine(RechargeBarrier(_rechargeTime));
            _collider.enabled = false;
            _renderer.enabled = false;
        }
    }

    IEnumerator RechargeBarrier(float time)
    {
        yield return new WaitForSeconds(time);
        _renderer.enabled = true;
        _collider.enabled = true;
        _currentHealth = _maxHealth;
    }
}
