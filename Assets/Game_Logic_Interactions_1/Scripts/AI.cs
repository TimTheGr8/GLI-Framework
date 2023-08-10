using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField]
    private Transform _destination;

    private NavMeshAgent _agent;
    private bool _isActive = false;

    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _destination = GameObject.Find("AI-Ending-Position").transform;
    }

    private void OnEnable()
    { 
        _agent.SetDestination(_destination.position);
        _isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        this.gameObject.SetActive(false);
    }
}
