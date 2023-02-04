using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class ShootAtClosestEnemy : MonoBehaviour
{
    const string EnemyTag = "Enemy";
    private GameObject _currentTarget;
    private WeaponUser _weaponUser;

    // Update is called once per frame
    private void Awake()
    {
        _weaponUser = GetComponent<WeaponUser>();
    }
    void FixedUpdate()
    {
        //FIXME: Extremely Inefficient. Needs list of enemies instead of finding them, update of current Target shouldnt happen every fixed Update etc
        var enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
        _currentTarget = enemies.OrderBy(x => Vector3.SqrMagnitude(x.transform.position - transform.position)).FirstOrDefault();
        if (_currentTarget && _weaponUser)
        {
            _weaponUser.UseWeapon(_currentTarget.transform.position);
        }
    }

    void Update()
    {
        if (_currentTarget == null)
        {
            return;
        }

        transform.LookAt(_currentTarget.transform.position);

    }
}
