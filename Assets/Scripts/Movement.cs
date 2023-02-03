using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    
    public float Speed = 1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            //Stopped movement
            return;
        }
        
        var direction = context.ReadValue<Vector2>().normalized;
        var velocity = Speed * direction;

        _rb.velocity = velocity;
    }
}

// public class Movement : MonoBehaviour
// {
//     public float SpeedPerSecond = 1f;
//     
//     public Vector3 Target { get; private set; } 
//     
//     public void SetTarget(Vector3 target)
//     {
//         Target = target;
//     }
//     
//     public void StopMovement()
//     {
//         Target = transform.position;
//     }
// }