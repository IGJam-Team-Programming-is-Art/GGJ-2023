using System;
using UnityEngine;

[RequireComponent(typeof(Hitpoints))]
public class CreatureBehaviour : MonoBehaviour
{
    [field: SerializeField] public Creature CreatureType { get; private set; }

    private Hitpoints _hitpoints;
    private AgentMovement _agentMovement;
    private TargetAssignmentController _targetAssignmentController;

    public TargetAssignmentController TargetAssignmentController
    {
        set => _targetAssignmentController = value;
    }

    public event Action<CreatureBehaviour> DeathEvent;

    private void Awake()
    {
        enabled = false;
        _agentMovement = GetComponent<AgentMovement>();

        _hitpoints = GetComponent<Hitpoints>();
        _hitpoints.OnDeath += OnDeath;
    }

    private void OnEnable()
    {
        SelectTarget();
    }

    /// <summary>
    ///     Find and select target to attack if one is found
    /// </summary>
    private void SelectTarget()
    {
        _agentMovement.SetTarget(_targetAssignmentController.GetTarget(), true);
    }

    private void OnDeath()
    {
        enabled = false;
        DeathEvent?.Invoke(this);
    }
}