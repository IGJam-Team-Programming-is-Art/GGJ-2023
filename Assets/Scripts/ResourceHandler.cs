using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class ResourceHandler : MonoBehaviour
{
     [SerializeField] private List<ResourceStack> EDITOR_Resource = new(2);
#if UNITY_EDITOR
    private void LateUpdate()
    {
        EDITOR_Resource = _resources.Values.ToList();
    }
#endif
    
    private readonly Dictionary<Resource, ResourceStack> _resources = new(2);
    
    private bool TryGetResourceStack(Resource resource, out ResourceStack stack) => _resources.TryGetValue(resource, out stack);

    /// <summary>
    /// Modifies resource stack
    /// </summary>
    /// <param name="stack">Can be positive and negative</param>
    /// <returns>Returns a boolean for success and the actual modified amount</returns>
    public (bool success, int modifiedAmount) TryModify(ResourceStack stack)
    {
        //TODO: Get max amount somewhere
        const int MAX_RESOURCE_AMOUNT = 100;

        var newAmount = _resources.TryGetValue(stack.Type, out var oldStack) switch
        {
            true => math.clamp(oldStack.Amount + stack.Amount, 0, MAX_RESOURCE_AMOUNT),
            false => stack.Amount,
        };
        
        //Nothing changed, modification failed
        if (newAmount == oldStack.Amount)
            return (false, 0);
        
        _resources[stack.Type] = new()
        {
            Type = oldStack.Type,
            Amount = newAmount,
        };
        
        //Success with diff
        return (true, newAmount - oldStack.Amount);
    }
}