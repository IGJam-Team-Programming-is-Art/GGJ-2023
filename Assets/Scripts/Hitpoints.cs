using System;
using Unity.Mathematics;
using UnityEngine;

public class Hitpoints : MonoBehaviour
{
    [field: SerializeField] public int Current { get; private set; } = 100;
    [field: SerializeField] public int Max { get; private set; } = 100;

    public event Action<int> OnModify;
    public event Action OnDeath;

    private void OnValidate()
    {
        if (Application.isPlaying)
            CheckHitpoints();
    }

    /// <summary>
    /// Modifies hit points of gameObject
    /// Positive values heal, negative values damage
    /// </summary>
    /// <param name="value"></param>
    public void Modify(int value)
    {
        Current = math.min(Current + value, Max);
        OnModify?.Invoke(value);

        CheckHitpoints();
    }

    private void CheckHitpoints()
    {
        if (Current is not 0)
            return;

        Debug.LogWarning("Destroying entity");
        OnDeath?.Invoke();
    }

    public void ResetHitpoints()
    {
        Current = Max;
    }
}