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

    public float SpawnPulseInterval = 3f;
    public float SpawnPulseIntervalVariation = 0.3f;
    public Range AmountPerPulse = new(1, 3);

    [FormerlySerializedAs("SpawnedCreatureCount")]
    public int CreatureSpawnedCount;

    public int MaxCreatureSpawnAmount = 10;

    public Creature CreatureType;
    public ObjectPool<CreatureBehaviour> CreaturePool;

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
            var randomVariation = Random.Range(-1 * SpawnPulseIntervalVariation, SpawnPulseIntervalVariation);

            await UniTask.Delay(TimeSpan.FromSeconds(SpawnPulseInterval + randomVariation),
                cancellationToken: ct).SuppressCancellationThrow();

            if (ct.IsCancellationRequested)
                return;

            var amount = Random.Range(AmountPerPulse.Start.Value, AmountPerPulse.End.Value + 1);
            for (var i = 0; i < amount; i++)
            {
                var creature = CreaturePool.Get();
                creature.transform.position = transform.position + Random.insideUnitCircle.to3D() * 2f;

                creature.enabled = true;
                creature.gameObject.SetActive(true);

                CreatureSpawnedCount += 1;
                OnCreatureSpawnEvent?.Invoke();

                stopSpawning = CreatureSpawnedCount >= MaxCreatureSpawnAmount;
                if (stopSpawning)
                    break;
            }
        }

        Debug.Log($"Spawner {gameObject} has spawned {CreatureSpawnedCount} of type {CreatureType}");
        enabled = false;
    }
}