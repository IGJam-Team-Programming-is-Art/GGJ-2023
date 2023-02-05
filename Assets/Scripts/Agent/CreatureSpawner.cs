using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CreatureSpawner : MonoBehaviour
{
    private CancellationTokenSource _cts;

    [SerializeField] private SpawnerSettings _spawnerSettings;

    public Creature CreatureType;
    public ObjectPool<CreatureBehaviour> CreaturePool;

    [FormerlySerializedAs("SpawnedCreatureCount")]
    public int CreatureSpawnedCount;

    public event Action OnCreatureSpawnEvent;
    public event Action<CreatureSpawner> OnDisableEvent;

    private void OnEnable()
    {
        if (CreatureType is Creature.None)
            Debug.LogError("No creature type assigned");

        _cts = new();
        var token = _cts.Token;
        SpawnUpdate(token).Forget();
    }

    private void OnDisable()
    {
        _cts?.Cancel();
        _cts?.Dispose();

        CreatureSpawnedCount = 0;
        CreatureType = Creature.None;

        OnDisableEvent?.Invoke(this);
    }

    private async UniTaskVoid SpawnUpdate(CancellationToken ct)
    {
        var stopSpawning = false;
        while (_cts.IsCancellationRequested is false && stopSpawning is false)
        {
            var randomVariation = Random.Range(-1 * _spawnerSettings.SpawnPulseIntervalVariation,
                _spawnerSettings.SpawnPulseIntervalVariation);

            await UniTask.Delay(TimeSpan.FromSeconds(_spawnerSettings.SpawnPulseInterval + randomVariation),
                cancellationToken: ct).SuppressCancellationThrow();

            if (ct.IsCancellationRequested)
                return;

            var amount = Random.Range(_spawnerSettings.AmountPerPulseRangeStart,
                _spawnerSettings.AmountPerPulseRangeEnd + 1);
            for (var i = 0; i < amount; i++)
            {
                var creature = CreaturePool.Get();
                creature.transform.position = transform.position + Random.insideUnitCircle.to3D() * 2f;

                creature.enabled = true;
                creature.gameObject.SetActive(true);

                CreatureSpawnedCount += 1;
                OnCreatureSpawnEvent?.Invoke();

                stopSpawning = CreatureSpawnedCount >= _spawnerSettings.MaxCreatureSpawnAmount;
                if (stopSpawning)
                    break;
            }
        }

        Debug.Log($"Spawner {gameObject} has spawned {CreatureSpawnedCount} of type {CreatureType}");
        enabled = false;
    }
}