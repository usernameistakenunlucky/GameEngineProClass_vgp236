using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerStats
{
    public float _moveSpeed = 5.0f;
    public float _jumpSpeed = 3.0f;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    [SerializeField] private PlayerStats _playerStats = new PlayerStats();
    [SerializeField] private GroundCheckScript _groundCheck = null;

    private Rigidbody2D _rigidBody = null;
    private float _desiredMoveSpeed = 0.0f;
    private float _facingDirection = 0;
    private bool _isFalling = false;
    private bool _isJumping = false;

    //play input hookups
    private PlayerInput _playerInput = null;
    private InputAction _moveAction = null;
    private InputAction _jumpAction = null;


    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = new PlayerInput();
        _moveAction = _playerInput.Player.Move;
        _jumpAction = _playerInput.Player.Jump;
        _jumpAction.performed += OnJump;
    }

    private void OnEnable() //need to manually enable and disable actions because we are controlling it all
    {
        _moveAction.Enable();
        _jumpAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction?.Disable();
        _jumpAction?.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        _desiredMoveSpeed = _moveAction.ReadValue<Vector2>().x * _playerStats._moveSpeed;
        if (_rigidBody.linearVelocityY < -0.1f && !_groundCheck.IsGrounded) // in air, going down, so falling
        {
            _isFalling = true;
            _isJumping = false;
        }
        else if (_isFalling && _groundCheck.IsGrounded && _rigidBody.linearVelocityY < 0.1f) //lands on ground, stop falling
        {
            _isFalling = false;
        }
    }

    private void FixedUpdate()
    {
        _rigidBody.linearVelocityX = _desiredMoveSpeed;
        if (MathF.Abs(_desiredMoveSpeed) > 0.1f)
        {
            _facingDirection = (_desiredMoveSpeed) > 0.0f ? 0.0f : 1.0f;
        }
        _animator.SetFloat("Speed", MathF.Abs(_desiredMoveSpeed));
        _animator.SetFloat("Direction", _facingDirection);
        _animator.SetBool("Jump", _isJumping);
        _animator.SetBool("Fall", _isFalling);
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_groundCheck.IsGrounded && _rigidBody.linearVelocityY < 0.1f)
        {
            _rigidBody.linearVelocityY = _playerStats._jumpSpeed;
            _isJumping = true;
        }
    }
}
