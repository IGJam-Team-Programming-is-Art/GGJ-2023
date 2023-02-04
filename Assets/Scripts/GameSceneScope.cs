using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameSceneScope : LifetimeScope
{
    [SerializeField] private GameObject _player;

    [SerializeField] private CreatureDataCollection _creatureDataCollection;
    [SerializeField] private CreatureSpawnerCollection _creatureSpawnerCollection;

    protected override void Configure(IContainerBuilder builder)
    {
        //TODO: Replace with reference in final scene
        if (_player == null)
            _player = GameObject.FindWithTag("Player");

        builder.RegisterInstance<PlayerReferences>(new()
        {
            GameObject = _player,
            Hitpoints = _player.GetComponent<Hitpoints>()
        });

        builder.RegisterInstance(_creatureDataCollection);
        builder.RegisterInstance(_creatureSpawnerCollection);

        RegisterController(builder);
    }

    private void RegisterController(IContainerBuilder builder)
    {
        builder.Register<TargetAssignmentController>(Lifetime.Scoped);
        builder.Register<WaveController>(Lifetime.Scoped);
    }
}

/// <summary>
///     Stores references to player components
/// </summary>
public struct PlayerReferences
{
    public GameObject GameObject;
    public Hitpoints Hitpoints;
}