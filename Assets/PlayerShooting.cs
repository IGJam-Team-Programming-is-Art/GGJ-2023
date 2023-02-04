using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerShooting : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnShootTargeted(InputAction.CallbackContext context)
    {
        var screenPos = context.ReadValue<Vector2>();
    }

    public void OnShootDirectional(InputAction.CallbackContext context)
    {
        var dir = context.ReadValue<Vector2>();
    }
}
