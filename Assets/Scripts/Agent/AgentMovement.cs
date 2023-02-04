using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMovement : MonoBehaviour
{
    [SerializeField] private float _stoppingDistance = 2f;

    private NavMeshAgent _navMeshAgent;

    [field: SerializeField] public GameObject TargetObject { get; private set; }
    [field: SerializeField] public Vector3 TargetPosition { get; private set; }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        _navMeshAgent.stoppingDistance = _stoppingDistance;
    }

    /// <summary>
    ///     Sets target object for agent
    /// </summary>
    /// <param name="obj"></param>
    public void SetTarget(GameObject obj)
    {
        var centerPosition = obj.transform.position;

        if (NavMesh.SamplePosition(centerPosition, out var hit, 2f, NavMesh.AllAreas) is false)
        {
            Debug.LogError("Cannot move to position!");
            _navMeshAgent.isStopped = true;
            return;
        }

        _navMeshAgent.isStopped = false;

        TargetObject = obj;
        TargetPosition = hit.position;

        _navMeshAgent.SetDestination(TargetPosition);
    }
}