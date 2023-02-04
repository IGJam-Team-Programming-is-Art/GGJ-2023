using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerShooting : MonoBehaviour
{
    private Rigidbody _rb;
    private WeaponUser _weaponUser;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _weaponUser = GetComponent<WeaponUser>();
    }

    public void OnShootTargeted(InputAction.CallbackContext context)
    {
        var screenPos = Mouse.current.position.ReadValue();
        var target = Extensions.GetGroundPoint(screenPos);
        target.y = 1;
        _weaponUser.UseWeapon(target);
    }

    public void OnShootDirectional(InputAction.CallbackContext context)
    {
        //TODO
        var angle = transform.rotation.eulerAngles.y;
    }
}
