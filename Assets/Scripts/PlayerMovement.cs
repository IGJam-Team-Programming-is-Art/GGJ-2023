using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField] private bool _isMoving = false;
    [SerializeField] private Vector2 _currentDirection;

    public float Speed = 1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _isMoving = !context.canceled;
        _currentDirection = context.ReadValue<Vector2>().normalized;
    }

    private void FixedUpdate()
    {
        if (_isMoving is false)
            return;
        
        var velocity = new Vector3(_currentDirection.x, 0f, _currentDirection.y) * Speed;
        _rb.velocity = velocity;
    }
}