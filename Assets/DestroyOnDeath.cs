using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    private Hitpoints _hitpoints;

    // Start is called before the first frame update
    void Awake()
    {
        _hitpoints = GetComponent<Hitpoints>();
        _hitpoints.OnDeath += () => Destroy(gameObject);
    }
}
