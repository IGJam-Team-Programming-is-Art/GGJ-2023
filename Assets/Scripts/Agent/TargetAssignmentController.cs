using System;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

/// <summary>
///     Handles target assignment for agents
/// </summary>
[UsedImplicitly]
public class TargetAssignmentController
{
    private Collider[] _collider;
    private string _layer = "Tower";

    private readonly PlayerReferences _playerReferences;
    private readonly TreeReferences _treeReferences;

    public TargetAssignmentController(PlayerReferences playerReferences, TreeReferences treeReferences)
    {
        _playerReferences = playerReferences;
        _treeReferences = treeReferences;

        _collider = new Collider[10];
    }

    public Transform GetTarget(Transform agent, [CanBeNull] Transform currentTarget, float targetingDistance)
    {
        //If no close object found
        var playerReferencesTransform = _playerReferences.Transform;
        var agentPosition = agent.position;

        //Try to keep current target most of the time
        var selectNewTargetChance = Random.Range(0f, 1f);
        if (currentTarget != null && selectNewTargetChance < 0.9 &&
            Vector3.Distance(agent.position, currentTarget.position) <= targetingDistance)
            return currentTarget;

        //Search all surrounding turrets
        _collider = Physics.OverlapSphere(agentPosition, targetingDistance, LayerMask.GetMask(_layer));
        if (_collider.Length is 0)
            return _treeReferences.Transform;

        Array.Sort(_collider,
            (a, b) => (int)Mathf.Sign(Vector3.Distance(agentPosition, b.transform.position) -
                                      Vector3.Distance(agentPosition, a.transform.position)));
        var selectedTransform = _collider[0].transform;
        Assert.IsFalse(selectedTransform.IsDestroyed());
        return selectedTransform;
    }
}