using UnityEngine;

[CreateAssetMenu(menuName = "Create WaveSettings", fileName = "GameData/WaveSettings", order = 0)]
public class WaveSettings : ScriptableObject
{
    public float StartCalmPeriodSeconds = 4f;
    public float CalmPeriodDurationSeconds = 15f;

    public int ExistingSpawnerTargetAmount = 5;
    public int ExistingCreaturesTargetAmount = 10;

    /// <summary>
    /// Amount of target of total spawned spawners for one wave
    /// </summary>
    public int CreatedSpawnerTargetAmount = 3;
}