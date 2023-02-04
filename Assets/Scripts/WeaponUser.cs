using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WeaponUser : MonoBehaviour
{
    public Weapon CurrentWeapon;

    //Connection to Input System
    private Vector2 CurrentTargetPoint;
    private bool ShootButtonPressed;

    //Gameplay Variables
    private float CooldownEndTimeStamp;

    private void FixedUpdate()
    {
        if (ShootButtonPressed)
        {
            UseWeapon(CurrentTargetPoint);
        }
    }

    public void UseWeapon(Vector3 targetPoint)
    {
        if (!CurrentWeapon)
        {
            Debug.LogError("Shot without weapon!");
            return;
        }
        if (CooldownEndTimeStamp > Time.time)
        {
            Debug.Log("Wanted to shoot, but cooldown was still running");
            return;
        }
        CooldownEndTimeStamp = Time.time + CurrentWeapon.Cooldown;

        //Wait Pressing Duration, then do actual Shot
        UniTask.Void(async target =>
        {
            await UniTask.Delay(TimeSpan.FromSeconds(CurrentWeapon.Preswing));
            Shoot(target);
        }, targetPoint);
    }

    private void Shoot(Vector3 targetPoint)
    {
        if (CurrentWeapon.Type == WeaponType.Projectile)
        {
            //TODO: 
            // Spawn Projectile (as setup in Weapon)
            Instantiate(CurrentWeapon.Projectile, targetPoint, Quaternion.identity);
            // Set Speed and Damage (from CurrentWeapon Values)
            // Turn Projectile such that forward vector points at targetPoint
        }
        else if (CurrentWeapon.Type == WeaponType.InstantArc)
        {
            //TODO
            // Select all enemies in Area defined by Arc and Radius_Length (should be a Arc Circle)
            // Damage all selected enemies
        }
        else if (CurrentWeapon.Type == WeaponType.InstantArc)
        {
            //TODO
            // Select all enemies in Area defined by Width and Radius_Length (should be a Rectangle Region)
            // Damage all selected enemies
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        // if (weapon == CurrentWeapon)
        // {
        //     return;
        // }
        CurrentWeapon = weapon;
        CooldownEndTimeStamp = 0f;
    }
}
