using UnityEngine;

public class WaveStatus
{
    public bool IsWaveActive;
    public int WaveCount;
}

public class WaveController : MonoBehaviour
{
    private WaveStatus _status;
    private CreatureSpawnerController _creatureSpawnerController;

    public WaveController(WaveStatus status, CreatureSpawnerController creatureSpawnerController)
    {
        _status = status;
        _creatureSpawnerController = creatureSpawnerController;
    }

    private void StartWave()
    {
        _status.IsWaveActive = true;
    }

    private void OnEnemyDead()
    {
        _status.IsWaveActive = false;
    }
}