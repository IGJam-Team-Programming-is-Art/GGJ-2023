using JetBrains.Annotations;
using UnityEngine;

/// <summary>
///     Handles target assignment for agents
/// </summary>
[UsedImplicitly]
public class TargetAssignmentController
{
    private readonly PlayerReferences _playerReferences;

    public TargetAssignmentController(PlayerReferences playerReferences)
    {
        _playerReferences = playerReferences;
    }

    public GameObject GetTarget()
    {
        return _playerReferences.GameObject;
    }
}