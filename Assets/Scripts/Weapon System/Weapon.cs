using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Projectile,
    InstantArc,
    InstantRect,

}

[CreateAssetMenu(menuName = "GameData/Weapon")]
public class Weapon : ScriptableObject
{
    public float Cooldown = 0.5f;
    public float InitialDelay = 0.1f;  //Wait time between using the Weapon and actually shooting
    public int Damage = 5;
    public WeaponType Type = WeaponType.Projectile;
    public List<Relationship> ValidTargets = new();

    //Projectile / Range Weapons
    public GameObject Projectile;
    public float Speed = 10f;
    public float LifeTime = 4f;

    //Instant / Melee Weapons
    public float Width; //InstantRect
    public float Radius_Length; //InstantRect and InstantArc
    public float Arc; //InstantArc
}
