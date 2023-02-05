using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMovement : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    [SerializeField] private float _normalSpeed;

    [SerializeField] [Obsolete("No moving targets right now")]
    private bool _isMoving;

    [field: SerializeField] public Transform Target { get; private set; } = null;
    [field: SerializeField] public Collider TargetCollider { get; private set; } = null;
    [field: SerializeField] public Vector3 TargetPosition { get; private set; }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _normalSpeed = _navMeshAgent.speed;
    }

    private void Update()
    {
        if (_isMoving) _navMeshAgent.SetDestination(Target.position);
    }

    /// <summary>
    ///     Sets target object for agent
    /// </summary>
    /// <param name="targetTransform"></param>
    /// <param name="isMoving"></param>
    public void SetTarget(Transform targetTransform, bool isMoving)
    {
        _isMoving = isMoving;
        _navMeshAgent.isStopped = false;

        Target = targetTransform;
        TargetCollider = targetTransform.GetComponent<Collider>();
        TargetPosition = TargetCollider.ClosestPoint(transform.position);

        _navMeshAgent.SetDestination(TargetPosition);
    }

    /// <summary>
    /// Toggle movement, but not rotation towards target
    /// </summary>
    public void ToggleMovement(bool setActive)
    {
        _navMeshAgent.speed = setActive ? _normalSpeed : 0;
    }
}