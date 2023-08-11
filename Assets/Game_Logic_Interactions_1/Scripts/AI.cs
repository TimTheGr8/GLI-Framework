using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField]
    private Transform _destination;

    private NavMeshAgent _agent;
    private enum AIState
    {
        Run,
        Hide,
        Death
    }
    [SerializeField]
    private AIState _currentState;
    
    private List<Transform> _waypoints = new List<Transform>();
    [SerializeField]
    private int _currentWaypointIndex = 0;
    private bool _isHiding = false;
    private Animator _anim;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null)
            Debug.LogError("There is no NavMeshAgent");
        _anim = GetComponent<Animator>();
        if (_anim == null)
            Debug.LogError("There is no animator");
    }

    private void OnEnable()
    {
        _currentWaypointIndex = 0;
        _currentState = AIState.Run;
        if (_waypoints.Count == 0)
            GameManager.Instance.AssignWaypoints(_waypoints);
        _currentWaypointIndex = Random.Range(0, 5);
        _agent.SetDestination(_waypoints[_currentWaypointIndex].position);
    }

    private void Update()
    {
        switch(_currentState)
        {
            case AIState.Run:
                Run();
                break;
            case AIState.Hide:
                if (!_isHiding)
                {
                    StartCoroutine(Hide());
                    _isHiding = true;
                }
                break;
            case AIState.Death:
                Death();
                break;
            default:
                Debug.LogError("The AI has no valid state.");
                break;
        }
    }

    private void Run()
    {
        if(_agent.remainingDistance <= 0.25f)
        {
            _currentState = AIState.Hide;
        }
    }

    private void Death()
    {
        _agent.isStopped = true;
        // Award 50 points to the player & update the UI
    }

    private void ChooseHidingSpot()
    {
        int newIndex;
        if (_currentWaypointIndex <= 4)
        {
             newIndex = Random.Range(_currentWaypointIndex, 7);
        }
        else if(_currentWaypointIndex <= 6)
        {
            newIndex = Random.Range(_currentWaypointIndex, 10);
        }
        else
        {
            newIndex = _waypoints.Count - 1;
        }
        // Level 1 0 - 4
        // Level 2 5 - 6
        // Level 3 7 - 9
        _currentWaypointIndex = newIndex;
        _agent.SetDestination(_waypoints[_currentWaypointIndex].position);
    }

    public void StartDeath()
    {
        _currentState = AIState.Death;
        Debug.Log("You get 50 points.");
    }

    IEnumerator Hide()
    {
        _agent.isStopped = true;
        float rand = Random.Range(0.25f, 3f);
        yield return new WaitForSeconds(rand);
        ChooseHidingSpot();
        _currentState = AIState.Run;
        _agent.isStopped = false;
        _isHiding = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Destination")
            this.gameObject.SetActive(false);
    }
}
