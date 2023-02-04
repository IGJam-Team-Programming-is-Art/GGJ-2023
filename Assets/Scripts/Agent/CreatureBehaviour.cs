using UnityEngine;
using VContainer;

public class CreatureBehaviour : MonoBehaviour
{
    private TargetAssignmentController _targetAssignmentController;

    public TargetAssignmentController TargetAssignmentController
    {
        set => _targetAssignmentController = value;
    }

    private AgentMovement _agentMovement;

    private void Awake()
    {
        enabled = false;
        _agentMovement = GetComponent<AgentMovement>();
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
        _agentMovement.SetTarget(_targetAssignmentController.GetTarget());
    }
}