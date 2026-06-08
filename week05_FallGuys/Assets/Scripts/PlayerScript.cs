using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [Serializable] public class AudioEffects
    {
        public AudioClip footStep = null;
        public AudioClip jump = null;
        public AudioClip respawn = null;

        public float stepRate = 0.15f;
    }

    [SerializeField] private AudioEffects _audioEffects = new AudioEffects();
    [SerializeField] private AudioSource _audioSource = null;

    [SerializeField] private ParticleSystem _particleSystem = null;

    [SerializeField] private Transform _shoulderTransform = null;
    [SerializeField] private GroundCheckScript _groundCheck = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private float _turnSpeed = 1.0f;
    [SerializeField] private float _pitchSpeed = 1.0f; // added to reduce vertical camera speed
    [SerializeField] private float _maxPitchAngle = 70.0f;
    [SerializeField] private float _minPitchAngle = -70.0f;
    [SerializeField] private float _jumpSpeed = 10.0f;
    [SerializeField] private float _gravityMultiplier = 3.0f; // added for less floaty jumping
    [SerializeField] private bool _invertY = true;


    private Rigidbody _rigidbody = null;
    private PlayerInput _playerInput = null;
    private InputAction _lookAction = null;
    private InputAction _moveAction = null;
    private InputAction _jumpAction = null;
    private Vector3 _moveVelocity = Vector3.zero;
    private Vector3 _moveRotation = Vector3.zero;
    private Vector3 _lastGroundedPos = Vector3.zero;
    private float _pitchRotation = 0.0f;
    private float _lastStepAudioTime = 0.0f;
    private int _initialFrameSkip = 2;
    private bool _isJumping = false;
    private bool _isFalling = false;
    private bool _controlDisabled = false;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerInput = new PlayerInput();
        _moveAction = _playerInput.Player.Move;
        _lookAction = _playerInput.Player.Look;
        _jumpAction = _playerInput.Player.Jump;
        _jumpAction.performed += OnJump;

        // hide cursor and lock to screen (press escape to show cursor again);
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _lookAction.Enable();
        _jumpAction.Enable();
    }
    private void OnDisable()
    {
        _moveAction.Disable();
        _lookAction.Disable();
        _jumpAction.Disable();
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

        //particle effect here
        if (_groundCheck.IsGrounded && _moveVelocity.sqrMagnitude > 0.0f)
        {
            _particleSystem.Play();
        }
        else
        {
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        if (_groundCheck.IsGrounded && _rigidbody.linearVelocity.y < 0.1f)
        {
            _lastGroundedPos = transform.position;
            _isJumping = false;
            _isFalling = false;
            //playing step audio
            //if (_groundCheck.IsGrounded && _moveVelocity.sqrMagnitude > 0.0f)
            //{
            //    _audioSource.pitch = _audioEffects.stepRate;
            //    if (!_audioSource.isPlaying)
            //    {
            //        _audioSource.clip = _audioEffects.footStep;
            //        _audioSource.loop = true;
            //        _audioSource.Play();
            //    }
            //}
            //else
            //{
            //    _audioSource.Stop();
            //}
        }
        else if (_rigidbody.linearVelocity.y < -0.1f)
        {
            _isFalling = true;
            _isJumping = false;
        }

        _animator.SetBool("Moving", _moveVelocity.magnitude > 0.1f);
        _animator.SetBool("Jumping", _isJumping);
        _animator.SetBool("Falling", _isFalling);
        _animator.SetFloat("Run", moveInput.y);
        _animator.SetFloat("Strafe", moveInput.x);


    }

    private void FixedUpdate()
    {
        if (_controlDisabled) { return; } 

        _moveVelocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = _moveVelocity;
        _rigidbody.angularVelocity = _moveRotation;
        _shoulderTransform.localRotation = Quaternion.Euler(_pitchRotation, 0.0f, 0.0f);

        //make jumping less floaty
        _rigidbody.AddForce(Physics.gravity * (_gravityMultiplier - 1f) * _rigidbody.mass);
    }

    private void OnJump(InputAction.CallbackContext callbackContext)
    {
        if (_rigidbody.linearVelocity.y < 0.1f && _groundCheck.IsGrounded)
        {
            Vector3 jumpVelocity = _rigidbody.linearVelocity;
            jumpVelocity.y = _jumpSpeed;
            _rigidbody.linearVelocity = jumpVelocity;
            _isJumping = true;

            _audioSource.PlayOneShot(_audioEffects.jump);
        }
    }

    public void Respawn()
    {
        Checkpoint checkpoint = CheckpointManager.Instance.GetSavedCheckPoint();
        if (checkpoint != null)
        {
            _lastGroundedPos = checkpoint.transform.position;
        }
        _rigidbody.linearVelocity = Vector3.zero;
        _lastGroundedPos.y += 1.0f;
        transform.position = _lastGroundedPos;
        _audioSource.PlayOneShot(_audioEffects.respawn);
    }

    
    public void DisableControl() // for when game over, freeze player and make it dance (jump anim)
    {
        _controlDisabled = true;
        _moveAction.Disable();
        _lookAction.Disable();
        _jumpAction.Disable();
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _animator.SetBool("Moving", false);
        _animator.SetBool("Falling", false);
        _animator.SetBool("Jumping", true);
    }
}
