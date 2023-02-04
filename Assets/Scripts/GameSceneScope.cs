using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameSceneScope : LifetimeScope
{
    [SerializeField] private CreatureDataCollection _creatureDataCollection;
    [SerializeField] private CreatureSpawnerCollection _creatureSpawnerCollection;

    protected override void Configure(IContainerBuilder builder)
    {
        var player = GameObject.FindWithTag("Player");

        builder.RegisterInstance<PlayerReferences>(new()
        {
            GameObject = player,
            Hitpoints = player.GetComponent<Hitpoints>()
        });

        builder.RegisterInstance(_creatureDataCollection);
        builder.RegisterInstance(_creatureSpawnerCollection);

        RegisterController(builder);
    }

    private void RegisterController(IContainerBuilder builder)
    {
        builder.Register<TargetAssignmentController>(Lifetime.Scoped);
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