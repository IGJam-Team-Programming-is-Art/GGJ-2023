using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameSceneScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        var player = GameObject.FindWithTag("Player");
        
        builder.RegisterInstance<PlayerReferences>(new()
        {
            Hitpoints = player.GetComponent<Hitpoints>()
        });
    }
}

/// <summary>
/// Stores references to player components
/// </summary>
public struct PlayerReferences
{
    public Hitpoints Hitpoints;
}