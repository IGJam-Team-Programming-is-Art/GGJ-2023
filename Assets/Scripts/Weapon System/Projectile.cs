using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float Speed;
    public int Damage;
    public float LifeTime;
    public GameObject Source;
    public List<Relationship> TargetedRelationships;
    public event Action<GameObject> OnHit; //Param: The Projectile that hit and the GameObject that was hit. GameObject might also be null

    private Rigidbody _rb;
    private float _lifeEndTimestamp;
    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _lifeEndTimestamp = Time.time + LifeTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = transform.forward * Speed;
        if (_lifeEndTimestamp < Time.time)
        {
            Destroy(gameObject);
        }
    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        var relationship = Ownership.GetRelationship(Source, other.gameObject);
        if (!TargetedRelationships.Contains(relationship))
        {
            return;
        }

        OnHit?.Invoke(other.gameObject);
    }
}
