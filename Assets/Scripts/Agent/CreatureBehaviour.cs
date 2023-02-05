using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Hitpoints))]
public class CreatureBehaviour : MonoBehaviour
{
    [field: SerializeField] public Creature CreatureType { get; private set; }

    [SerializeField] private bool _isAttacking = false;

    private float _lastRetargetingTimestamp;
    [SerializeField] private float _retargetingInterval = 1f;

    private float _lastAttackCheckTimestamp;
    [SerializeField] private float _lastAttackCheckInterval = 0.5f;

    private Hitpoints _hitpoints;
    private AgentMovement _agentMovement;
    private TargetAssignmentController _targetAssignmentController;
    private NavMeshAgent _navMeshAgent;
    private WeaponUser _weaponUser;

    public TargetAssignmentController TargetAssignmentController
    {
        set => _targetAssignmentController = value;
    }

    public event Action<CreatureBehaviour> DeathEvent;

    private void Awake()
    {
        enabled = false;
        _agentMovement = GetComponent<AgentMovement>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _weaponUser = GetComponent<WeaponUser>();
        _hitpoints = GetComponent<Hitpoints>();

        _hitpoints.OnDeath += OnDeath;
    }

    private void OnEnable()
    {
        _hitpoints.ResetHitpoints();
        SelectTarget();

        _lastRetargetingTimestamp = Time.time;
    }

    private void Update()
    {
        if (_isAttacking is false && Time.time - _lastRetargetingTimestamp > _retargetingInterval)
        {
            _lastRetargetingTimestamp = Time.time;
            SelectTarget();
        }

        if (Time.time - _lastAttackCheckTimestamp > _lastAttackCheckInterval)
        {
            _lastAttackCheckTimestamp = Time.time;
            var isWithingAttackingDistance = _navMeshAgent.remainingDistance < 0.5;
            _isAttacking = isWithingAttackingDistance;
            if (isWithingAttackingDistance)
            {
                Debug.Log("Trying to shoot", gameObject);

                var variation = Random.insideUnitCircle * 0.2f;
                _weaponUser.UseWeapon(_agentMovement.Target.position + variation.to3D());
            }

            _agentMovement.ToggleMovement(!_isAttacking);
        }
    }

    /// <summary>
    ///     Find and select target to attack if one is found
    /// </summary>
    private void SelectTarget()
    {
        _agentMovement.SetTarget(
            _targetAssignmentController.GetTarget(transform, _agentMovement.Target,
                _navMeshAgent.stoppingDistance * 1.5f),
            true);
    }

    private void OnDeath()
    {
        enabled = false;
        DeathEvent?.Invoke(this);
    }
}