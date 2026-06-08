using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AiMouseClickController : MonoBehaviour
{
    [SerializeField] private Transform _targetWaypoint = null;
    [SerializeField] private NavMeshAgent _agent = null;
    private PlayerInput _playerInput = null;
    private InputAction _mouseClick = null;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _mouseClick = _playerInput.Player.Attack;
        _mouseClick.performed += OnMouseClick;
    }

    private void OnEnable()
    {
        _mouseClick.Enable();
    }

    private void OnDisable()
    {
        _mouseClick.Disable();
    }

    void OnMouseClick(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray cameraRay = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(cameraRay, out hitInfo))
        {
            if (hitInfo.collider != null)
            {
                _targetWaypoint.position = hitInfo.point;
                _agent.destination = hitInfo.point;
            }
        }
    }

}
