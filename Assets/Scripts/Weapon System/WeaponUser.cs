using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;



public class WeaponUser : MonoBehaviour
{
    const string ProjectileSpawnPointTag = "ProjectileSpawn";
    public Weapon CurrentWeapon;
    public event Action OnPreswing;

    //Gameplay Variables
    private float _cooldownEndTimeStamp;
    private GameObject _projectileSpawnPoint;
    private Ownership _ownership;

    private void Awake()
    {
        _projectileSpawnPoint = transform.FindRecursiveByTag(ProjectileSpawnPointTag)?.gameObject;
        _ownership = GetComponent<Ownership>();
        Debug.Log($"Projectile Spawn: {_projectileSpawnPoint}");
    }

    public void UseWeapon(Vector3 targetPoint)
    {
        if (!CurrentWeapon)
        {
            Debug.LogError("Shot without weapon!");
            return;
        }
        if (_cooldownEndTimeStamp > Time.time)
        {
            Debug.Log("Wanted to shoot, but cooldown was still running");
            return;
        }
        _cooldownEndTimeStamp = Time.time + CurrentWeapon.Cooldown;

        //Wait Pressing Duration, then do actual Shot
        UniTask.Void(async target =>
        {
            await UniTask.Delay(TimeSpan.FromSeconds(CurrentWeapon.InitialDelay));
            Shoot(target);
        }, targetPoint);
    }

    private void Shoot(Vector3 targetPoint)
    {
        if (CurrentWeapon.Type == WeaponType.Projectile)
        {
            // Spawn Projectile (as setup in Weapon)
            Vector3 spawnPoint = GetProjectileSpawnPoint();
            var projectileObject = Instantiate(CurrentWeapon.Projectile, spawnPoint, Quaternion.identity);
            var projectile = projectileObject.GetComponent<Projectile>();
            if (projectile == null)
            {
                Debug.LogWarning("No Projectile found");
                return;
            }

            // Set Speed and Damage (from CurrentWeapon Values)7
            targetPoint.y = CurrentWeapon.TargetGround ? 0 : spawnPoint.y;
            projectile.Speed = CurrentWeapon.Speed;
            projectile.Damage = CurrentWeapon.Damage;
            projectile.TargetedRelationships = CurrentWeapon.ValidTargets;
            projectile.LifeTime = CurrentWeapon.LifeTime;
            projectile.Source = gameObject;

            // Turn Projectile such that forward vector points at targetPoint
            projectile.transform.LookAt(targetPoint, Vector3.up);
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

    private Vector3 GetProjectileSpawnPoint()
    {
        return _projectileSpawnPoint != null ? _projectileSpawnPoint.transform.position : transform.position;
    }

    public void SetWeapon(Weapon weapon)
    {
        // if (weapon == CurrentWeapon)
        // {
        //     return;
        // }
        CurrentWeapon = weapon;
        _cooldownEndTimeStamp = 0f;
    }
}
