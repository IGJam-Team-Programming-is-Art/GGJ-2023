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

        RegisterScriptableObjects(builder);
        RegisterModel(builder);
        RegisterController(builder);
    }

    private void RegisterScriptableObjects(IContainerBuilder builder)
    {
        builder.RegisterInstance(_creatureDataCollection);
        builder.RegisterInstance(_creatureSpawnerCollection);
    }

    private void RegisterModel(IContainerBuilder builder)
    {
        builder.Register<WaveStatus>(Lifetime.Scoped);
        builder.Register<SpawnerStatus>(Lifetime.Scoped);
    }

    private void RegisterController(IContainerBuilder builder)
    {
        builder.Register<TargetAssignmentController>(Lifetime.Scoped);
        builder.RegisterComponentInHierarchy<CreatureSpawnerController>();
        builder.RegisterEntryPoint<WaveController>(Lifetime.Scoped).AsSelf();
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