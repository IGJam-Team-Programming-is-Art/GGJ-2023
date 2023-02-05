using UnityEngine;

[CreateAssetMenu(menuName = "Create SpawnerSettings", fileName = "GameData/SpawnerSettings", order = 0)]
public class SpawnerSettings : ScriptableObject
{
    public int MaxCreatureSpawnAmount = 3;

    public float SpawnPulseInterval = 5f;
    public float SpawnPulseIntervalVariation = 0.3f;
    public int AmountPerPulseRangeStart = 0;
    public int AmountPerPulseRangeEnd = 2;
}