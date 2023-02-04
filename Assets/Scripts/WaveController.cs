using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

public class WaveSettings
{
}

public class WaveStatus
{
    public bool IsWaveActive;
    public int TotalWaveCount;

    public int ExistingSpawnerTargetAmount = 5;
    public int ExistingCreaturesTargetAmount = 10;

    /// <summary>
    /// Amount of target of total spawned spawners for one wave
    /// </summary>
    public int CreatedSpawnerTargetAmount;

    public int AlreadyCreatedSpawnerCount;
    public bool IsSpawnerTargetReached => AlreadyCreatedSpawnerCount >= CreatedSpawnerTargetAmount;
}

public class WaveController : IDisposable, IStartable
{
    private readonly CancellationTokenSource _cts;

    private readonly WaveStatus _waveStatus;
    private readonly SpawnerStatus _spawnerStatus;
    private readonly CreatureSpawnerController _creatureSpawnerController;

    public WaveController(WaveStatus waveStatus,
        SpawnerStatus spawnerStatus,
        CreatureSpawnerController creatureSpawnerController)
    {
        _cts = new();

        _waveStatus = waveStatus;
        _spawnerStatus = spawnerStatus;
        _creatureSpawnerController = creatureSpawnerController;

        _creatureSpawnerController.OnPotentialWaveEndEvent += OnWaveOver;
    }

    public void Start()
    {
        StartWave();
    }

    private void StartWave()
    {
        _waveStatus.TotalWaveCount += 1;
        _waveStatus.IsWaveActive = true;

        CreateSpawners(_cts.Token).Forget();
    }

    /// <summary>
    /// Creates spawners over time, checking regularly if new spawners are needed
    /// Will spawn until a total spawner target value is reached
    /// </summary>
    /// <param name="ct"></param>
    private async UniTaskVoid CreateSpawners(CancellationToken ct)
    {
        const double checkIntervalSeconds = 2f;
        _waveStatus.CreatedSpawnerTargetAmount = 5;

        while (ct.IsCancellationRequested is false && _waveStatus.IsSpawnerTargetReached is false)
        {
            var activeSpawnerCount = _spawnerStatus.ActiveSpawnerCount;
            var simultaneousSpawnerTargetAmount = _waveStatus.ExistingSpawnerTargetAmount;

            //If we have less spawners than desired, spawn one
            var lessSpawnerThanDesired = simultaneousSpawnerTargetAmount > activeSpawnerCount;
            if (lessSpawnerThanDesired && Random.value > 0.5f)
            {
                //Aggro is increased if the difference between desired and existing spawners is higher
                var aggro = GetAggro();
                CreateSpawner(aggro);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(checkIntervalSeconds), cancellationToken: ct)
                .SuppressCancellationThrow();
        }
    }

    private float GetAggro()
    {
        var existingSpawnersWeight = 1 -
                                     Mathf.InverseLerp(0, _waveStatus.ExistingSpawnerTargetAmount,
                                         _spawnerStatus.ActiveSpawnerCount);
        var existingEnemiesWeight = 1 - Mathf.InverseLerp(0, _waveStatus.ExistingCreaturesTargetAmount,
            _spawnerStatus.ActiveCreatureCount);

        return math.clamp(existingSpawnersWeight + existingEnemiesWeight, 0, 1);
    }

    /// <summary>
    /// Creates spawner, updates counter
    /// </summary>
    /// <param name="aggro"></param>
    private void CreateSpawner(float aggro)
    {
        _creatureSpawnerController.CreateSpawner(aggro);
        _waveStatus.AlreadyCreatedSpawnerCount += 1;
    }

    /// <summary>
    /// Checks if all spawners and enemies have been eliminated, if so then ends wave
    /// </summary>
    private void OnWaveOver()
    {
        if (_spawnerStatus.IsWaveActive && _waveStatus.IsSpawnerTargetReached)
            _waveStatus.IsWaveActive = false;
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}