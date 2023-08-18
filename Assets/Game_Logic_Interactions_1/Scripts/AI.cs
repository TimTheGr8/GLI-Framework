using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
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
    [SerializeField]
    private AudioClip _deathClip;
    [SerializeField]
    private AudioClip _completedTrack;
    private bool _isHiding = false;
    private bool _isDead = false;
    private Animator _anim;
    private AudioSource _auido;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null)
            Debug.LogError("There is no NavMeshAgent");
        _anim = GetComponent<Animator>();
        if (_anim == null)
            Debug.LogError("There is no animator");
        _auido = GetComponent<AudioSource>();
        if (_auido == null)
            Debug.LogError("There is no AudioSource");
    }

    private void OnEnable()
    {
        _currentWaypointIndex = 0;
        _isDead = false;
        _currentState = AIState.Run;
        if (_waypoints.Count == 0)
            SpawnManager.Instance.AssignWaypoints(_waypoints);
        _currentWaypointIndex = Random.Range(0, 5);
        _agent.speed = Random.Range(5.5f, 10.0f);
        _agent.SetDestination(_waypoints[_currentWaypointIndex].position);
    }

    private void OnDisable()
    {
        _isDead = true;
    }

    private void Update()
    {
        switch (_currentState)
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
                if (!_isDead)
                {
                    Death();
                    _isDead = true;
                }
                break;
            default:
                Debug.LogError("The AI has no valid state.");
                break;
        }
    }

    private void Run()
    {
        _anim.SetFloat("Speed", _agent.velocity.magnitude);
        if (_agent.remainingDistance <= 0.75f)
        {
            _currentState = AIState.Hide;
        }
    }

    private void Death()
    {
        GameManager.Instance.AddScore(50);
        _agent.isStopped = true;
        _anim.SetTrigger("Death");
        StartCoroutine(DeactivateObject());
        GameManager.Instance.PlayAudio(_deathClip, 0.5f);
        SpawnManager.Instance.UpdateEnemyCount(-1);
    }

    private void ChooseHidingSpot()
    {
        int newIndex;
        newIndex = Random.Range(_currentWaypointIndex, _waypoints.Count);
        _currentWaypointIndex = newIndex;
        _agent.SetDestination(_waypoints[_currentWaypointIndex].position);
    }

    public void StartDeath()
    {
        _currentState = AIState.Death;
    }

    public bool IsAIDead()
    {
        return _isDead;
    }

    IEnumerator Hide()
    {
        _agent.isStopped = true;
        _agent.avoidancePriority = 0;
        _anim.SetBool("Hiding", true);
        float rand = Random.Range(0.5f, 3f);
        yield return new WaitForSeconds(rand);
        ChooseHidingSpot();
        _agent.isStopped = false;
        _anim.SetBool("Hiding", false);
        _isHiding = false;
        _currentState = AIState.Run;
        _agent.avoidancePriority = 50;
    }

    IEnumerator DeactivateObject()
    {
        yield return new WaitForSeconds(3.7f);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Destination")
        {
            GameManager.Instance.PlayAudio(_completedTrack, 1.0f);
            SpawnManager.Instance.UpdateEnemyCount(-1);
            this.gameObject.SetActive(false);
        }
    }
}
