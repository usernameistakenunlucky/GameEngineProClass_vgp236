using UnityEngine;
using UnityEngine.AI;

public class AgentAnimationLinker : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    [SerializeField] private float _speedThreshold = 0.1f;

    private NavMeshAgent _navAgent = null;
    private Rigidbody _rigidbody = null;
    private Vector3 _lastPosition = Vector3.zero;

    void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        _navAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 worldVelocity = GetAgentVelocity();
        Vector3 localVelocity = transform.InverseTransformDirection(worldVelocity);

        bool isMoving = worldVelocity.magnitude > _speedThreshold;

        _animator.SetBool("Moving", isMoving);
        _animator.SetFloat("Run", localVelocity.z);
        _animator.SetFloat("Strafe", localVelocity.x);
    }

    private Vector3 GetAgentVelocity()
    {
        if (_navAgent != null)
        {
            return _navAgent.velocity;
        }

        if (_rigidbody != null && !_rigidbody.isKinematic)
        {
            return _rigidbody.linearVelocity;
        }

        // Manual fallback for custom script movement
        Vector3 displacementVelocity = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;
        return displacementVelocity;
    }
}