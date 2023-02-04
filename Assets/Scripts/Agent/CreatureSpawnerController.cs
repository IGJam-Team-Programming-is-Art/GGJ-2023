using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using Random = UnityEngine.Random;

public class CreatureSpawnerController : MonoBehaviour
{
    [Inject] private CreatureDataCollection _createDataCollection;
    [Inject] private CreatureSpawnerCollection _creatureSpawnerCollection;
    [Inject] private PlayerReferences _playerReferences;
    [Inject] private TargetAssignmentController _targetAssignmentController;

    private ObjectPool<CreatureSpawner> _spawnerPools;
    private readonly Dictionary<Creature, ObjectPool<CreatureBehaviour>> _creaturePools = new(4);

    private void Awake()
    {
        //Generate pool of spawners
        _spawnerPools = new(
            () =>
            {
                var spawner = Instantiate(_creatureSpawnerCollection.TEST_SpawnerPrefab);

                spawner.OnDisableEvent += OnSpawnerDisabled;
                return spawner;
            },
            actionOnRelease: spawner => spawner.enabled = false);

        //Generate pool for each creature type
        foreach (var creatureData in _createDataCollection.Creatures)
        {
            var pool = new ObjectPool<CreatureBehaviour>(() =>
            {
                var creature = Instantiate(creatureData.Prefab).GetComponent<CreatureBehaviour>();
                creature.gameObject.SetActive(false);
                creature.TargetAssignmentController = _targetAssignmentController;

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
    [ContextMenu("Create spawner")]
    public void CreateSpawner()
    {
        var spawnerPosition = new Vector3(5f, 0f, 5f);

        var spawner = _spawnerPools.Get();

        var creaturesList = _createDataCollection.Creatures;
        var creatureData = creaturesList[Random.Range(0, creaturesList.Count)];
        var creatureType = creatureData.Type;

        spawner.transform.position = spawnerPosition;
        spawner.CreaturePool = _creaturePools[creatureType];
        spawner.CreatureType = creatureType;

        spawner.enabled = true;
        spawner.gameObject.SetActive(true);
        Debug.Log($"TEST_SpawnerPrefab of enemy type {spawner} created!");
    }

    private void OnSpawnerDisabled(CreatureSpawner spawner)
    {
        Debug.Log("TEST_SpawnerPrefab released!");
        _spawnerPools.Release(spawner);
    }
}

public enum Creature
{
    None,
    Melee
}