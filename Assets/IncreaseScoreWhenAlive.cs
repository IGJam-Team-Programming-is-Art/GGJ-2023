using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseScoreWhenAlive : MonoBehaviour
{
    [SerializeField] PlayerScore Score;
    [SerializeField] float Interval = 5f;
    [SerializeField] int Amount = 5;

    private Hitpoints _hitpoints;
    private float _timestamp;

    void Awake()
    {
        _hitpoints = GetComponent<Hitpoints>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _timestamp = Time.time + Interval;
        _hitpoints.OnDeath += () => Destroy(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_timestamp > Time.time)
        {
            return;
        }
        _timestamp = Time.time + Interval;
        Score.IncreaseScore(Amount);
    }
}
