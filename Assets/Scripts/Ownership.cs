using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OwnerType
{
    Player,
    Enemy
}

public enum Relationship
{
    Self,
    Friendly,
    Neutral,
    Hostile
}

public class Ownership : MonoBehaviour
{
    public OwnerType Owner;

    public Relationship GetRelationship(GameObject target)
    {
        if (target == gameObject)
        {
            return Relationship.Self;
        }
        var targetOwnership = target.GetComponentInParent<Ownership>() ?? target.GetComponentInChildren<Ownership>();
        if (targetOwnership == null)
        {
            return Relationship.Neutral;
        }
        if (Owner == OwnerType.Player && targetOwnership.Owner == OwnerType.Enemy)
        {
            return Relationship.Hostile;
        }
        if (Owner == OwnerType.Enemy && targetOwnership.Owner == OwnerType.Player)
        {
            return Relationship.Hostile;
        }
        return Relationship.Friendly;
    }

    public static Relationship GetRelationship(GameObject source, GameObject target)
    {
        if (target.transform.IsSameOrChildOf(source.transform) || source.transform.IsSameOrChildOf(target.transform))
        {
            return Relationship.Self;
        }
        Ownership ownerSource = source.GetComponentInParent<Ownership>() ?? source.GetComponentInChildren<Ownership>();
        if (ownerSource == null)
        {
            return Relationship.Neutral;
        }
        return ownerSource.GetRelationship(target);
    }
}
