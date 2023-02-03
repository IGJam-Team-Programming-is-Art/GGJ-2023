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
    public float Cooldown;
    public float Preswing;  //Wait time between using the Weapon and actually shooting
    public float Damage;
    public WeaponType Type;

    //Projectile / Range Weapons
    public GameObject Projectile;
    public float Speed;

    //Instant / Melee Weapons
    public float Width; //InstantRect
    public float Radius_Length; //InstantRect and InstantArc
    public float Arc; //InstantArc
}
