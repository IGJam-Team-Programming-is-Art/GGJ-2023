using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField] private bool _isMoving = false;
    [SerializeField] private Vector2 _currentDirection;

    public float Speed = 1f;
    private Ray cameraRay;
    Plane plane = new Plane(Vector3.up, 0);

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_isMoving is false)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        var velocity = new Vector3(_currentDirection.x, 0f, _currentDirection.y) * Speed;
        _rb.velocity = velocity;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _isMoving = !context.canceled;
        _currentDirection = context.ReadValue<Vector2>().normalized;
    }

    public void OnLookTargeted(InputAction.CallbackContext context)
    {
        var mousepos = context.ReadValue<Vector2>();
        var target = Extensions.GetGroundPoint(mousepos);
        transform.LookAt(target, Vector3.up);
    }

    public void OnLookDirectional(InputAction.CallbackContext context)
    {
        var dir = context.ReadValue<Vector2>();
        var target = transform.position + dir.to3D();
        transform.LookAt(target, Vector3.up);
    }
}