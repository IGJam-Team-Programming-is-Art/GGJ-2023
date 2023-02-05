using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer.Unity;
using Random = UnityEngine.Random;

public class WaveStatus
{
    public bool IsWaveActive;
    public int TotalWaveCount;

    public int AlreadyCreatedSpawnerCount;
}

public class WaveController : IDisposable, IStartable
{
    private readonly CancellationTokenSource _cts;

    private readonly WaveStatus _waveStatus;
    private readonly WaveSettings _waveSettings;
    private readonly SpawnerStatus _spawnerStatus;
    private readonly CreatureSpawnerController _creatureSpawnerController;

    public bool IsSpawnerTargetReached =>
        _waveStatus.AlreadyCreatedSpawnerCount >= _waveSettings.CreatedSpawnerTargetAmount;

    public event Action StartingWaveEvent;
    public event Action EndWaveEvent;

    public WaveController(WaveStatus waveStatus,
        SpawnerStatus spawnerStatus,
        CreatureSpawnerController creatureSpawnerController,
        WaveSettings waveSettings)
    {
        _cts = new();

        _waveStatus = waveStatus;
        _waveSettings = waveSettings;
        _spawnerStatus = spawnerStatus;
        _creatureSpawnerController = creatureSpawnerController;

        _creatureSpawnerController.OnPotentialWaveEndEvent += OnWaveOver;
    }

    public void Start()
    {
        UniTask.Void(
            async ct =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_waveSettings.StartCalmPeriodSeconds), cancellationToken: ct)
                    .SuppressCancellationThrow();

                do
                {
                    StartWave();

                    await UniTask.WaitUntil(() => _waveStatus.IsWaveActive is false, cancellationToken: ct)
                        .SuppressCancellationThrow();

                    Debug.LogWarning(
                        $"Wave {_waveStatus.TotalWaveCount} has ended | Calm period of {_waveSettings.CalmPeriodDurationSeconds} seconds");

                    await UniTask.Delay(TimeSpan.FromSeconds(_waveSettings.CalmPeriodDurationSeconds),
                            cancellationToken: ct)
                        .SuppressCancellationThrow();
                } while (ct.IsCancellationRequested is false);
            },
            _cts.Token);
    }

    private void StartWave()
    {
        _waveStatus.TotalWaveCount += 1;
        _waveStatus.IsWaveActive = true;

        Debug.LogWarning($"Starting Wave {_waveStatus.TotalWaveCount}");
        StartingWaveEvent?.Invoke();
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

        while (ct.IsCancellationRequested is false && IsSpawnerTargetReached is false)
        {
            var activeSpawnerCount = _spawnerStatus.ActiveSpawnerCount;
            var simultaneousSpawnerTargetAmount = _waveSettings.ExistingSpawnerTargetAmount;

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
                                     Mathf.InverseLerp(0, _waveSettings.ExistingSpawnerTargetAmount,
                                         _spawnerStatus.ActiveSpawnerCount);
        var existingEnemiesWeight = 1 - Mathf.InverseLerp(0, _waveSettings.ExistingCreaturesTargetAmount,
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
        if (!_spawnerStatus.IsWaveActive && IsSpawnerTargetReached)
        {
            _waveStatus.IsWaveActive = false;
            _waveStatus.AlreadyCreatedSpawnerCount = 0;
            EndWaveEvent?.Invoke();
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}