using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    private Projectile _projectile;
    private void Awake()
    {
        _projectile = GetComponent<Projectile>();
        _projectile.OnHit += OnHit;
    }

    private void OnHit(GameObject target)
    {
        target.GetComponent<Hitpoints>()?.Modify(-_projectile.Damage);
    }
}
