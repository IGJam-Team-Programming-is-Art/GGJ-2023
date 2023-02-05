using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField] private bool _isMoving = false;
    [SerializeField] private Vector2 _currentMoveDirection;
    [SerializeField] private Vector2 _currentViewDirection;
    [SerializeField] private GameOverHandler _gameOverHandler;

    private PlayerInput input;

    public float Speed = 1f;
    private Ray cameraRay;
    private Plane plane = new(Vector3.up, 0);

    private void Awake()
    {
        if (_gameOverHandler != null)
        {
            _gameOverHandler.OnGameOver += OnGameOver;
        }
        _rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
    }

    private void OnGameOver()
    {
        input.enabled = false;
        this.enabled = false;
    }

    private void FixedUpdate()
    {
        if (_isMoving is false)
        {
            _rb.velocity = Vector3.zero + new Vector3(0, _rb.velocity.y, 0);
            return;
        }

        var velocity = new Vector3(_currentMoveDirection.x, 0f, _currentMoveDirection.y) * Speed;
        velocity.y = _rb.velocity.y;
        _rb.velocity = velocity;
    }

    private void Update()
    {
        if (_currentViewDirection.sqrMagnitude < 0.01f) return;
        var target = transform.position + _currentViewDirection.to3D();
        transform.LookAt(target, Vector3.up);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _isMoving = !context.canceled;
        _currentMoveDirection = context.ReadValue<Vector2>().normalized;
    }

    public void OnLookTargeted(InputAction.CallbackContext context)
    {
        var mousepos = context.ReadValue<Vector2>();
        var target = Extensions.GetGroundPoint(mousepos).to2D();
        var source = transform.position.to2D();
        _currentViewDirection = (target - source).normalized;
    }

    public void OnLookDirectional(InputAction.CallbackContext context)
    {
        _currentViewDirection = context.ReadValue<Vector2>();
    }
}