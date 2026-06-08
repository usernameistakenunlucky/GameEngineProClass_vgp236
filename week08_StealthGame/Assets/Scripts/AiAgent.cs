using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    enum BehaviourState
    {
        Wander, // randomly go to waypoints
        Seek // Go To specified Target
    }

    // data for wander Behaviour
    public class WanderData
    {
        public float minUpdateTime = 10.0f;
        public float maxUpdateTime = 20.0f;
        public float moveRange = 5.0f;

        public float updateTime = 0.0f;
        public Vector3 centerPoint = Vector3.zero;
        public Vector3 currentTarget = Vector3.zero;

    }

    // data for seek Behaviour
    public class SeekData
    {
        public float maxSearchTime = 5.0f;
        public float searchTime = 0.0f;
        public Vector3 targetPosition = Vector3.zero;

    }

    [SerializeField] private Animator _animator = null;
    [SerializeField] private GameObject _targetObject = null; // This can be the player!
    [SerializeField] private float _viewDistance = 10.0f;
    [SerializeField] private float _speedThreshold = 0.1f;
    [SerializeField] private float _walkSpeed = 2.0f;
    [SerializeField] private float _runSpeed = 5.0f;
    [SerializeField] private float _searchSpeed = 1.5f;
    private NavMeshAgent _agent = null;
    private BehaviourState _state = BehaviourState.Wander;
    private WanderData _wanderData = new WanderData();
    private SeekData _seekData = new SeekData();

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 Destination)
    {
        _agent.SetDestination(Destination);
    }

    private void Update()
    {
        switch (_state)
        {

            case BehaviourState.Wander: DoWander(); break;
            case BehaviourState.Seek: DoSeek(); break;
            default:
                break;

        }

        UpdateAnimationStates();
    }

    //check for target

    private bool CanSeekTarget()
    {
        if (_targetObject != null)
        {
            Vector3 direction = _targetObject.transform.position - transform.position;
            direction.Normalize();
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, direction, out hitInfo, _viewDistance))
            {
                // FIX: Check if the collider is NOT null
                if (hitInfo.collider != null)
                {
                    if (hitInfo.collider.gameObject == _targetObject)
                    {
                        Debug.DrawLine(transform.position, hitInfo.point, Color.green, 1);
                        return true;
                    }
                }
                Debug.DrawLine(transform.position, hitInfo.point, Color.red, 1);
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + direction * _viewDistance, Color.red, 1.0f);
            }
        }

        return false;
    }

    private void StartWander()
    {
        WaypointScript waypoint = WaypointManager.Instance.GetRandomWaypoint();
        _wanderData.currentTarget = waypoint.transform.position;
        _wanderData.updateTime = Random.Range(0, _wanderData.minUpdateTime);
        SetDestination(_wanderData.currentTarget);
        _state = BehaviourState.Wander;
        _agent.speed = _walkSpeed;
    }

    private void DoWander()
    {
        if (CanSeekTarget())
        {
            StartSeek(); // start seeking
            return;
        }

        _wanderData.updateTime -= Time.deltaTime;

        // only pick new waypoint once agent stopped
        if (_wanderData.updateTime <= 0 || (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance))
        {
            StartWander();
        }
    }

    // seek state
    private void StartSeek()
    {
        _seekData.searchTime = _seekData.maxSearchTime;
        _seekData.targetPosition = _targetObject.transform.position;
        SetDestination(_seekData.targetPosition);
        _state = BehaviourState.Seek;
        _agent.speed = _runSpeed;
    }

    private void DoSeek()
    {
        if (!CanSeekTarget())
        {
            _seekData.searchTime -= Time.deltaTime;
            _agent.speed = _searchSpeed;
            if (_seekData.searchTime <= 0)
            {
                StartWander();
            }
        }
        else
        {
            _agent.speed = _runSpeed;
            _seekData.searchTime = _seekData.maxSearchTime;
            _seekData.targetPosition = _targetObject.transform.position;
        }
        SetDestination(_seekData.targetPosition);
    }

    // ANIMATIONS!
    private void UpdateAnimationStates()
    {
        bool isMoving = _agent.velocity.magnitude > _speedThreshold;
        bool walking = false;
        bool running = false;
        bool searching = false;

        if (isMoving)
        {
            if (_state == BehaviourState.Wander)
            {
                walking = true;
            }
            else if (_state == BehaviourState.Seek)
            {
                if (CanSeekTarget())
                {
                    running = true;
                }
                else
                {
                    searching = true;
                }
            }
        }

        _animator.SetBool("isMoving", isMoving);
        _animator.SetBool("walking", walking);
        _animator.SetBool("running", running);
        _animator.SetBool("searching", searching);
    }

}