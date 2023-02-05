using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using VContainer;
using Random = UnityEngine.Random;

public class SpawnerStatus
{
    public int ActiveSpawnerCount;
    public int ActiveCreatureCount;

    public (float Tree, float Plant) ClosestSpawnDistance = (10f, 5f);
    public (float Tree, float Plant) MaxSpawnDistance = (30f, 10f);

    public bool IsWaveActive => ActiveSpawnerCount is not 0 || ActiveCreatureCount is not 0;
}

public class CreatureSpawnerController : MonoBehaviour
{
    [Inject] private SpawnerStatus _spawnerStatus;

    [Inject] private CreatureDataCollection _createDataCollection;
    [Inject] private CreatureSpawnerCollection _creatureSpawnerCollection;
    [Inject] private PlayerReferences _playerReferences;
    [Inject] private TargetAssignmentController _targetAssignmentController;

    private ObjectPool<CreatureSpawner> _spawnerPools;
    private readonly Dictionary<Creature, ObjectPool<CreatureBehaviour>> _creaturePools = new(4);

    private int _creatureInstanceCounter;

    /// <summary>
    /// Triggered when creature count or spawner count goes down
    /// </summary>
    public event Action OnPotentialWaveEndEvent;

    private void Awake()
    {
        //Generate pool of spawners
        _spawnerPools = new(
            () =>
            {
                var spawner = Instantiate(_creatureSpawnerCollection.TEST_SpawnerPrefab);

                spawner.OnCreatureSpawnEvent += OnCreatureSpawned;
                spawner.OnDisableEvent += OnSpawnerDisabled;
                return spawner;
            },
            actionOnRelease: spawner => spawner.gameObject.SetActive(false));

        //Generate pool for each creature type
        foreach (var creatureData in _createDataCollection.Creatures)
        {
            var pool = new ObjectPool<CreatureBehaviour>(() =>
            {
                var creature = Instantiate(creatureData.Prefab).GetComponent<CreatureBehaviour>();

                creature.name = $"{creature.name}_{_creatureInstanceCounter}";
                creature.gameObject.SetActive(false);

                creature.DeathEvent += OnCreatureDied;
                creature.TargetAssignmentController = _targetAssignmentController;

                _creatureInstanceCounter += 1;
                return creature;
            }, actionOnRelease: creature => creature.gameObject.SetActive(false));

            if (_creaturePools.TryAdd(creatureData.Type, pool))
                continue;

            Debug.LogError($"Creature of type {creatureData.Type} already has pool!");
            pool.Dispose();
        }
    }


    /// <summary>
    ///     Creates spawner that spawns enemies at some position
    /// </summary>
    /// <param name="aggro">Between 0 and 1. Non-aggro and very aggro</param>
    [ContextMenu("Create spawner")]
    public void CreateSpawner(float aggro)
    {
        var spawner = _spawnerPools.Get();

        var creaturesList = _createDataCollection.Creatures;
        var creatureData = creaturesList[Random.Range(0, creaturesList.Count)];
        var creatureType = creatureData.Type;

        spawner.transform.position = GetSuitableSpawnerPosition(aggro).to3D();
        spawner.CreaturePool = _creaturePools[creatureType];
        spawner.CreatureType = creatureType;

        spawner.enabled = true;
        spawner.gameObject.SetActive(true);
        _spawnerStatus.ActiveSpawnerCount += 1;

        Debug.Log($"Spawner of enemy type {spawner} created! Aggro > {aggro}");
    }

    /// <summary>
    /// Get suitable spawn position for spawner based on aggro value
    /// </summary>
    /// <param name="aggro"></param>
    /// <returns></returns>
    private Vector2 GetSuitableSpawnerPosition(float aggro)
    {
        var tries = 0;
        Vector2 selectedPosition;
        do
        {
            var targetRange = Mathf.Lerp(_spawnerStatus.ClosestSpawnDistance.Tree, _spawnerStatus.MaxSpawnDistance.Tree,
                aggro);

            var angle = Random.Range(0f, Mathf.PI * 2);
            var x = Mathf.Sin(angle) * targetRange;
            var y = Mathf.Cos(angle) * targetRange;

            selectedPosition = new(x, y);
            tries += 1;
        } while (NavMesh.SamplePosition(selectedPosition, out var _, 3f, NavMesh.AllAreas) is false && tries < 100);

        if (tries >= 100) Debug.LogError("Tried 100 times to find suitable spawner position! Failed!");

        return selectedPosition;
    }

    private void OnSpawnerDisabled(CreatureSpawner spawner)
    {
        _spawnerStatus.ActiveSpawnerCount -= 1;
        OnPotentialWaveEndEvent?.Invoke();

        Debug.Log($"{spawner.gameObject} released!");
        _spawnerPools.Release(spawner);
    }

    private void OnCreatureSpawned()
    {
        _spawnerStatus.ActiveCreatureCount += 1;
    }

    private void OnCreatureDied(CreatureBehaviour creature)
    {
        _spawnerStatus.ActiveCreatureCount -= 1;
        OnPotentialWaveEndEvent?.Invoke();

        Debug.Log($"{creature.gameObject} died!");
        _creaturePools[creature.CreatureType].Release(creature);
    }
}

[Serializable]
public enum Creature
{
    None,
    Ranged
}