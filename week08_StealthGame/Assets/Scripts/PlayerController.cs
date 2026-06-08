using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Transform _shoulderTransform = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private float _turnSpeed = 1.0f;
    [SerializeField] private float _pitchSpeed = 1.0f; // added to reduce vertical camera speed
    [SerializeField] private float _maxPitchAngle = 70.0f;
    [SerializeField] private float _minPitchAngle = -70.0f;
    [SerializeField] private bool _invertY = true;


    private Rigidbody _rigidbody = null;
    private PlayerInput _playerInput = null;
    private InputAction _lookAction = null;
    private InputAction _moveAction = null;
    private Vector3 _moveVelocity = Vector3.zero;
    private Vector3 _moveRotation = Vector3.zero;
    private float _pitchRotation = 0.0f;
    private int _initialFrameSkip = 2;
    private bool _controlDisabled = false;

    [SerializeField] private Transform _startSpawnPoint = null;

    [SerializeField] private GameObject _goldCrossVisual = null;
    [SerializeField] private GameObject _originalCrossPickup = null;
    private bool _hasGoldCross = false;
    public bool HasGoldCross => _hasGoldCross;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerInput = new PlayerInput();
        _moveAction = _playerInput.Player.Move;
        _lookAction = _playerInput.Player.Look;

        // hide cursor and lock to screen (press escape to show cursor again);
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _lookAction.Enable();
    }
    private void OnDisable()
    {
        _lookAction.Disable();
    }

    void Update()
    {
        if (_initialFrameSkip > 0 || _controlDisabled)
        {
            --_initialFrameSkip;
            return;
        }
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Vector2 lookInput = _lookAction.ReadValue<Vector2>();

        //WASD controls. now facing camera, strafe
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0.0f;
        right.y = 0.0f;
        _moveVelocity = ((forward * moveInput.y) + (right * moveInput.x)) * _speed;

        _moveRotation.y = lookInput.x * _turnSpeed;
        if (_invertY)
        {
            lookInput.y *= -1.0f;
        }
        _pitchRotation = Mathf.Clamp(_pitchRotation + lookInput.y * _pitchSpeed, _minPitchAngle, _maxPitchAngle);

        _animator.SetBool("moving", _moveVelocity.magnitude > 0.1f);

        // strafing logic
        if (moveInput.x > 0.1f)
        {
            _animator.SetBool("strafeRight", true);
            _animator.SetBool("strafeLeft", false);
        }
        else if (moveInput.x < -0.1f)
        {
            _animator.SetBool("strafeRight", false);
            _animator.SetBool("strafeLeft", true);
        }
        else
        {
            _animator.SetBool("strafeRight", false);
            _animator.SetBool("strafeLeft", false);
        }
    }

    private void FixedUpdate()
    {
        if (_controlDisabled) { return; }

        _moveVelocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = _moveVelocity;
        _rigidbody.angularVelocity = _moveRotation;
        _shoulderTransform.localRotation = Quaternion.Euler(_pitchRotation, 0.0f, 0.0f);
    }

    public void Respawn()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        if (_hasGoldCross) // drop the gold cross when grabbed
        {
            _hasGoldCross = false;

            if (_goldCrossVisual != null)
            {
                _goldCrossVisual.SetActive(false);
            }

            if (_originalCrossPickup != null)
            {
                _originalCrossPickup.SetActive(true);
            }
        }

        transform.position = _startSpawnPoint.position; // spawn back at the respawn transform block
    }

    public void DisableControl() // for when game over, freeze player and make it dance (jump anim)
    {
        _controlDisabled = true;
        _moveAction.Disable();
        _lookAction.Disable();
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _animator.SetBool("moving", false);
        _animator.SetBool("dancing", true);
    }

    public void PickUpGoldCross()
    {
        _hasGoldCross = true;

        if (_goldCrossVisual != null)
        {
            _goldCrossVisual.SetActive(true);
        }
    }
}