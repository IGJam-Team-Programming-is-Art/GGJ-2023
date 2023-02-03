using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class Well : MonoBehaviour
{
    private UniTask _replenishTask;
    private CancellationTokenSource _tokenSource;

    [SerializeField] private double _replenishIntervalSeconds = 3d;
    [field: SerializeField] public int ReplenishAmount { get; private set; } = 10;
    
    [field: SerializeField] public int WaterLevel { get; private set; } = 100;
    [field: SerializeField] public int WaterLevelMax { get; private set; } = 100;

    public bool CanTakeWater => WaterLevel is not 0;

    private void OnEnable()
    {
        _tokenSource = new();
        _replenishTask = ReplenishUpdate(_tokenSource.Token);
    }

    private void OnDisable()
    {
        _tokenSource.Cancel();
        _tokenSource.Dispose();
    }
    
    /// <summary>
    /// Replenishes well every interval
    /// </summary>
    /// <param name="ct"></param>
    private async UniTask ReplenishUpdate(CancellationToken ct)
    {
        while (ct.IsCancellationRequested is false)
        {
            if (await UniTask.Delay(TimeSpan.FromSeconds(_replenishIntervalSeconds), DelayType.DeltaTime, cancellationToken: ct).SuppressCancellationThrow())
                return;

            if (WaterLevel == WaterLevelMax)
                continue;

            var waterLevelOld = WaterLevel;
            WaterLevel = math.min(WaterLevelMax, WaterLevel + ReplenishAmount);
            Debug.Log($"Replenishing well with {ReplenishAmount} | {waterLevelOld} > {WaterLevel}");
        }
    }

    /// <summary>
    /// Takes water from well
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>Returns false when no water is left</returns>>
    /// <seealso cref="CanTakeWater"/>
    /// <returns></returns>
    public (bool success, int takenAmount) TryTakeWater(int amount)
    {
        if (CanTakeWater is false)
            return (false, 0);
        
        var diff = WaterLevel - amount;
        var takenAmount = diff switch
        {
            < 0 => amount - math.abs(diff),
            _ => amount
        };
        
        return (true, takenAmount);
    }
}